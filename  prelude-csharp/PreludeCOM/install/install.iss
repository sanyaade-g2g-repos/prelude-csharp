; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=PreludeIM
AppVerName=PreludeIM 0.0.0.1
DefaultDirName={pf}\Miranda IM\plugins
UninstallDisplayIcon={app}\PreludeIM.exe
Compression=lzma
SolidCompression=yes
AppPublisherURL=http://novalis78.topcities.com
AppSupportURL=http://novalis78.topcities.com
AppUpdatesURL=http://novalis78.topcities.com
DisableProgramGroupPage=yes
DisableDirPage=yes
WizardImageFile=laude1.bmp

[Files]
Source: "PreludeIM.dll"; DestDir: "{app}"
Source: "st.dll"; DestDir: "{app}"
Source: "PreludeCOM.dll"; DestDir: "{app}"
Source: "RegAsm.exe"; DestDir: "{app}"
Source: "gacutil.exe"; DestDir: "{app}"
Source: "rprogram.bat"; DestDir: "{app}"
Source: "mind.mdu"; DestDir: "{pf}\Miranda IM"
Source: "Readme.txt"; DestDir: "{app}"; Flags: isreadme

[RUN]
Filename: "{app}\rprogram.bat"
Filename: "{app}\Readme.txt"; Description: "View the Readme file"; Flags: postinstall shellexec skipifsilent
Filename: "{app}\miranda.exe"; Description: "Launch Miranda"; Flags: postinstall

[UNINSTALLRUN]
Filename: "{app}\unrprogram.bat"
