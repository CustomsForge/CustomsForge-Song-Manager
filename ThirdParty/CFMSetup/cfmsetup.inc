#define buildpath SourcePath +"..\..\CustomsForgeManager\bin\InnoInstaller\"
#define AppVersion GetFileVersion(buildpath + "CustomsForgeSongManager.exe")
#define AppURL "http://customsforge.com/"
#define ApplicationName "CustomsForge Manager"
#define InstallerName "CFSMSetup";

#define DFSERVER
#define DOUPDATE

#ifdef DFServer
#define BaseUpdateURL "http://dfcrs.com/CFSM"
#else
#define BaseUpdateURL "http://customsforge.com/CFSM"
#endif

#define UpdateInfoLocation BaseUpdateURL + "/VersionInfo.txt"   
#define ReleaseNotes BaseUpdateURL + "/ReleaseNotes.txt"
#define LatestVersionDowload BaseUpdateURL + "/CFSMSetup.exe"  