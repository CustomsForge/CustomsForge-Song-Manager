#include "ISPPBuiltins.iss"
/////////////////////////////////////////////////////////////////////
#define AppGUID "58F35625-541C-493A-A289-4B2D362DAFE0"
#define AppWebsite "http://customsforge.com/"
#define AppExeName "CustomsForgeSongManager.exe"
#define AppCopyright "CF © 2017"
#define AppCompany "CustomsForge"
#define InstallGroup "CustomsForge Song Manager"; // Start Menu
#define InstallDir "CustomsForgeSongManager"; // Programs sub folder
/////////////////////////////////////////////////////////////////////
#ifdef BETA
#define BaseUpdateURL "http://appdev.cfmanager.com/beta"
#define BuildPath SourcePath +"..\..\CustomsForgeSongManager\bin\ConfuserEx\"
#define OutputDirExe SourcePath +"..\..\ThirdParty\CFSM_Beta_Setup\"
#define AppTitle "CustomsForge Song Manager - Beta Version"
#define InstallerName "CFSMSetup"
#else
#define DOUPDATE
#define BaseUpdateURL "http://appdev.cfmanager.com"
#define BuildPath SourcePath +"..\..\CustomsForgeSongManager\bin\ConfuserEx\"
#define OutputDirExe SourcePath +"..\..\ThirdParty\CFSM_Release_Setup\"
#define AppTitle "CustomsForge Song Manager - Release Version"
#define InstallerName "CFSMSetup"
#endif
/////////////////////////////////////////////////////////////////////
#define LatestVersionDowload BaseUpdateURL + "/CFSMSetup.rar"
#define VersionInfoLocation BaseUpdateURL + "/VersionInfo.txt"
#define AppVersion GetFileVersion(BuildPath + AppExeName)


