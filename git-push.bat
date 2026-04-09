@echo off
setlocal
chcp 65001 >nul
cd /d "%~dp0"

echo === Git push ===
echo Carpeta: %CD%
echo.

git rev-parse --is-inside-work-tree >nul 2>&1
if errorlevel 1 (
  echo ERROR: Esta carpeta no es un repositorio Git.
  pause
  exit /b 1
)

for /f "delims=" %%b in ('git branch --show-current 2^>nul') do set "RAMA=%%b"
if "%RAMA%"=="" (
  echo ERROR: No hay rama activa ^(¿hay commits?^).
  pause
  exit /b 1
)

echo Rama: %RAMA%
git remote -v 2>nul | findstr /r "." >nul
if errorlevel 1 (
  echo.
  echo No hay remoto configurado. Una sola vez ejecutá:
  echo   git remote add origin https://github.com/TU_USUARIO/TU_REPO.git
  echo.
  pause
  exit /b 1
)

echo.
git push -u origin "%RAMA%"
set "ERR=%ERRORLEVEL%"

if not "%ERR%"=="0" (
  echo.
  echo ERROR: git push falló ^(código %ERR%^).
  echo.
  echo Revisá: internet, que el remoto exista, y el inicio de sesión en GitHub
  echo ^(token, SSH o Git Credential Manager^).
  pause
  exit /b %ERR%
)

echo.
echo Listo: cambios subidos a origin / %RAMA%
pause
