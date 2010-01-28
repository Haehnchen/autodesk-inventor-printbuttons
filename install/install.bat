@echo off

IF %PROCESSOR_ARCHITECTURE% == AMD64 GOTO x64
IF %PROCESSOR_ARCHITECTURE% == x86 GOTO x86

echo unknown CPU
goto end

:x86
echo found x86 system  
%windir%\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe /codebase ../PrintButtons.dll /nologo
goto end

:x64
echo found x64 system  
%windir%\Microsoft.NET\Framework64\v2.0.50727\RegAsm.exe /codebase ../PrintButtons.dll /nologo
goto end


:end
pause

