@ECHO off
COLOR 0A 
CLS
SETLOCAL enabledelayedexpansion

if errorlevel 1 goto BatchFailed

SET batchpath=%~dp0
ECHO Current batch path: %batchpath%

REM rootproject is up one directory
CD ..
SET rootproject=%cd%
ECHO Root Project path is : %rootproject%
ECHO.
ECHO This batch file copies dll libraries and support files from the
ECHO libs folder to the project folder to keep the project up-to-date.
ECHO.
ECHO Press Ctrl-C to abort or any key to continue
PAUSE>nul

REM @ECHO on

XCOPY /Y/E/D "%rootproject%\libs\RocksmithToolkitLib.Config.xml" "%rootproject%\CustomsForgeSongManager\RocksmithToolkitLib.Config.xml" 
XCOPY /Y/E/D "%rootproject%\libs\RocksmithToolkitlib.SongAppId.xml" "%rootproject%\CustomsForgeSongManager\RocksmithToolkitlib.SongAppId.xml" 
XCOPY /Y/E/D "%rootproject%\libs\RocksmithToolkitlib.TuningDefinition.xml" "%rootproject%\CustomsForgeSongManager\RocksmithToolkitlib.TuningDefinition.xml" 
XCOPY /Y/E/D "%rootproject%\libs\7za.dll" "%rootproject%\CustomsForgeSongManager\7za.dll" 
XCOPY /Y/E/D "%rootproject%\libs\DF_DDSImage.dll" "%rootproject%\CustomsForgeSongManager\DF_DDSImage.dll" 
XCOPY /Y/E/D "%rootproject%\libs\bass.dll" "%rootproject%\CustomsForgeSongManager\bass.dll" 

ECHO Done updating project dll libraries
PAUSE >nul
@ECHO on
ENDLOCAL
EXIT /b 0

:BatchFailed
ECHO ERROR: Batch failed ...
PAUSE
