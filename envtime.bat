@echo off
setlocal enabledelayedexpansion
for /f "tokens=2 delims=:. " %%a in ('date /t') do (
    set "hour=%%a"
)
for /f "tokens=1-4 delims=/ " %%a in ("%date%") do (
    set "month=%%a"
    set "day=%%b"
    set "year=%%c"
)
for /f "tokens=1-4 delims=:. " %%a in ('time /t') do (
    set "minute=%%a"
    set "second=%%b"
)
set "current_time=%month%%day%%year%%minute%%second%"
if "!hour!" LSS "10" (
    set "current_time=0!current_time:~1!"
)

echo GIT_COMMIT=%current_time% > .env

