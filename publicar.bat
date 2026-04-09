@echo off
setlocal
chcp 65001 >nul
set "ROOT=%~dp0"
set "PROJ=%ROOT%SistemaGestionVentas\SistemaGestionVentas.csproj"
set "OUT=C:\Publicaciones\SistemaVentas"

if not exist "%PROJ%" (
  echo No se encontró el proyecto:
  echo %PROJ%
  pause
  exit /b 1
)

echo Publicando Release en:
echo %OUT%
echo.
echo Si la app está abierta, Windows bloquea los .dll y falla la copia.
echo Cerrando SistemaGestionVentas.exe si está en ejecución...
taskkill /F /T /IM SistemaGestionVentas.exe >nul 2>&1
timeout /t 1 /nobreak >nul
echo.

dotnet publish "%PROJ%" -c Release -o "%OUT%"
if errorlevel 1 (
  echo.
  echo ERROR: dotnet publish falló.
  pause
  exit /b 1
)

echo.
echo Listo. Ejecutable: %OUT%\SistemaGestionVentas.exe
pause
