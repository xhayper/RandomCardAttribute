@echo off
dotnet build RandomCardAttribute --output .output/
RMDIR /s /q production >nul 2>&1
XCOPY .output\RandomCardAttribute.dll production\ /v /y >nul
XCOPY manifest.json production\ /v /y >nul
XCOPY icon.png production\ /v /y >nul
XCOPY README.md production\ /v /y >nul