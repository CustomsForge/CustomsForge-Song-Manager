REM use this batch file if the Configuration Manager PublishBeta ftp hangs/fails
REM better yet ... use FileZilla FTP GUI program

@ECHO OFF
COLOR 0A 
CLS

if errorlevel 1 goto BatchFailed

SET batchPath=%~dp0
ECHO Current batch path: %batchpath%
ECHO.

Start CFSMPostBuilder.exe CREATEVERSIONINFO

ECHO Created VersionInfo.txt ... Press any key to continue
Pause > nul
@ECHO on
ENDLOCAL
EXIT /b 0

:BatchFailed
ECHO ERROR: Batch failed ...
PAUSE

