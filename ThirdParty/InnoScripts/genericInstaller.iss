#include "ISPPBuiltins.iss"
#include "genericInclude.iss"
#include "idp.iss"
// #define SHOWDEBUGMSGS

/////////////////////////////////////////////////////////////////////
[Setup]
; TODO: SignTool=signtool
; SignedUninstaller=yes
DirExistsWarning=no
RestartIfNeededByRun=False
SetupLogging=yes
UsePreviousTasks=no
AppName={#AppTitle}
AppVerName={#AppTitle} {#AppVersion}
AppVersion={#AppVersion}
VersionInfoVersion={#AppVersion}
AppCopyright={#AppCopyright}
AppId={#AppGUID}
AppPublisher={#AppCompany}
VersionInfoCompany={#AppCompany}
AppPublisherURL={#AppWebsite}
AppSupportURL={#AppWebsite}
AppUpdatesURL={#AppWebsite}
WizardImageFile=artwork\genericWizard.bmp
WizardSmallImageFile=artwork\genericWizardSmall.bmp
SetupIconFile=artwork\install.ico
UninstallDisplayIcon=artwork\uninstall.ico
DefaultDirName={pf}\{#InstallDir}
DefaultGroupName={#InstallGroup}
OutputBaseFilename={#InstallerName}
OutputDir={#OutputDirExe}

/////////////////////////////////////////////////////////////////////
[Files]
; Give OS write permisions to all app exe and library exe files
Source: {#BuildPath}{#AppExeName}; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full 
Source: artwork\install.ico; DestDir: {app}; Flags: ignoreversion
Source: artwork\uninstall.ico; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}CFSM.AudioTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}CFSM.ImageTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}DF.WinForms.ThemeLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}GenTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}CustomControls.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}CFSM.RSTKLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}bass.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}DataGridViewTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}CFSM.NCalc.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}DLogNet.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}HtmlRenderer.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}HtmlRenderer.WinForms.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}ICSharpCode.SharpZipLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}RocksmithToolkitLib.dll; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}X360.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}Antlr3.Runtime.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}Antlr4.StringTemplate.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}DF_DDSImage.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}MiscUtil.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}7z.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}oggCut.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}oggdec.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}oggenc.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}packed_codebooks.bin; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}packed_codebooks_aoTuV_603.bin; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}revorb.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}ww2ogg.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}SevenZipSharp.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}Newtonsoft.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}ReleaseNotes.txt; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}RocksmithToolkitLib.Config.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}RocksmithToolkitLib.SongAppId.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}RocksmithToolkitLib.TuningDefinition.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#BuildPath}zlib.net.dll; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}D3DX9_42.dll.old; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}D3DX9_42.dll.new; DestDir: {app}; Flags: ignoreversion
Source: {#BuildPath}ReleaseNotes.txt; DestDir: {app}; Flags: ignoreversion
Source: "{#BuildPath}ddc\*"; DestDir: "{app}\ddc"; Flags: ignoreversion recursesubdirs createallsubdirs; Permissions: everyone-full
Source: {srcexe}; DestDir: {app}; DestName: {#InstallerName}.exe; Flags: ignoreversion external; Permissions: everyone-full
Source: unrar.exe; DestDir: {tmp}; Flags: dontcopy
; alternate method of checking for a running process if CheckForMutexes does not work
; Source: processviewer.exe; DestDir: {tmp}; Flags: dontcopy

/////////////////////////////////////////////////////////////////////
[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons};
Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked; OnlyBelowVersion: 0,6.1

///////////////////////////////////////////////////////////////////// 
[Run]
Filename: {app}\{#AppExeName}; Description: "{cm:LaunchProgram, {%AppName}}"; Flags: nowait postinstall skipifsilent

/////////////////////////////////////////////////////////////////////
[Icons]
Name: {group}\{#AppTitle}; Filename: {app}\{#AppExeName}; WorkingDir: {app}; IconFilename: "{app}\install.ico"
Name: {group}\{cm:UninstallProgram,{#AppTitle}}; Filename: {uninstallexe}; IconFilename: "{app}\uninstall.ico"
Name: {commondesktop}\{#AppTitle}; Filename: {app}\{#AppExeName}; WorkingDir: {app}; IconFilename: {app}\{#AppExeName}; Tasks: desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#AppTitle}; Filename: {app}\{#AppExeName}; Tasks: quicklaunchicon

/////////////////////////////////////////////////////////////////////
[InstallDelete]
// cleanup the programs start group
Type: files; Name: "{group}\*";
Type: files; Name: "{group}\*";

/////////////////////////////////////////////////////////////////////
[UninstallDelete]
Type: files; Name: {app}\Installation.log
// Type: filesandordirs; Name: {app}\Template // uninstall user any created files/settings here

/////////////////////////////////////////////////////////////////////
[Code]
const
  WM_COMMAND = $0111;
  CN_BASE = $BC00;
  CN_COMMAND = CN_BASE + WM_COMMAND;
//  
// Global Variables
//
var 
  okToCopyLog, doneUninstall, runningWebUpdate, hasUpgrade: Boolean;
  oldGuid, currentVersion, newVersion : String;
  tmpUpdateLocation, tmpUpdateRarLocation : String;
  DownloadPageId : Integer;
  UninstallPage: TWizardPage;
//
// ============== Display a messagebox (for debugging) ===============
//
procedure DebugMessage(const s: String);
begin
#IFDEF SHOWDEBUGMSGS
  MsgBox(s, mbInformation, MB_OK);
#ENDIF
end;
//
// ============== User Clicks on URL Label, Open App Website ===============
//
procedure URLLabelOnClick(Sender: TObject);
var
  ErrorCode: Integer;
begin
  ShellExec('open', ExpandConstant('{#AppWebsite}'), '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
end;
//
// ============== .NET Framework Installation ===============
//
//http://www.kynosarges.de/DotNetVersion.html
function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1'          .NET Framework 1.1
//    'v2.0'          .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//    'v4.5.1'        .NET Framework 4.5.1
//    'v4.5.2'        .NET Framework 4.5.2
//    'v4.6'          .NET Framework 4.6
//    'v4.6.1'        .NET Framework 4.6.1
//    'v4.6.2'        .NET Framework 4.6.2
//    'v4.7'          .NET Framework 4.7
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
  key, versionKey: string;
  install, release, serviceCount, versionRelease: cardinal;
  success: boolean;
begin
  versionKey := version;
  versionRelease := 0;

  // .NET 1.1 and 2.0 embed release number in version key
  if version = 'v1.1' then begin
    versionKey := 'v1.1.4322';
  end else if version = 'v2.0' then begin
    versionKey := 'v2.0.50727';
  end

  // .NET 4.5 and newer install as update to .NET 4.0 Full
  else if Pos('v4.', version) = 1 then begin
    versionKey := 'v4\Full';
    case version of
      'v4.5':   versionRelease := 378389;
      'v4.5.1': versionRelease := 378675; // 378758 on Windows 8 and older
      'v4.5.2': versionRelease := 379893;
      'v4.6':   versionRelease := 393295; // 393297 on Windows 8.1 and older
      'v4.6.1': versionRelease := 394254; // 394271 before Win10 November Update
      'v4.6.2': versionRelease := 394802; // 394806 before Win10 Anniversary Update
      'v4.7':   versionRelease := 460798; // 460805 before Win10 Creators Update
    end;
  end;

  // installation key group for all .NET versions
  key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + versionKey;

  // .NET 3.0 uses value InstallSuccess in subkey Setup
  if Pos('v3.0', version) = 1 then begin
    success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
  end else begin
    success := RegQueryDWordValue(HKLM, key, 'Install', install);
  end;

  // .NET 4.0 and newer use value Servicing instead of SP
  if Pos('v4', version) = 1 then begin
    success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
  end else begin
    success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
  end;

  // .NET 4.5 and newer use additional value Release
  if versionRelease > 0 then begin
    success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
    success := success and (release >= versionRelease);
  end;

  result := success and (install = 1) and (serviceCount >= service);
end;
/////////////////////////////////////////////////////////////////////
function IsNet4Installed: Boolean;
begin
  if not IsDotNetDetected('v4\Client', 0)
  and not IsDotNetDetected('v4\Full', 0)
  then begin
    MsgBox('This application requires Microsoft .NET Framework 4.0.30319.'#13#13
           'Please install it from the Microsoft website,'#13
           'and then re-run the setup program.', mbInformation, MB_OK);
    Log('Failed to detect .NET Framework 4.0.30319 installation ...');
    Result := False;
  end else
    Log('Successfully detected .NET Framework 4.0.30319 installation ... ');
    Result := True;
end;
//
// ============== Compare Current Version vs VersionInfo.txt content ============== 
//
function GetNumber(var temp: String): Integer;
var
  part: String;
  pos1: Integer;
begin
  if Length(temp) = 0
	then begin
    Result := -1;
    Exit;
  end;
  pos1 := Pos('.', temp);
  if (pos1 = 0)
	then begin
    Result := StrToInt(temp);
  temp := '';
  end else begin
  part := Copy(temp, 1, pos1 - 1);
    temp := Copy(temp, pos1 + 1, Length(temp));
    Result := StrToInt(part);
  end;
end;
/////////////////////////////////////////////////////////////////////
function CompareInner(var temp1, temp2: String): Integer;
var
  num1, num2: Integer;
begin
  num1 := GetNumber(temp1);
  num2 := GetNumber(temp2);
  if (num1 = -1) or (num2 = -1) 
	then begin
    Result := 0;
    Exit;
  end;
  if (num1 > num2) 
	then begin
    Result := 1;
  end else if (num1 < num2)
	then begin
    Result := -1;
  end else begin
    Result := CompareInner(temp1, temp2);
  end;
end;
/////////////////////////////////////////////////////////////////////
function CompareVersion(str1, str2: String): Integer;
var
  temp1, temp2 : String;
begin
  temp1 := str1;
  temp2 := str2;
  Result := CompareInner(temp1, temp2);
end;
/////////////////////////////////////////////////////////////////////
function BoolToStr(Value : Boolean) : String;
begin
  Result := 'True';
  if not Value then
    Result := 'False';
end;
//
// ============== Extract a RAR Archive ============== 
//
function UnRar(FileNameRar: String) : Boolean; 
var
  fn, args : String;
  ErrorCode: Integer;
begin
  Result := False;
  if VarIsClear(FileNameRar) then
    RaiseException(Format('RAR file "%s" does not exist or cannot be opened', [FileNameRar]));

  ExtractTemporaryFiles('{tmp}\unrar.exe');
  fn := ExpandConstant('{tmp}\unrar.exe');
  if FileExists(fn)
  then begin
    args := 'e ' + FileNameRar;
    // may need to run UnRAR as Admin to avoid some AV issues
    // ShellExec('runas', fn, ExpandConstant(args), '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
    Exec(fn, ExpandConstant(args), '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);		
    if ErrorCode <> 0 
    then begin
      SuppressibleMsgBox('<ERROR> Could not extract ' + ExpandConstant(FileNameRar), mbCriticalError, MB_OK, MB_OK);
	end else begin	
	  DeleteFile(fn);
	  Result := True;
	end;	
  end else begin
    SuppressibleMsgBox('<ERROR> ' + fn + ' does not exist', mbCriticalError, MB_OK, MB_OK);
  end;
end;
//
// ============== Exit Process Smoothly ==============
//
procedure ExitProcess(exitCode:integer);
  external 'ExitProcess@kernel32.dll stdcall';
//
// ============== Is Process Loaded (Reliable on WinXp) ==============
//  
function IsProcessLoaded(FileNameExe: String) : Boolean;
var
  ResultCode : Integer;
  pviewer : String;
begin 
  Result := False;
  ExtractTemporaryFiles('{tmp}\processviewer.exe');
  pviewer := ExpandConstant('{tmp}\processviewer.exe');
  
  if FileExists(pviewer)
  then begin
    if Exec(pviewer, FileNameExe, '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
    then begin
      DebugMessage('ResultCode = ' + IntToStr(ResultCode));
      Result := (ResultCode > 0);  
      if Result	  
      then begin  
        DebugMessage(FileNameExe + ' is running.');
      end else begin
        DebugMessage(FileNameExe + ' is not running.');
      end;
    end;
  end else begin
    SuppressibleMsgBox('<ERROR> Can not find required file ' + pviewer, mbCriticalError, MB_OK, MB_OK);
  end;
end;
//
// ============== Is Mutex Loaded (w/ notification to shut down process) ==============
//  
function IsMutexLoaded(FileNameExe: String) : Boolean;
var
  mutexName : String;
begin
  //remove ".exe" from FileNameExe to get the Mutex name 
  mutexName :=  UpperCase(Copy(FileNameExe, 1, Length(FileNameExe) - 4));
  
  if CheckForMutexes('Global\' + mutexName)
  then begin
    SuppressibleMsgBox(FileNameExe + ' is already running ...' + #13#10 +
                'Please close all application instances before continuing.'
                , mbError, MB_OK, MB_OK);
    
    if CheckForMutexes('Global\' + mutexName)
    then begin
      Result := True;
      // kill the installer if user didn't close all application instances                
      // ExitProcess(0);
    end else begin
      Result := False;
    end;  
  end else begin
    DebugMessage(mutexName + 'Mutex not found ...');
    Result := False;
  end;
end;
//
// ============== Kill Running Process (may not work on WinXp) ==============
//
procedure TaskKill(FileNameExe: String);
var
  ResultCode: Integer;
begin
    Exec(ExpandConstant('taskkill.exe'), '/f /im ' + '"' + FileNameExe + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;  
//
// ============== Do Something when next button is clicked ==============
//
function NextButtonClick(CurPageID: Integer): Boolean;
var
  ErrorCode: Integer;
begin
  DebugMessage('Debugging NextButtonClick PageID:' + IntToStr(CurPageID));
  if IsMutexLoaded(ExpandConstant('{#AppExeName}'))
  then begin
    DebugMessage('WTF! ' + ExpandConstant('{#AppExeName}') + ' is still running ...' + #13#10 + 'Installer will now shut down.');
    Log('-- Custom Run Entry --'); 
    Log('User failed to shut down ' + ExpandConstant('{#AppExeName}'));
    Log('Installation aborted ...'); 
    ExitProcess(0); // Exit;
    Result := False;
  end;
  
  if not hasUpgrade
  then begin
    Result := True;
    exit;
  end else begin
    if (CurPageID =	IDPForm.Page.Id) // IDP Form Page 101
    then begin   
      WizardForm.Visible := False;
      hasUpgrade := UnRar(tmpUpdateRarLocation);
      if hasUpgrade
      then begin
  	    DebugMessage('tmpUpdateLocation: ' + tmpUpdateLocation);	     
        // may need to run webupdate as Admin to avoid some AV issues        
        if Exec(tmpUpdateLocation, '-webupdate', '', SW_SHOW, ewNoWait, ErrorCode)
	    then begin
   	      DebugMessage('-webupdate successful ...');	
          Log('-- Custom Run Entry --'); 
          Log('-webupdate successful ...');
	      ExitProcess(0);
        end else begin
          Log('-- Custom Run Entry --'); 
          Log('-webupdate failed ...');
	      SuppressibleMsgBox('<ERROR> -webupdate failed ...', mbCriticalError, MB_OK, MB_OK);	
        end;
      end;
      WizardForm.Visible := true;
    end;
  end;
  // move through wizard pages smoothly
  Result := True;
end;
//
//  ============== Customize Current Page ==============
//
procedure CurPageChanged(CurPageID: Integer);
begin
  DebugMessage('Debugging CurPageChanged PageID:' + IntToStr(CurPageID));	
  if (CurPageID = 1) // wpWelcome
  then begin
	if hasUpgrade
	then begin
      WizardForm.Caption := ExpandConstant('Version ' + newVersion + ' is now available for download ...');	
	end else begin
	  WizardForm.Caption := ExpandConstant('{#AppTitle}') + ' (v'	+ currentVersion + ')';
	end;	  
    WizardForm.WelcomeLabel1.Caption := 'Welcome to CustomsForge' + #13#10 + 'Song Manager Setup';		
	WizardForm.WelcomeLabel2.Font.Color := clBlue;
	WizardForm.WelcomeLabel2.Font.Size := 10;
	; WizardForm.WelcomeLabel2.Font.Style := [fsBold];
	WizardForm.WelcomeLabel2.Caption := 'This will install ' + ExpandConstant('{#AppTitle}') + ' on your computer.' + #13#10 + #13#10 + 
	                                    'It is recommended that you close all other applications and disable any anti virus before continuing.';
  end;
  
  if (CurPageID = 6) and hasUpgrade
  then begin
    WizardForm.Caption := ExpandConstant('{#AppTitle}') + ' (v'	+ newVersion + ')';
  end;
  
  if (CurPageID = 14) // wpFinished
  then begin
	WizardForm.FinishedHeadingLabel.Font.Size := 10;
	WizardForm.FinishedHeadingLabel.Caption := 'Completed Installing CustomsForge' + #13#10 + 'Song Manager Setup';		
	//WizardForm.FinishedLabel.Caption
  end;
end;
//
// ============== Save Installation Log File ==============
//
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssDone then
    okToCopyLog := True;
end;
/////////////////////////////////////////////////////////////////////
procedure DeinitializeSetup();
begin
  if okToCopyLog then
    FileCopy (ExpandConstant ('{log}'), ExpandConstant ('{app}\Installation.log'), FALSE);
  RestartReplace (ExpandConstant ('{log}'), '');
end;
//
// ============== Uninstall Other Applications (with page dialog) ==============
//
function OldVersionInstalled : Boolean;
begin
  Result := RegKeyExists(HKEY_CURRENT_USER, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\' + ExpandConstant(oldGuid));
end;
/////////////////////////////////////////////////////////////////////
function UninstallNextButtonClick(Page: TWizardPage) : Boolean;
var
  ErrorCode : integer;
  fn : String;
begin
  if OldVersionInstalled and not doneUninstall 
  then begin
    // REVISE As Needed
    ExtractTemporaryFiles('{tmp}\ClickOnceUninstaller.exe');
    fn := ExpandConstant('{tmp}\ClickOnceUninstaller.exe');
    if FileExists(fn) 
    then begin
	  // may need to run as Admin to avoid some AV issues
      Exec(fn, '', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
      DeleteFile(fn);
    end;
  end;
  doneUninstall := true;
  Result := true;
end;
/////////////////////////////////////////////////////////////////////
function UninstallShouldSkipPage(Page: TWizardPage): Boolean;
begin
  result := (not OldVersionInstalled) or (doneUninstall);
end;
/////////////////////////////////////////////////////////////////////
procedure CreateUninstallPage;
begin
  doneUninstall := False;
  UninstallPage := CreateCustomPage(wpWelcome, 'Removing old version of ' + ExpandConstant('{#AppTitle}'),
  'Uninstalling ' + ExpandConstant('{#AppTitle}'));

  with TNewStaticText.Create(UninstallPage) do
  begin
    Parent := UninstallPage.Surface;
    Caption := 'Click the next button to uninstall the old version.';
    Left := ScaleX(0);
    Top := ScaleY(10);
    Width := ScaleX(400);
    Height := ScaleY(14);
    AutoSize := False;
  end;

  with UninstallPage do
  begin
    OnShouldSkipPage    := @UninstallShouldSkipPage;
    OnNextButtonClick   := @UninstallNextButtonClick;
  end;
end;
//
// ============== Inno Wizard Screen Prompts and Pages ============== 
//
procedure InitializeWizard();
var
  updateLoc, updateUrl : String;
  DownloadStep, i : Integer;
  sl:TStringlist;
  DownloadPage: TWizardPage;
  urlLabel : TNewStaticText;	
begin
  DebugMessage('Running InitializeWizard ...');
  // as needed uninstall some old application
  oldGuid := '73e8aceb7ff35be2'
  currentVersion := ExpandConstant('{#AppVersion}');
  DownloadStep := wpWelcome;
  CreateUninstallPage();
  if OldVersionInstalled then
    DownloadStep := UninstallPage.Id;

  hasUpgrade := False;
  runningWebUpdate := pos('-webupdate',GetCmdTail) > 0;
  tmpUpdateLocation := '';
  tmpUpdateRarLocation := '';
  urlLabel := TNewStaticText.Create(WizardForm);
  
  with urlLabel do
  begin
    Caption := ExpandConstant('{#AppWebsite}');
    Cursor := crHand;
    OnClick := @URLLabelOnClick;
    Parent := WizardForm;
    Font.Style := Font.Style + [fsUnderline];
    Font.Color := clBlue;
    Top := WizardForm.CancelButton.Top + WizardForm.CancelButton.Height - Height - 4;
    Left := ScaleX(15);
  end;

#IFDEF DOUPDATE
DebugMessage('Debugging DOUPDATE is defined ...');
DebugMessage('VersionInfoLocation ' + ExpandConstant('{#VersionInfoLocation}'));
DebugMessage('runningWebUpdate ' + BoolToStr(runningWebUpdate));
  
//don't check for updates when -webupdate is located in the command line params.
  if not runningWebUpdate
  then begin
    updateLoc := ExpandConstant('{tmp}\update.txt');
    //check for an update    
    if idpDownloadFile('{#VersionInfoLocation}', updateLoc)
	then begin
      DebugMessage('Successfully Downloaded ' + updateLoc);
      sl := TStringlist.Create;
      sl.LoadFromFile(updateLoc);
      if sl.count > 0
      then begin
        for i := 0 to sl.count -1 do
        begin
          if sl[i] <> '' 
 	      then begin
            newVersion := sl[i];
            break;
          end;
        end;
       
        updateUrl := ExpandConstant('{#LatestVersionDownload}');	
        DebugMessage('hasUpgrade ' + BoolToStr(hasUpgrade));
        
        hasUpgrade := CompareVersion(currentVersion, newVersion) < 0;        
        if hasUpgrade
        then begin
          DebugMessage('hasUpgrade ' + BoolToStr(hasUpgrade));

          tmpUpdateRarLocation := ExpandConstant('{tmp}\' + ExpandConstant('{#InstallerName}') + '_'+ newVersion+ '.rar');
          tmpUpdateLocation := ExpandConstant('{tmp}\' + ExpandConstant('{#InstallerName}') + '.exe');
          // add files to downloader
		  idpAddFile(updateUrl, tmpUpdateRarLocation);				
		  // create a custom (idp) download page							
          DownloadPageId := idpCreateDownloadForm(DownloadStep);
		  DownloadPage := PageFromID(DownloadPageId);
		  DownloadPage.Caption := ExpandConstant('{#InstallerName} Downloader ...');
		  DownloadPage.Description := ExpandConstant('Setup is downloading a new version of {#AppTitle} ...');
		  idpSetOption('DetailsVisible', '1');					
		  idpConnectControls();	
		  idpInitMessages();
		end;
      end;
      sl.Free;
    end else begin
      DebugMessage('<WARNING> Auto updater version info download failed ...' + #13#10 + 
                   ExpandConstant('{#LatestVersionDownload}') + #13#10 + #13#10 +                     
                   'Check your internet connection ...');
    end;
  end;
#ENDIF
  
  if runningWebUpdate then
     PostMessage(WizardForm.NextButton.Handle, CN_COMMAND, 0, 0);
end;
//
// ============== Initialization Steps - Check .NET4, Uninstall old app version, download new version ============== 
//
function InitializeSetup(): Boolean;
var
  WwiseRoot, vCurID, vCurAppName, installedVersion, uninstaller : String;
  ErrorCode, mbRet : Integer;
begin
DebugMessage('Running InitializeSetup ...');
//
// Check .Net4 Installation
//
begin 
  if not IsDotNetDetected('v4\Client', 0)
  and not IsDotNetDetected('v4\Full', 0)
  then begin
    MsgBox('This application requires Microsoft .NET Framework 4.0.30319.'#13#13
           'Please use install it from the Microsoft website,'#13
           'and then re-run the setup program.', mbInformation, MB_OK);
    Log('Failed to detect .NET4 installation ...');
    Result := False;
  end else begin
	Log('Successfully detected .NET4 installation ... ');
    Result := True;
  end;
end;
//
// Check Wwise Installation
//  
// begin
//   WwiseRoot := GetEnv('WWISEROOT');
//   Log('-- Custom Run Entry --'); 
//     if Length(WwiseRoot)= 0 then begin
//       MsgBox('Audiokinect Wwise must be installed before' + #13#10 + 'application can use Wwise CLI.', mbCriticalError, MB_OK);
//       Log('Failed to detect any Wwise installation ...');
//       {* returns True so that setup continues normally *}
//       Result := True; 
//     end else begin 
//       Log('Successfully detected Wwise installation ... ');
//       Log(WwiseRoot);
//       Result := True;
//     end;
// end;  
//
// uninstall the existing application for complete fresh installation 
//  
begin
  vCurID := ExpandConstant('{#AppGUID}');
  vCurAppName := ExpandConstant('{#AppTitle}');
  //remove first "{" of ID
  //vCurID := Copy(vCurID, 2, Length(vCurID) - 1);
  //DebugMessage('vCurID = ' + vCurID);
  Result := True; // in case when no previous version is found

  if IsMutexLoaded(ExpandConstant('{#AppExeName}'))
  then begin
    DebugMessage('WTF! ' + ExpandConstant('{#AppExeName}') + ' is still running ...' + #13#10 + 'Installer will now shut down.');
    Log('-- Custom Run Entry --'); 
    Log('User failed to shut down ' + ExpandConstant('{#AppExeName}'));
    Log('Clean installation aborted ...');
    ExitProcess(0); // Exit;
    Result := False;
  end;

  if RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\' + vCurID + '_is1')
  then begin
    RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\' + vCurID + '_is1', 'DisplayVersion', installedVersion);
	// most AV does not like automated uninstalls so prompt to run as Admin
	mbRet := MsgBox(ExpandConstant('{#AppTitle}') + ' (v'	+ installedVersion + ')' + #13#10 + 
                    'is currently installed.'  + #13#10 +  #13#10 + 
			        'The uninstaller must be run as Administrator   ' + #13#10 + 
                    'to help prevent antivirus false positives.' + #13#10 + #13#10 + 
			        'Do you want to perform a clean installation?', mbConfirmation, MB_YESNO);
	//mbRet := IDNO; // silent uninstall
	if  mbRet = IDNO
	then begin 
      // user decided not to do clean installation
      Log('-- Custom Run Entry --'); 
      Log('User aborted clean installation ...');
 	  // Exit;
	end else begin
      RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\' + vCurID + '_is1',	'UninstallString', uninstaller); 
      // run the uninstaller using current user privileges
      // Exec(uninstaller, '', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode);
      // show no prompts and run as Admin
      // ShellExec('runas', uninstaller, '/VERYSILENT /SUPPRESSMSGBOXES', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
      // show all prompts and run as Admin
      ShellExec('runas', uninstaller, '', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode);
      Result := True; // proceed after uninstall
    end;
    // proceed with the rest of installation
    Result := True; 
  end;
end;
// Really THE END
end;
