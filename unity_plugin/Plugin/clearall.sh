#!/usr/bin/env bash

pushd build

if [ "$(uname)" == "Darwin" ]; then
rm -rf ios
rm -rf mac
rm -rf mac_editor
rm -rf android
else
rm -rf windows
rm -rf windows_editor
fi

popd