/////////////////////////////////////////////////////////////////////
#define AppGUID "58F35625-541C-493A-A289-4B2D362DAFE0"
#define AppWebsite "http://customsforge.com/"
#define AppExeName "CustomsForgeSongManager.exe"
#define AppCopyright "CustomsForge © 2017"
#define AppCompany "CustomsForge"
#define InstallGroup "CustomsForge Song Manager"; // Start Menu
#define InstallDir "CustomsForgeSongManager"; // Programs sub folder
#define InstallerName "CFSMSetup"
#define BuildPath SourcePath + "..\..\CustomsForgeSongManager\bin\ConfuserEx\"
#define DOUPDATE

/////////////////////////////////////////////////////////////////////
#ifdef BETA
#define BaseUpdateURL "http://ignition.customsforge.com/cfsm_uploads/beta"
#define OutputDirExe SourcePath + "..\..\ThirdParty\CFSM_Setup_Beta\"
#define AppTitle "CustomsForge Song Manager - Beta Version"
#define LatestVersionDownload BaseUpdateURL + "/CFSMSetupBeta.rar"
#else
#define BaseUpdateURL "http://ignition.customsforge.com/cfsm_uploads"
#define OutputDirExe SourcePath +"..\..\ThirdParty\CFSM_Setup_Release\"
#define AppTitle "CustomsForge Song Manager - Release Version"
#define LatestVersionDownload BaseUpdateURL + "/CFSMSetup.rar"
#endif

/////////////////////////////////////////////////////////////////////
#define VersionInfoLocation BaseUpdateURL + "/VersionInfo.txt"
#define AppVersion GetFileVersion(BuildPath + AppExeName)


