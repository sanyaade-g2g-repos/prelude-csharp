; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=Prelude@#
AppVerName=Prelude@# 0.8.0
DefaultDirName={pf}\PreludeSharp
DefaultGroupName=Prelude
UninstallDisplayIcon={app}\preludeSHARP.exe
OutputBaseFilename=preludeSharp8
Compression=lzma
SolidCompression=yes
OutputDir=C:\Documents and Settings\novalis78\My Documents\SharpDevelop Projects\preLudeStandard\install
AppPublisherURL=http://novalis78.topcities.com
AppSupportURL=http://novalis78.topcities.com
AppUpdatesURL=http://novalis78.topcities.com
DisableProgramGroupPage=yes
DisableDirPage=no
WizardImageFile=laude1.bmp

[Files]
Source: "preLude.exe"; DestDir: "{app}"
Source: "mind.mdu"; DestDir: "{app}"
Source: "Interop.SpeechLib.dll"; DestDir: "{app}"
Source: "AxInterop.AgentObjects.dll"; DestDir: "{app}"
Source: "Interop.AgentObjects.dll"; DestDir: "{app}"
Source: "StringTokenizer.dll"; DestDir: "{app}"
Source: "PreLudeEngine.dll"; DestDir: "{app}"
Source: "mdu Converter Utility.zip"; DestDir: "{app}"
Source: "Readme First!.txt"; DestDir: "{app}";
Source: "Prelude Online.url"; DestDir: "{app}";
Source: "Prelude.exe.manifest"; DestDir: "{app}";

[Icons]
;Name: "{group}\Prelude's Chat"; Filename: "{app}\prelude.exe"
Name: "{group}\Prelude's Home"; Filename: "{app}\Prelude Online.url"


[Run]
Filename: "{app}\Readme First!.txt"; Flags: postinstall shellexec skipifsilent unchecked
Filename: "{app}\preLude.EXE"; Description: "Launch application"; Flags: postinstall nowait skipifsilent

[Code]

function InitializeSetup(): Boolean;

var
    ErrorCode: Integer;
    NetFrameWorkInstalled : Boolean;
    Result1 : Boolean;
begin
    NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v1.1');
    if NetFrameWorkInstalled =true then
    begin
      Result := true;
    end;
    if NetFrameWorkInstalled =false then
    begin
      Result1 := MsgBox('This setup requires the .NET Framework. Please download and install the .NET Framework and run this setup again. Do you want to download the framwork now?',

      mbConfirmation, MB_YESNO) = idYes;
      if Result1 =false then
      begin
        Result:=false;
      end
      else
      begin
        Result:=false;
        InstShellExec('http://download.microsoft.com/download/4/f/3/4f3ac857-e063-45d0-9835-83894f20e808/dotnetfx.exe','','',SW_SHOWNORMAL,ErrorCode);

      end;
    end;
end;


