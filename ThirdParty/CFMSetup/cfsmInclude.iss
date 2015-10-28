;the isbeta.iss tells the compiler to compile it in beta or release mode
#include "isbeta.iss"

#define buildpath SourcePath +"..\..\CustomsForgeManager\bin\InnoInstaller\"
#define AppVersion GetFileVersion(buildpath + "CustomsForgeSongManager.exe")
#define AppURL "http://customsforge.com/"
#define AppExeName "CustomsForgeSongManager.exe"

#define DOUPDATE

#ifdef BETA
#define ApplicationName "CustomsForge Manager Beta"
#define BaseUpdateURL "http://appdev.cfmanager.com/beta"
#define InstallerName "CFSMSetup";
#else
#define ApplicationName "CustomsForge Manager"
#define BaseUpdateURL "http://appdev.cfmanager.com/release"
#define InstallerName "CFSMSetup";
#endif

#define UpdateInfoLocation BaseUpdateURL + "/VersionInfo.txt"
#define LatestVersionDowload BaseUpdateURL + "/CFSMSetup.exe"
