using Microsoft.EntityFrameworkCore;
using SistemaGestionVentas.Data;
using SistemaGestionVentas.Models;

namespace SistemaGestionVentas.Services;

public record LowStockRow(int Id, string Name, int Stock, int MinThreshold);

public record MovementStatRow(int ProductId, string ProductName, int TotalUnits);

public record MovementHistoryRow(
    DateTime OccurredAt,
    string ProductName,
    string TypeLabel,
    int Quantity,
    decimal UnitSalePrice,
    decimal UnitCostSnapshot,
    string? Note);

public record MarginRow(
    int ProductId,
    string ProductName,
    decimal TotalRevenue,
    decimal TotalCostOfSoldUnits,
    decimal Profit,
    decimal RoiPercentOnCost);

public class ReportingService
{
    private readonly DbContextOptions<AppDbContext> _options;

    public ReportingService(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    public List<LowStockRow> GetLowStock()
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking()
            .Where(p => p.StockQuantity <= p.MinStockThreshold)
            .OrderBy(p => p.StockQuantity)
            .Select(p => new LowStockRow(p.Id, p.Name, p.StockQuantity, p.MinStockThreshold))
            .ToList();
    }

    public decimal GetInventoryTotalValue()
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking()
            .Sum(p => (decimal?)p.StockQuantity * p.CostPrice) ?? 0m;
    }

    /// <summary>Unidades vendidas por producto (solo movimientos tipo Venta).</summary>
    public List<MovementStatRow> GetTopSold(int take = 20)
    {
        using var ctx = new AppDbContext(_options);
        return (
            from m in ctx.StockMovements.AsNoTracking()
            join p in ctx.Products.AsNoTracking() on m.ProductId equals p.Id
            where m.Type == MovementType.Venta
            group m by new { p.Id, p.Name } into g
            select new MovementStatRow(g.Key.Id, g.Key.Name, g.Sum(x => x.Quantity))
        ).OrderByDescending(x => x.TotalUnits).Take(take).ToList();
    }

    /// <summary>Total de unidades en movimientos que sacan stock (salida, venta, ajuste negativo).</summary>
    public List<MovementStatRow> GetTopOutgoingMovementVolume(int take = 20)
    {
        var outgoing = new[] { MovementType.Salida, MovementType.Venta, MovementType.AjusteNegativo };
        using var ctx = new AppDbContext(_options);
        return (
            from m in ctx.StockMovements.AsNoTracking()
            join p in ctx.Products.AsNoTracking() on m.ProductId equals p.Id
            where outgoing.Contains(m.Type)
            group m by new { p.Id, p.Name } into g
            select new MovementStatRow(g.Key.Id, g.Key.Name, g.Sum(x => x.Quantity))
        ).OrderByDescending(x => x.TotalUnits).Take(take).ToList();
    }

    public List<MovementHistoryRow> GetMovementHistory(int? productId, DateTime? fromUtc, DateTime? toUtc)
    {
        using var ctx = new AppDbContext(_options);
        var q = ctx.StockMovements.AsNoTracking()
            .Include(m => m.Product)
            .AsQueryable();

        if (productId is int pid)
            q = q.Where(m => m.ProductId == pid);
        if (fromUtc is DateTime f)
            q = q.Where(m => m.OccurredAt >= f);
        if (toUtc is DateTime t)
            q = q.Where(m => m.OccurredAt <= t);

        return q.OrderByDescending(m => m.OccurredAt)
            .AsEnumerable()
            .Select(m => new MovementHistoryRow(
                m.OccurredAt,
                m.Product.Name,
                MovementTypeLabel(m.Type),
                m.Quantity,
                m.UnitSalePrice,
                m.UnitCostSnapshot,
                m.Note))
            .ToList();
    }

    public List<MarginRow> GetMarginByProduct()
    {
        using var ctx = new AppDbContext(_options);
        var sales = (
            from m in ctx.StockMovements.AsNoTracking()
            join p in ctx.Products.AsNoTracking() on m.ProductId equals p.Id
            where m.Type == MovementType.Venta
            group m by new { p.Id, p.Name } into g
            select new
            {
                g.Key.Id,
                g.Key.Name,
                Revenue = g.Sum(x => x.UnitSalePrice * x.Quantity),
                Cost = g.Sum(x => x.UnitCostSnapshot * x.Quantity)
            }).ToList();

        return sales
            .Select(s =>
            {
                var profit = s.Revenue - s.Cost;
                var roi = s.Cost > 0 ? profit / s.Cost * 100m : 0m;
                return new MarginRow(s.Id, s.Name, s.Revenue, s.Cost, profit, roi);
            })
            .OrderByDescending(r => r.Profit)
            .ToList();
    }

    private static string MovementTypeLabel(MovementType t) => t switch
    {
        MovementType.Entrada => "Entrada",
        MovementType.Salida => "Salida",
        MovementType.Venta => "Venta",
        MovementType.AjustePositivo => "Ajuste +",
        MovementType.AjusteNegativo => "Ajuste −",
        _ => t.ToString()
    };
}
