#!/usr/bin/env bash

set -x

PLUGIN_DIR=`pwd`
mkdir -p build
pushd build

function pre_cmake()
{
    BUILD_DIR=$1
    mkdir -p $1
    pushd $1
}

function post_cmake()
{
    popd
}
function build_ios()
{
    pre_cmake ios
    cmake -G "Unix Makefiles" -DCMAKE_TOOLCHAIN_FILE=${PLUGIN_DIR}/iOS.cmake ${PLUGIN_DIR}
#    cmake -G "Xcode" -DCMAKE_TOOLCHAIN_FILE=${PLUGIN_DIR}/iOS.cmake ${PLUGIN_DIR}
    make
    post_cmake
}

function build_mac()
{
    pre_cmake mac
    cmake -G "Unix Makefiles" ${PLUGIN_DIR}
#    cmake -G "Xcode" ${PLUGIN_DIR}
    make
    post_cmake
}

function build_mac_editor()
{
    pre_cmake mac_editor
    cmake -G "Unix Makefiles" -DEDITOR=TRUE ${PLUGIN_DIR}
#    cmake -G "Xcode" -DEDITOR=TRUE ${PLUGIN_DIR}
    make
    post_cmake
}

function build_android()
{
    ANDROID_ABI=$1
    pre_cmake android/${ANDROID_ABI}
    cmake -G "Unix Makefiles" -DCMAKE_TOOLCHAIN_FILE=${ANDROID_HOME}/build/cmake/android.toolchain.cmake -DANDROID_ABI=${ANDROID_ABI} ${PLUGIN_DIR}
    make
    post_cmake
}

function build_windows()
{
    pre_cmake windows
    cmake -G "Visual Studio 15 2017 Win64" ${PLUGIN_DIR}
    post_cmake
}

function build_windows_editor()
{
    pre_cmake windows_editor
    cmake -G "Visual Studio 15 2017 Win64" -DEDITOR=TRUE ${PLUGIN_DIR}
    post_cmake
}


if [ "$(uname)" == "Darwin" ]; then
build_ios
build_mac
build_mac_editor
build_android armeabi-v7a
build_android arm64-v8a
build_android x86
else
build_windows
build_windows_editor
fi

popd