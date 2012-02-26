@echo off
net stop RavenDB
rd /S /Q C:\RavenDB\Server\Data
rd /S /Q C:\RavenDB\Server\Tenants
net start RavenDB
pause