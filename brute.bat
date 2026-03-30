@echo off
setlocal enabledelayedexpansion

if not exist ".env" (
    echo Error: .env file not found
    exit /B 1
)

for /f "usebackq tokens=1,* delims==" %%A in (".env") do (
    if /I "%%A"=="GRAPH_DIRECTORY" set "GRAPH_DIRECTORY=%%~B"
)

if "!GRAPH_DIRECTORY!"=="" (
    echo Error: GRAPH_DIRECTORY is missing in .env
    exit /B 1
)

REM Check if the first argument (%1) is empty
if "%1"=="" (
    echo Error: Missing argument
    echo Usage: %0 ^<argument^>
    exit /B 1
)

REM For when I inevitably accidentally run this with an extra zero
if %1 gtr 12 (
    echo Error: Brute force only supports up to 12 nodes
    echo Usage: %0 ^<argument^>
    exit /B 1
)

set param=%1
set "graph_file=!GRAPH_DIRECTORY!!param!.graph"

@REM dotnet run --project Bruteforce/Bruteforce.csproj -- "!graph_file!"

@REM dotnet run restores the project every time and is slow
.\Bruteforce\bin\Debug\net9.0\Bruteforce.exe "!graph_file!"
