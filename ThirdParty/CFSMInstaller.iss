#define buildpath SourcePath +"..\CustomsForgeManager\bin\InnoInstaller\"
#define AppVersion GetFileVersion(buildpath + "CustomsForgeSongManager.exe")
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
Source: "{#buildpath}CFMAudioTools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}CFMImageTools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}ClickOnceUninstaller.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}CustomsForgeSongManager.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}DataGridViewAutoFilter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}DLogNet.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}RocksmithToolkitLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}RocksmithToTabLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#buildpath}X360.dll"; DestDir: "{app}"; Flags: ignoreversion
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
Name: "{group}\Customs Forge Song Manager"; Filename: "{app}\{#buildpath}CustomsForgeSongManager.exe"; WorkingDir: "{app}"; IconFilename: "{app}\{#buildpath}CustomsForgeSongManager.exe"
