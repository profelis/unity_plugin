#!/usr/bin/env bash

set -x

if [ "$(uname)" == "Darwin" ]; then
rm -rf build/ios
rm -rf build/mac
rm -rf build/mac_editor
rm -rf build/android
else
rm -rf build/windows_x86
rm -rf build/windows_x86_64
rm -rf build/windows_editor_x86
rm -rf build/windows_editor_x86_64
fi
