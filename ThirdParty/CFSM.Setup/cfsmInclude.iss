#define buildpath SourcePath +"..\..\CustomsForgeSongManager\bin\ConfuserEx\"
#define AppVersion GetFileVersion(buildpath + "CustomsForgeSongManager.exe")
#define AppURL "http://customsforge.com/"
#define AppExeName "CustomsForgeSongManager.exe"
#define AppName "CustomsForge Song Manager"
#define InstallerName "CFSMSetup";

#define DOUPDATE

#ifdef BETA
#define BaseUpdateURL "http://appdev.cfmanager.com/beta"
#define InstallerCaption "Setup - CustomsForge Song Manager - Beta Version"
#else
#define BaseUpdateURL "http://appdev.cfmanager.com"
#define InstallerCaption "Setup - CustomsForge Song Manager - Release Version"
#endif

#define UpdateInfoLocation BaseUpdateURL + "/VersionInfo.txt"
#define LatestVersionDowload BaseUpdateURL + "/CFSMSetup.rar"
