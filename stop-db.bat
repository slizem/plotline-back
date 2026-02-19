@echo off
chcp 65001 >nul
echo Stopping PostgreSQL...

docker ps --filter "name=plotlinedb-container" --format "{{.Names}}" | findstr "plotlinedb-container" >nul
if errorlevel 1 (
    echo Container plotlinedb-container is not running
    goto :check_volume
)

echo Stopping container...
docker-compose down

:check_volume
echo.
echo Checking Docker volumes...
docker volume ls --filter "name=postgres_data" --format "{{.Name}}" | findstr "postgres_data" >nul
if not errorlevel 1 (
    echo Volume 'postgres_data' exists (your data is safe)
) else (
    echo Volume 'postgres_data' not found
)

echo.
echo [SUCCESS] PostgreSQL stopped
echo Data is saved in Docker volume 'postgres_data'
echo.
echo To start again: start-db.bat
echo To DELETE ALL DATA: docker-compose down -v
pause