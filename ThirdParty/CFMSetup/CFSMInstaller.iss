#include "cfmsetup.inc"  
#include "ISPPBuiltins.iss"
#include "idp.iss"   

[Setup]
AppName={#ApplicationName}
AppVersion={#AppVersion}
AppId={{58F35625-541C-493A-A289-4B2D362DAFE0}
RestartIfNeededByRun=False
AppPublisher=CustomsForge.com
AppPublisherURL=AppURL
AppSupportURL=AppURL
AppUpdatesURL=AppURL
VersionInfoCompany=CustomsForge.com
DefaultDirName={pf}\CustomsForgeManager
DefaultGroupName=CustomsForge Song Manager
WizardImageFile=CFSMInstallWiz.bmp
WizardSmallImageFile=cfmWizardSmall.bmp
OutputBaseFilename={#InstallerName}
VersionInfoVersion={#AppVersion}
AppCopyright=CustomsForge.com
SetupIconFile= "..\..\CustomsForgeManager\Resources\cfsm_48x48.ico"

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

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: checkedonce
Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Icons]
Name: "{group}\{#ApplicationName}"; Filename: "{app}\{#buildpath}CustomsForgeSongManager.exe"; WorkingDir: "{app}";
Name: {group}\{cm:UninstallProgram,{#ApplicationName}}; Filename: {uninstallexe}
Name: {commondesktop}\{#ApplicationName}; Filename: {app}\{#ApplicationName}; Tasks: desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ApplicationName}; Filename: {app}\{#ApplicationName}; Tasks: quicklaunchicon


[Code]   
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

const
  WM_COMMAND = $0111;
  CN_BASE = $BC00;
  CN_COMMAND = CN_BASE + WM_COMMAND;


var runningWebUpdate: boolean;
var hasUpgrade : boolean;
var tmpUpdateLocation : string;

procedure InitializeWizard();
var
 currentVersion,newVersion,updateLoc : string;
 sl:TStringlist;
 updateUrl:string;
 urlLabel : TNewStaticText; 
begin
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
        newVersion := sl[0];
        if sl.Count > 1 then
            updateUrl := sl[1]
        else
            updateUrl := ExpandConstant('{#LatestVersionDowload}');
      
        currentVersion := ExpandConstant('{#AppVersion}');
        hasUpgrade := CompareVersion(currentVersion, newVersion) < 0;
        if hasUpgrade then
        begin
            tmpUpdateLocation := ExpandConstant('{tmp}\cfm_'+newVersion+'_setup.exe');
            WizardForm.Caption := WizardForm.Caption + ' '+ '(V'+newVersion+' available.)';
            idpAddFile(updateUrl, tmpUpdateLocation);
            idpDownloadAfter(wpWelcome);
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
   if not hasUpgrade then
   begin
      result := true;
      exit;
   end else
   begin
     if (curpageID = 1) then
        result := true 
      else
         //done downloading the update, run the new installer with -webupdate paramerter
        if (curpageID = 100) then
        begin
           WizardForm.Visible := false;
           //run the new setup
           if Exec(tmpUpdateLocation, '-webupdate', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
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
