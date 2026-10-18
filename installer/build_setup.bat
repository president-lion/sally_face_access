@echo off
setlocal

REM ---------------------------------------------------------------------------
REM Builds "Setup sally_face_access.exe": one file carrying MelonLoader, the mod
REM and Prism as embedded resources.
REM
REM Needs, before running:
REM   ..\src\SallyFaceAccess\bin\Release\SallyFaceAccess.dll   (dotnet build -c Release)
REM   ..\src\prism\build-x64\prism.dll
REM   ..\tools\melonloader\MelonLoader.x64.zip                 (MelonLoader v0.7.3 release)
REM
REM Uses the C# compiler in .NET Framework 4, which every Windows 10 and 11 has.
REM ---------------------------------------------------------------------------

set "HERE=%~dp0"
set "ROOT=%HERE%.."
set "OUT=%ROOT%\bin\Setup sally_face_access.exe"
set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"

set "MOD=%ROOT%\src\SallyFaceAccess\bin\Release\SallyFaceAccess.dll"
set "PRISM=%ROOT%\src\prism\build-x64\prism.dll"
set "ML=%ROOT%\tools\melonloader\MelonLoader.x64.zip"

for %%F in ("%MOD%" "%PRISM%" "%ML%" "%CSC%") do (
    if not exist %%F (
        echo ERROR: missing %%~F
        exit /b 1
    )
)

if not exist "%ROOT%\bin" mkdir "%ROOT%\bin"
"%CSC%" /nologo /target:winexe /platform:anycpu /optimize+ ^
    /out:"%OUT%" ^
    /reference:System.dll /reference:System.Drawing.dll /reference:System.Windows.Forms.dll ^
    /reference:System.IO.Compression.dll /reference:System.IO.Compression.FileSystem.dll ^
    /resource:"%MOD%",SallyFaceAccess.dll ^
    /resource:"%PRISM%",prism.dll ^
    /resource:"%ML%",MelonLoader.x64.zip ^
    "%HERE%SallyFaceSetup.cs" || exit /b 1

echo Built: %OUT%
