#define buildpath "..\CustomsForgeManager\bin\x86\ConfuserEx Release\"
#define ConfuserPath buildpath  + "Confused\bin\x86\ConfuserEx Release\"
#define AppVersion GetFileVersion(ConfuserPath + "CustomsForgeSongManager.exe")
#define AppURL "http://customsforge.com/"

[Setup]
AppName=CustomsForge Manager
AppVersion={#AppVersion}
AppId={{58F35625-541C-493A-A289-4B2D362DAFE0}
RestartIfNeededByRun=False
AppPublisher=CustomsForge.com
AppPublisherURL=AppURL
AppSupportURL=AppURL
AppUpdatesURL=AppURL
VersionInfoCompany=CustomsForge.com
DefaultDirName={pf}\CustomForgeManager

[Files]
Source: "{#ConfuserPath}CFMAudioTools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}CFMImageTools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}ClickOnceUninstaller.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}CustomsForgeSongManager.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}DataGridViewAutoFilter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}DLogNet.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}RocksmithToolkitLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}RocksmithToTabLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ConfuserPath}X360.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}Antlr3.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}Antlr4.StringTemplate.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}DF_DDSImage.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}MiscUtil.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}ReleaseNotes.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}RocksmithToolkitLib.Config.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}RocksmithToolkitLib.SongAppId.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}RocksmithToolkitLib.TuningDefinition.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}zlib.net.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Customs Forge Song Manager"; Filename: "{app}\{#ConfuserPath}CustomsForgeSongManager.exe"; WorkingDir: "{app}"; IconFilename: "{app}\{#ConfuserPath}CustomsForgeSongManager.exe"
