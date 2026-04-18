!macro CustomCodePostInstall
CopyFiles /SILENT "$INSTDIR\App\blueshot\blueshot.exe.config" "$INSTDIR\"
ReadINIStr $0 "$INSTDIR\App\AppInfo\appinfo.ini" "Version" "PackageVersion"
ExecShell "open" "http://getblueshot.org/thank-you/?language=en-US&version=$0"
!macroend