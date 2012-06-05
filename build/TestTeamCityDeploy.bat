@echo off
cd ..
%SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\Msbuild.exe build\Ohb.proj /t:TeamCityDeploy
pause