
set PLUGIN_DIR=%cd%

pushd build
call :build_windows
call :build_windows_editor
popd

echo "Done."

EXIT /B 0

:build_windows
    mkdir windows
    pushd windows
    cmake -G "Visual Studio 15 2017 Win64" %PLUGIN_DIR%
    popd
EXIT /B 0


:build_windows_editor
    mkdir windows_editor
    pushd windows_editor
    cmake -G "Visual Studio 15 2017 Win64" -DEDITOR=TRUE %PLUGIN_DIR%
    popd
EXIT /B 0