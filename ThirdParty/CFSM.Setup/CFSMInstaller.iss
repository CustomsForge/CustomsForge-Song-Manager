#include "cfsmInclude.iss"
#include "ISPPBuiltins.iss"
#include "idp.iss"

[Setup]
AppName={#AppName}
AppVersion={#AppVersion}
AppId={{58F35625-541C-493A-A289-4B2D362DAFE0}
RestartIfNeededByRun=False
AppPublisher=CustomsForge.com
AppPublisherURL=AppURL
AppSupportURL=AppURL
AppUpdatesURL=AppURL
VersionInfoCompany=CustomsForge.com
DefaultDirName={pf}\CustomsForgeSongManager
DefaultGroupName=CustomsForge Song Manager
WizardImageFile=cfsmInstallWiz.bmp
WizardSmallImageFile=cfsmWizardSmall.bmp
OutputBaseFilename={#InstallerName}
VersionInfoVersion={#AppVersion}
AppCopyright=CustomsForge.com
SetupIconFile=..\..\CustomsForgeSongManager\Resources\cfsm_48x48.ico

[Files]
Source: {#buildpath}{#AppExeName}; DestDir: {app}; Flags: ignoreversion
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
Source: {#buildpath}RocksmithToolkitLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}RocksmithToTabLib.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}X360.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Antlr3.Runtime.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Antlr4.StringTemplate.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}DF_DDSImage.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}MiscUtil.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}7z.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}oggCut.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}oggdec.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}oggenc.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}packed_codebooks.bin; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}packed_codebooks_aoTuV_603.bin; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}revorb.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}ww2ogg.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}remastered.exe; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}SevenZipSharp.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}Newtonsoft.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}ReleaseNotes.txt; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}RocksmithToolkitLib.Config.xml; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}RocksmithToolkitLib.SongAppId.xml; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}RocksmithToolkitLib.TuningDefinition.xml; DestDir: {app}; Flags: ignoreversion
Source: {#buildpath}zlib.net.dll; DestDir: {app}; Flags: ignoreversion
Source: "{#buildpath}ddc\*"; DestDir: "{app}\ddc"; Flags: replacesameversion recursesubdirs
Source: {srcexe}; DestDir: {app}; DestName: {#InstallerName}.exe; Flags: ignoreversion external

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: checkedonce
;todo: quick launch win 7 and up
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
            tmpUpdateLocation := ExpandConstant('{tmp}\cfm_'+newVersion+'_setup.exe');
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
