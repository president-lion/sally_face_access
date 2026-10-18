@echo off
setlocal

REM ---------------------------------------------------------------------------
REM Builds the two release files into release\:
REM   Setup sally_face_access.exe            - the setup program
REM   sally_face_access_portable.zip         - unzip into the game folder by hand
REM ---------------------------------------------------------------------------

set "HERE=%~dp0"
set "REL=%HERE%release"
set "STAGE=%REL%\portable"

dotnet build "%HERE%src\SallyFaceAccess" -c Release -p:SkipDeploy=true || exit /b 1
call "%HERE%installer\build_setup.bat" || exit /b 1

if exist "%REL%" rmdir /S /Q "%REL%"
mkdir "%STAGE%\Mods" "%STAGE%\UserLibs" "%STAGE%\licenses"
copy /Y "%HERE%bin\Setup sally_face_access.exe" "%REL%\" >nul
copy /Y "%HERE%src\SallyFaceAccess\bin\Release\SallyFaceAccess.dll" "%STAGE%\Mods\" >nul
copy /Y "%HERE%src\prism\build-x64\prism.dll" "%STAGE%\UserLibs\" >nul
copy /Y "%HERE%README.md" "%STAGE%\Sally Face Access README.md" >nul
copy /Y "%HERE%LICENSE" "%STAGE%\licenses\Sally Face Access LICENSE.txt" >nul
copy /Y "%HERE%THIRD-PARTY.txt" "%STAGE%\licenses\" >nul
copy /Y "%HERE%src\prism\LICENSE" "%STAGE%\licenses\prism LICENSE.txt" >nul

powershell -NoProfile -ExecutionPolicy Bypass -Command ^
    "Add-Type -AssemblyName System.IO.Compression.FileSystem;" ^
    "[System.IO.Compression.ZipFile]::ExtractToDirectory('%HERE%tools\melonloader\MelonLoader.x64.zip', '%STAGE%');" ^
    "[System.IO.Compression.ZipFile]::CreateFromDirectory('%STAGE%', '%REL%\sally_face_access_portable.zip', 'Optimal', $false)" || exit /b 1
rmdir /S /Q "%STAGE%"

echo.
echo Done:
dir /B "%REL%"
