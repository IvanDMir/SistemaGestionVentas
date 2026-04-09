namespace CatalogoWeb.Services;

public static class MercadoLibreUrlValidator
{
    public static bool IsValidMercadoLibreUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
            return false;

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            return false;

        var host = uri.Host.ToLowerInvariant();
        return host.Contains("mercadolibre", StringComparison.OrdinalIgnoreCase)
               || host.Contains("mercadolivre", StringComparison.OrdinalIgnoreCase)
               || host.Contains("mercadopago", StringComparison.OrdinalIgnoreCase);
    }
}
