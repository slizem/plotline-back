@echo off
chcp 65001 >nul
echo starting PostgreSQL...

docker --version >nul 2>&1
if errorlevel 1 (
    echo Docker not installed!
    pause
    exit /b 1
)

netstat -ano | findstr :5432 >nul
if not errorlevel 1 (
    echo Port 5432 is already in use!
    echo Checking if it's our container...
    docker ps --filter "name=plotlinedb-container" --format "{{.Names}}" | findstr "plotlinedb-container" >nul
    if errorlevel 1 (
        echo Port 5432 is used by another process!
        pause
        exit /b 1
    ) else (
        echo Container plotlinedb-container is already running
        pause
        exit /b 0
    )
)

echo Starting PostgreSQL container...
docker-compose up -d

if errorlevel 1 (
    echo Error starting container!
    pause
    exit /b 1
)

echo.
echo [SUCCESS] PostgreSQL is running!
echo.
echo Connection info:
echo    Host:     localhost
echo    Port:     5432
echo    Database: 
echo    Username: 
echo    Password: 
echo.
echo Commands:
echo    Logs:     docker-compose logs -f
echo    Stop:     stop-db.bat
echo    Status:   docker-compose ps
echo.
echo Open DBeaver and connect to localhost:5432
pause