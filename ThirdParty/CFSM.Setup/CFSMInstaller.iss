#include "cfsmInclude.iss"
#include "ISPPBuiltins.iss"
#include "idp.iss"

[Setup]
; TODO: SignTool=signtool
AppName={#AppName}
AppVersion={#AppVersion}
AppId={{58F35625-541C-493A-A289-4B2D362DAFE0}
RestartIfNeededByRun=False
AppPublisher=CustomsForge.com
AppPublisherURL=AppURL
AppSupportURL=AppURL
AppUpdatesURL=AppURL
VersionInfoCompany=CustomsForge.com
; TODO: install CFMS in current user's Common Files dirctory, may have less OS permission issues
; DefaultDirName={usercf}\CustomsForgeSongManager
; TODO: install CFMS in current user's Program Files dirctory, may have less OS permission issues
; DefaultDirName={userpf}\CustomsForgeSongManager
DefaultDirName={pf}\CustomsForgeSongManager
DefaultGroupName=CustomsForge Song Manager
WizardImageFile=cfsmInstallWiz.bmp
WizardSmallImageFile=cfsmWizardSmall.bmp
OutputBaseFilename={#InstallerName}
VersionInfoVersion={#AppVersion}
AppCopyright=CustomsForge.com
SetupIconFile=..\..\CustomsForgeSongManager\Resources\cfsm_48x48.ico
DirExistsWarning=no

; Give OS write permissions to CFSM over these folders
[Dirs]
Name: "{app}"
Name: "{app}\ddc"; Permissions: everyone-full
Name: "{app}\tmp"; Permissions: everyone-full

; Give OS write permisions to all app exe and library exe files
[Files]
Source: {#buildpath}{#AppExeName}; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full 
Source: {#buildpath}CFSM.AudioTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}CFSM.ImageTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}DF.WinForms.ThemeLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}GenTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}CustomControls.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}CFSM.RSTKLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}bass.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}DataGridViewTools.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}CFSM.NCalc.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}DLogNet.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}HtmlRenderer.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}HtmlRenderer.WinForms.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}ICSharpCode.SharpZipLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}RocksmithToolkitLib.dll; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}X360.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Antlr3.Runtime.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Antlr4.StringTemplate.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}DF_DDSImage.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}MiscUtil.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}7z.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}oggCut.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}oggdec.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}oggenc.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}packed_codebooks.bin; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}packed_codebooks_aoTuV_603.bin; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}revorb.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}ww2ogg.exe; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}SevenZipSharp.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Newtonsoft.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}ReleaseNotes.txt; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}RocksmithToolkitLib.Config.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}RocksmithToolkitLib.SongAppId.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}RocksmithToolkitLib.TuningDefinition.xml; DestDir: {app}; Flags: ignoreversion; Permissions: everyone-full
Source: {#buildpath}zlib.net.dll; DestDir: {app}; Flags: ignoreversion
Source: "{#buildpath}ddc\*"; DestDir: "{app}\ddc"; Flags: ignoreversion recursesubdirs createallsubdirs; Permissions: everyone-full
Source: {srcexe}; DestDir: {app}; DestName: {#InstallerName}.exe; Flags: ignoreversion external; Permissions: everyone-full

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: checkedonce
; TODO: quick launch win 7 and up
Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Run]
Filename: {app}\{#AppExeName}; Description: "{cm:LaunchProgram, {%AppName}}"; Flags: nowait postinstall skipifsilent

[Icons]
Name: {group}\{#AppName}; Filename: {app}\{#AppExeName}; WorkingDir: {app}; IconFilename: {app}\{#AppExeName}
Name: {group}\{cm:UninstallProgram,{#AppName}}; Filename: {uninstallexe}
Name: {commondesktop}\{#AppName}; Filename: {app}\{#AppExeName}; WorkingDir: {app}; IconFilename: {app}\{#AppExeName}; Tasks: desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#AppName}; Filename: {app}\{#AppExeName}; Tasks: quicklaunchicon


[Code]
const
  WM_COMMAND = $0111;
  CN_BASE = $BC00;
  CN_COMMAND = CN_BASE + WM_COMMAND;

var runningWebUpdate: boolean;
var hasUpgrade : boolean;
var tmpUpdateLocation : string;
var UninstallPage: TWizardPage;
var doneUninstall : boolean;

function GetNumber(var temp: String): Integer;
var
  part: String;
  pos1: Integer;
begin
  if Length(temp) = 0 then
  begin
    Result := -1;
    Exit;
  end;
  pos1 := Pos('.', temp);
  if (pos1 = 0) then
  begin
    Result := StrToInt(temp);
    temp := '';
  end
  else
  begin
    part := Copy(temp, 1, pos1 - 1);
    temp := Copy(temp, pos1 + 1, Length(temp));
    Result := StrToInt(part);
  end;
end;

function CompareInner(var temp1, temp2: String): Integer;
var
  num1, num2: Integer;
begin
  num1 := GetNumber(temp1);
  num2 := GetNumber(temp2);
  if (num1 = -1) or (num2 = -1) then
  begin
    Result := 0;
    Exit;
  end;
  if (num1 > num2) then
    Result := 1
  else
  if (num1 < num2) then
    Result := -1
  else
    Result := CompareInner(temp1, temp2);
end;

function CompareVersion(str1, str2: String): Integer;
var
  temp1, temp2: String;
begin
    temp1 := str1;
    temp2 := str2;
    Result := CompareInner(temp1, temp2);
end;

procedure URLLabelOnClick(Sender: TObject);
var
  ErrorCode: Integer;
begin
  ShellExec('open', ExpandConstant('{#AppURL}'), '', '', SW_SHOWNORMAL,
    ewNoWait, ErrorCode);
end;

procedure ShowMessage(const s:String);
begin
   MsgBox(s, mbInformation, MB_OK);
end;

function OldVersionInstalled:Boolean;
begin
   result := RegKeyExists(HKEY_CURRENT_USER, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\73e8aceb7ff35be2');
   //ignore this check
   result := false;
end;


function UninstallNextButtonClick(Page: TWizardPage): Boolean;
var
 ResultCode : integer;
 fn : string;
begin
  if OldVersionInstalled and not doneUninstall then
  begin
    ExtractTemporaryFiles('{tmp}\ClickOnceUninstaller.exe');
    fn := ExpandConstant('{tmp}\ClickOnceUninstaller.exe');
    if FileExists(fn) then
    begin
      Exec(fn, '', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
      DeleteFile(fn);
    end;
  end;
  doneUninstall := true;
  Result := true;
end;

function UninstallShouldSkipPage(Page: TWizardPage): Boolean;
begin
  result := (not OldVersionInstalled) or (doneUninstall);
end;

procedure CreateUninstallPage;
begin
  doneUninstall := false;
  UninstallPage := CreateCustomPage(wpWelcome, 'Removing old version of ' + ExpandConstant('{#AppName}'),
  'Uninstalling ' + ExpandConstant('{#AppName}'));

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

procedure InitializeWizard();
var
 customCaption,currentVersion,newVersion,updateLoc : string;
 sl:TStringlist;
 i:integer;
 updateUrl:string;
 urlLabel : TNewStaticText;
 DownloadStep : integer;
begin
  customCaption := ExpandConstant('{#InstallerCaption}');
  WizardForm.Caption := customCaption;

  DownloadStep := wpWelcome;
  CreateUninstallPage();
  if OldVersionInstalled then
    DownloadStep := UninstallPage.Id;


  hasUpgrade := false;
  runningWebUpdate := pos('-webupdate',GetCmdTail) > 0;
  tmpUpdateLocation := '';
  urlLabel := TNewStaticText.Create(WizardForm);
  with urlLabel do
  begin
    Caption := ExpandConstant('{#AppURL}');
    Cursor := crHand;
    OnClick := @URLLabelOnClick;
    Parent := WizardForm;
    Font.Style := Font.Style + [fsUnderline];
    Font.Color := clBlue;
    Top := WizardForm.CancelButton.Top + WizardForm.CancelButton.Height - Height - 4;
    Left := ScaleX(15);
  end;

  #IFDEF DOUPDATE
 //don't check for updates when -webupdate is located in the command line params.
  if not runningWebUpdate then
  begin
    updateLoc :=  ExpandConstant('{tmp}\update.txt');
    //check for an update
    if idpDownloadFile('{#UpdateInfoLocation}', updateLoc) then
    begin
       sl := TStringlist.Create;
       sl.LoadFromFile(updateLoc);
       if sl.count > 0 then
       begin
        for i := 0 to sl.count -1 do
        begin
           if sl[i] <> '' then
           begin
              newVersion := sl[i];
              break;
           end;
        end;
        updateUrl := ExpandConstant('{#LatestVersionDowload}');

        hasUpgrade := CompareVersion(currentVersion, newVersion) < 0;
        if hasUpgrade then
        begin
            tmpUpdateLocation := ExpandConstant('{tmp}\cfsm_'+newVersion+'_setup.exe');
            WizardForm.Caption := WizardForm.Caption + ' - '+ '(V'+newVersion+' available.)';
            idpAddFile(updateUrl, tmpUpdateLocation);
            idpDownloadAfter(DownloadStep);
        end
      end;
      sl.Free;
    end;
  end;
#ENDIF
 if runningWebUpdate then
     PostMessage(WizardForm.NextButton.Handle, CN_COMMAND, 0, 0);
end;

//runningWebUpdate

procedure ExitProcess(exitCode:integer);
  external 'ExitProcess@kernel32.dll stdcall';

function NextButtonClick(CurPageID: Integer): Boolean;
var
  ResultCode: Integer;
begin
  if CheckForMutexes('Global\CUSTOMSFORGESONGMANAGER') then
  begin
    ShowMessage('CustomsForge Song Manager is running.'+#13#10+'it will need to be closed before installation continues.');
    result := false;
    exit;
  end;
  if not hasUpgrade then
  begin
    result := true;
    exit;
  end else
  begin
    case curpageID of
    1 :
    begin
      result := true;
    end;
    IDPForm.Page.Id:
      begin
        WizardForm.Visible := false;
        //run the new setup
        if Exec(tmpUpdateLocation, '-webupdate', '', SW_SHOW, ewNoWait, ResultCode) then
        begin
          ExitProcess(0);
        end else
        begin
          WizardForm.Visible := true;
          result := true;
        end;
      end;
    end;
  end;
end;
// ========= App Uninstaller Code ===========

function GetUninstallString: string;
var
  sUnInstPath: string;
  sUnInstallString: String;
begin
  Result := '';
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{{58F35625-541C-493A-A289-4B2D362DAFE0}_is1'); //Your App GUID/ID
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;

function IsUpgrade: Boolean;
begin
  Result := (GetUninstallString() <> '');
end;

//http://www.kynosarges.de/DotNetVersion.html
function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1.4322'     .NET Framework 1.1
//    'v2.0.50727'    .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key: string;
    install, serviceCount: cardinal;
    success: boolean;
begin
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + version;
    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;
    // .NET 4.0 uses value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;
    result := success and (install = 1) and (serviceCount >= service);
end;

function InitializeSetup: Boolean;
var
  V: Integer;
  iResultCode: Integer;
  sUnInstallString: string;
begin
  begin
    if not IsDotNetDetected('v4\Client', 0)
    and not IsDotNetDetected('v4\Full', 0)
    then begin
        MsgBox('This application requires Microsoft .NET Framework 4.0.30319.'#13#13
            'Please use install it from the Microsoft website,'#13
            'and then re-run the setup program.', mbInformation, MB_OK);
        result := false;
    end else
        result := true;
  end;

  begin
    Result := True; // in case when no previous version is found
    if RegValueExists(HKEY_LOCAL_MACHINE,'Software\Microsoft\Windows\CurrentVersion\Uninstall\{58F35625-541C-493A-A289-4B2D362DAFE0}_is1', 'UninstallString') then  //Your App GUID/ID
    begin
      //V := MsgBox(ExpandConstant('An old version of CFSM was detected. Do you want to uninstall it?'), mbInformation, MB_YESNO); //Custom Message if App installed
      V := IDYES;
      if V = IDYES then
      begin
        sUnInstallString := GetUninstallString();
        sUnInstallString :=  RemoveQuotes(sUnInstallString);
        Exec(ExpandConstant(sUnInstallString), '/VERYSILENT /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode);
        //commented out showing all user prompts
        //Exec(ExpandConstant(sUnInstallString), '', '', SW_SHOW, ewWaitUntilTerminated, iResultCode);
        Result := True; //if you want to proceed after uninstall
        //Exit; //if you want to quit after uninstall
      end
      else
        Result := False; //when older version present and not uninstalled
    end;
  end;
end;
