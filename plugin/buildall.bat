
set PLUGIN_DIR=%cd%
set BUILD_TYPE="Debug"
set OUTPUT_DIR="%PLUGIN_DIR%\..\unity_project\Assets\Plugins\unity_plugin"

:loop
IF NOT "%1"=="" (
    IF "%1"=="-r" (
        SET BUILD_TYPE="Release"
    )
    SHIFT
    GOTO :loop
)


call :build_windows x86 "" || goto :error
call :build_windows x86_64 " Win64" || goto :error

call :build_windows_uwp x86 "" || goto :error
call :build_windows_uwp x86_64 " Win64" || goto :error
call :build_windows_uwp arm " ARM" || goto :error

call :build_windows_editor x86 "" || goto :error
call :build_windows_editor x86_64 " Win64" || goto :error


echo "Done."

EXIT /B 0

:build_windows
    set DIR=%~1
    set ARCH=%~2
    mkdir "build\windows_%DIR%"
    pushd "build\windows_%DIR%"
    cmake -G "Visual Studio 15 2017%ARCH%" -DCMAKE_BUILD_TYPE=%BUILD_TYPE% -DTARGET_DIR="%OUTPUT_DIR%\%DIR%" %PLUGIN_DIR% || (popd && exit /b 1)
    cmake --build . --config %BUILD_TYPE% || (popd && exit /b 1)
    popd
EXIT /B 0

:build_windows_uwp
    set DIR=%~1
    set ARCH=%~2
    mkdir "build\windows_uwp_%DIR%"
    pushd "build\windows_uwp_%DIR%"
    cmake -G "Visual Studio 15 2017%ARCH%" -DCMAKE_BUILD_TYPE=%BUILD_TYPE% -DCMAKE_SYSTEM_NAME=WindowsStore -DCMAKE_SYSTEM_VERSION:STRING="10.0" -DTARGET_DIR="%OUTPUT_DIR%\uwp\%DIR%" %PLUGIN_DIR% || (popd && exit /b 1)
    cmake --build . --config %BUILD_TYPE% || (popd && exit /b 1)
    popd
EXIT /B 0

:build_windows_editor
    set DIR=%~1
    set ARCH=%~2
    mkdir "build\windows_editor_%DIR%"
    pushd "build\windows_editor_%DIR%"
    cmake -G "Visual Studio 15 2017%ARCH%" -DCMAKE_BUILD_TYPE=%BUILD_TYPE% -DEDITOR=TRUE -DTARGET_DIR="%OUTPUT_DIR%\%DIR%" %PLUGIN_DIR%  || (popd && exit /b 1)
    cmake --build . --config %BUILD_TYPE% || (popd && exit /b 1)
    popd
EXIT /B 0

error:
    pause
exit /b %errorlevel%
