@echo off
cd /d "%~dp0"
dotnet run --project AssignmentTracker.WinForms
if errorlevel 1 pause
