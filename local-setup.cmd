@echo off

SET NEWLINE=^& echo.

FIND /C /I "local.estimatorx.com" %WINDIR%\system32\drivers\etc\hosts
IF %ERRORLEVEL% NEQ 0 ECHO %NEWLINE%^127.0.0.1 local.estimatorx.com>>%WINDIR%\System32\drivers\etc\hosts
