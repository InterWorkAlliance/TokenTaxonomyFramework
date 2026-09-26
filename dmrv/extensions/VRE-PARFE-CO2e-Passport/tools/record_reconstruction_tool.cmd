@echo off
setlocal

set "SCRIPT_DIR=%~dp0"
set "NODE_EXE=node"
set "HAS_PATH_NODE=0"

where node >nul 2>nul
if errorlevel 1 (
  set "NODE_EXE=%USERPROFILE%\.cache\codex-runtimes\codex-primary-runtime\dependencies\node\bin\node.exe"
) else (
  set "HAS_PATH_NODE=1"
)

if "%HAS_PATH_NODE%"=="0" if not exist "%NODE_EXE%" (
  echo Node.js was not found on PATH or in the bundled Codex runtime. 1>&2
  exit /b 1
)

"%NODE_EXE%" "%SCRIPT_DIR%record_reconstruction_tool.js" %*
exit /b %ERRORLEVEL%
