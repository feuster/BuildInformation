# BuildInformation
 BuildInformation creates a C# class with auto-increment build, build date and revision control system info.

Use the bootstrap BuildInformation.exe for e.g. as pre-build event

> start /WAIT "BuildInformation" "$(ProjectDir)Bootstrap\BuildInformation.exe" "--file=$(ProjectDir)BuildInformation.cs"
> exit 0
