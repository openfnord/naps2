; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define AppVersion "5.5.0"

[Setup]
AppName=NAPS2 (Not Another PDF Scanner 2)
AppVerName=NAPS2 {#AppVersion}
AppPublisher=Ben Olden-Cooligan
AppPublisherURL=http://www.sourceforge.net/projects/naps2
AppSupportURL=http://www.sourceforge.net/projects/naps2
AppUpdatesURL=http://www.sourceforge.net/projects/naps2
DefaultDirName={pf}\NAPS2
DefaultGroupName=NAPS2
OutputDir=../publish/{#AppVersion}
OutputBaseFilename=naps2-{#AppVersion}-setup       
Compression=lzma
SolidCompression=yes

LicenseFile=..\..\LICENSE
UninstallDisplayIcon={app}\scanner-app.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]                              
#include "setup.languagefiles.iss"                                                          
Source: "..\bin\InstallerEXE\NAPS2.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\NAPS2.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\NAPS2.Core.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\NAPS2.DI.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\PdfSharp.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\Interop.WIA.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\Ninject.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\NLog.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\NTwain.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\zxing.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\Ghostscript.NET.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\bin\InstallerEXE\System.ValueTuple.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "..\appsettings.xml"; DestDir: "{app}"; Flags: ignoreversion           
Source: "..\..\LICENSE"; DestDir: "{app}"; DestName: "license.txt"; Flags: ignoreversion
Source: "..\..\CONTRIBUTORS"; DestDir: "{app}"; DestName: "contributors.txt"; Flags: ignoreversion
Source: "..\..\NAPS2.Console\bin\InstallerEXE\NAPS2.Console.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\NAPS2.Console\bin\InstallerEXE\NAPS2.Console.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\NAPS2.Console\bin\InstallerEXE\CommandLine.dll"; DestDir: "{app}\lib"; Flags: ignoreversion
Source: "lib\twaindsm.dll"; DestDir: "{sys}"; Flags: sharedfile
Source: "lib\wiaaut.dll"; DestDir: "{sys}"; Flags: regserver

; Delete files from old locations in case of upgrade
[InstallDelete]
Type: files; Name: "{app}\*.dll"
Type: filesandordirs; Name: "{app}\??"
Type: filesandordirs; Name: "{app}\??-??"

[Icons]
Name: "{group}\NAPS2"; Filename: "{app}\NAPS2.exe"
Name: "{commondesktop}\NAPS2"; Filename: "{app}\NAPS2.exe"; Tasks: desktopicon

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Action"; ValueData: "Scan with NAPS2"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "CLSID"; ValueData: "WIACLSID"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "DefaultIcon"; ValueData: "sti.dll,0"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "InitCmdLine"; ValueData: "/WiaCmd;{pf}\NAPS2\NAPS2.exe /StiDevice:%1 /StiEvent:%2;"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\Handlers\WIA_{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Provider"; ValueData: "NAPS2"

Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\StillImage\Registered Applications"; Flags:uninsdeletevalue; ValueType: string; ValueName: "NAPS2"; ValueData: "{pf}\NAPS2\NAPS2.exe"

Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\StillImage\Events\STIProxyEvent\{{1c3a7177-f3a7-439e-be47-e304a185f932}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\StillImage\Events\STIProxyEvent\{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Cmdline"; ValueData: "{pf}\NAPS2\NAPS2.exe /StiDevice:%1 /StiEvent:%2"
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\StillImage\Events\STIProxyEvent\{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Desc"; ValueData: "Scan with NAPS2"
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\StillImage\Events\STIProxyEvent\{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Icon"; ValueData: "{pf}\NAPS2\NAPS2.exe,0"
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\StillImage\Events\STIProxyEvent\{{1c3a7177-f3a7-439e-be47-e304a185f932}"; ValueType: string; ValueName: "Name"; ValueData: "NAPS2"
