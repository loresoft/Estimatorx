@echo off
Nuget.exe restore "Code\Source\Estimatorx.sln"

NuGet.exe install MSBuildTasks -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
