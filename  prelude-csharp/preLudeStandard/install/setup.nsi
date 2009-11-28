; Script generated by the ISS2NSI program.
; Generated from: setup.iss

; MUI 1.66 compatible
!include "MUI.nsh"

SetCompressor lzma

;----------Custom Function for GUI (needed for splash screen)
!define IMG_SPLASH "splash.bmp"
!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_CUSTOMFUNCTION_GUIINIT myGUIInit
!define INSTALLER_WELCOME_BITMAP "laude1.bmp"
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\nsis.bmp" ; optional
!define MUI_WELCOMEFINISHPAGE_BITMAP "${INSTALLER_WELCOME_BITMAP}"
; MUI Settings
!define MUI_ABORTWARNING
!define MUI_SPECIALBITMAP "laude1.bmp"
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
    !define MUI_FINISHPAGE_RUN_CHECKED
    !define MUI_FINISHPAGE_SHOWREADME $INSTDIR\readme.txt
    !define MUI_FINISHPAGE_RUN "$INSTDIR\preLude.exe"
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"



Name "Prelude@# v. 0.9.6"
BrandingText ""
XPStyle on
OutFile "preludeSharp9.6.exe"
InstallDir "$PROGRAMFILES\PreludeSharp"
ShowInstDetails show
ShowUnInstDetails show

Section -Files
  SetOutPath "$INSTDIR"
  File "preLude.exe"
  File "preLude.exe.xml"
  File "Prelude.exe.manifest"
  File "mind.mdu"
  File "Interop.SpeechLib.dll"
  File "AxInterop.AgentObjects.dll"
  File "Interop.AgentObjects.dll"
  File "AgentObjects.dll"
  File "stem.dll"
  File "Melissa.acs"
  File "AMS.Profile.dll"
  File "PreludeEngine.dll"
  File "readme.txt"
  File "Prelude Online.url"
  File "Prelude UPDATES.url"
  File "Prelude FAN SITE.url"
SectionEnd

Section -Icons
  CreateDirectory "$SMPROGRAMS\Pali Software Tools"
  CreateShortCut "$SMPROGRAMS\Pali Text Reader.lnk" "$INSTDIR\PaliReader.exe"
  CreateShortCut "$SMPROGRAMS\Pali Software Tools\Uninstall Pali Text Reader.lnk" "$INSTDIR\uninstall.exe"
SectionEnd

Section -PostInstall
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "DisplayName" "Prelude@# v.0.9.6"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "UninstallString" "$INSTDIR\uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "DisplayIcon" "$INSTDIR\uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "URLInfoAbout" "http://prelude.lennart-lopin.de"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "HelpLink" "http://prelude.lennart-lopin.de"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#" "URLUpdateInfo" "http://prelude.lennart-lopin.de"
  WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd


#### Uninstaller code ####

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Section Uninstall
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Prelude@#"

  Delete "$INSTDIR\uninstall.exe"
  Delete "$SMPROGRAMS\Prelude\Prelude's Home.lnk"
  Delete "$INSTDIR\Prelude.exe.manifest"
  Delete "$INSTDIR\Prelude Online.url"
  Delete "$INSTDIR\Readme First!.txt"
  Delete "$INSTDIR\mdu Converter Utility.zip"
  Delete "$INSTDIR\PreLudeEngine.dll"
  Delete "$INSTDIR\StringTokenizer.dll"
  Delete "$INSTDIR\Interop.AgentObjects.dll"
  Delete "$INSTDIR\AxInterop.AgentObjects.dll"
  Delete "$INSTDIR\Interop.SpeechLib.dll"
  Delete "$INSTDIR\mind.mdu"
  Delete "$INSTDIR\preLude.exe"

  Delete "$SMPROGRAMS\Prelude\Prelude's Home.lnk" 
  Delete "$SMPROGRAMS\Prelude\start Prelude@#.lnk"
  Delete "$SMPROGRAMS\Prelude\uninstall Prelude@#.lnk"

  RMDir "$SMPROGRAMS\Prelude"
  RMDir /r $INSTDIR ;den Rest kicken wir so raus (Beispiel E-Mails etc.)
  SetAutoClose true
SectionEnd

; Custom Functions
;===========================================================================================================================
Function .onInit
    ;splash screen stuff++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    InitPluginsDir
    SetOutPath "$PLUGINSDIR"

    ;Modeless banner sample: show + wait
    File ${IMG_SPLASH}
    newadvsplash::show /NOUNLOAD 2000 1000 500 -2 /BANNER "$PLUGINSDIR\${IMG_SPLASH}"
    Sleep 2000 ; not changes 3.5 sec of 'show time'. add your code instead of sleep
    ;splash screen stuff ends here  ++++++++++++++++++++++++++++++++++++++++++++++++++++++
   ;LogSet  on
FunctionEnd


Function myGUIInit
    ;newadvsplash::wait ; waits or exits immediately if finished, use 'stop' to terminate
    Delete "$PLUGINSDIR\${IMG_SPLASH}"
    SetOutPath "$EXEDIR"
    ;  plug-in requires this to be called in .onGUIInit
    ;  if you use 'show' in the .onInit function with /BANNER key.
    ShowWindow $HWNDPARENT ${SW_RESTORE}
FunctionEnd
