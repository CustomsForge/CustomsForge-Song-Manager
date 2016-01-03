#define buildpath SourcePath +"..\..\CustomsForgeManager\bin\InnoInstaller\"
#define AppVersion GetFileVersion(buildpath + "CustomsForgeSongManager.exe")
#define AppURL "http://customsforge.com/"
#define AppExeName "CustomsForgeSongManager.exe"

#define DOUPDATE

#ifdef BETA
#define ApplicationName "CustomsForge Song Manager Beta"
#define BaseUpdateURL "http://appdev.cfmanager.com/beta"
#define InstallerName "CFSMSetup";
#else
#define ApplicationName "CustomsForge Song Manager"
#define BaseUpdateURL "http://appdev.cfmanager.com/release"
#define InstallerName "CFSMSetup";
#endif

#define UpdateInfoLocation BaseUpdateURL + "/VersionInfo.txt"
#define LatestVersionDowload BaseUpdateURL + "/CFSMSetup.exe"
