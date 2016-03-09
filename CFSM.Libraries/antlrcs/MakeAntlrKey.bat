@ECHO OFF

COLOR 0A

rem # determine programm files of x86 for 32 and 64 Platform
IF     EXIST "%PROGRAMFILES(x86)%" set prorgammFiles=%PROGRAMFILES(x86)%
IF NOT EXIST "%PROGRAMFILES(x86)%" set prorgammFiles=%PROGRAMFILES%

mkdir ..\..\..\keys\antlr

rem # change the directory path to the sn.exe file below to match your system:
"%prorgammFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn.exe" -k ..\..\..\keys\antlr\Key.snk

pause

@ECHO ON