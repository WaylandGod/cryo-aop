@echo off
set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_2_0_Debug;NoWarn="0649;1685" /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_2_0_Debug.nunit
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_2_0_Release;NoWarn="0649;1685" /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_2_0_Release.nunit
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_3_5_Debug;NoWarn=0649 /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_3_5_Debug.nunit
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_3_5_Release;NoWarn=0649 /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_3_5_Release.nunit
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_4_0_Debug;NoWarn=0649 /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_4_0_Debug.nunit
msbuild .\CryoAOP.sln /t:Rebuild /p:Configuration=net_4_0_Release;NoWarn=0649 /verbosity:minimal
.\References\NUnit\2.5.9.10348\nunit-console.exe .\bin\net_4_0_Release.nunit
del .\bin\*.xml
pause