@echo off
setlocal
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Build-KidGame.ps1"
exit /b %ERRORLEVEL%
