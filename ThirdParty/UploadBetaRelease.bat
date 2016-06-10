REM use this batch file if the Configuration Manager PublishBeta ftp hangs/fails
REM better yet ... use FileZilla FTP GUI program

@ECHO OFF
COLOR 0A 
CLS

if errorlevel 1 goto BatchFailed

SET batchPath=%~dp0
ECHO Current batch path: %batchpath%
CD

IF NOT EXIST "CFSM.Setup\Output\VersionInfo.txt"(
ThirdParty\CFSMPostBuilder.exe CREATEVERSIONINFO
)

ThirdParty\CFSMPostBuilder.exe UPLOAD 198.58.96.81/beta/ appdev kvPSF7HL

ECHO Upload completed ... Press any key to continue
Pause > nul
@ECHO on
ENDLOCAL
EXIT /b 0

:BatchFailed
ECHO ERROR: Batch failed ...
PAUSE


