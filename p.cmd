@echo off
IF NOT EXIST %~dp0.paket/paket.exe %~dp0.paket/paket.bootstrapper.exe
%~dp0.paket/paket.exe %*
