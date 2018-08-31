#!/usr/bin/env bash

SWIFT=false
XCODE=false
SOURCE=false
BUILD_TYPE="Debug"

OUTPUT_DIR="`pwd`/../Assets/Plugins/unity_plugin"

while getopts ":wxsr" o; do
    case "${o}" in
        w)
            SWIFT=true; XCODE=true; ;;
        x)
            XCODE=true; ;;
        r)
            BUILD_TYPE="Release"; ;;
        s)
            SOURCE=true; ;;
        *)
            echo "Usage: $0 [-w swift] [-x xcode] [-r release]"; exit 1; ;;
    esac
done

set -x

MAC_GENERATOR="Unix Makefiles"
IOS_GENERATOR="Unix Makefiles"
if [ "${XCODE}" = true ] ; then
    MAC_GENERATOR="Xcode"
    IOS_GENERATOR="Xcode"
fi

PLUGIN_DIR=`pwd`

function pre_build()
{
    BUILD_DIR=$1
    mkdir -p $1
    pushd $1
}

function post_build()
{
    popd
}

function copy_sources()
{
    mkdir -p "${OUTPUT_DIR}"
    cp -r ./Source "${OUTPUT_DIR}"
    cp -r ./External "${OUTPUT_DIR}"
    cp -r ./objective_c "${OUTPUT_DIR}"
    if [ "${SWIFT}" = true ] ; then
        cp -r ./swift "${OUTPUT_DIR}"
        cp -r ./swiftResources/ "${OUTPUT_DIR}"
    fi
}

function build_ios()
{
    if [ "${SOURCE}" = true ]; then
        copy_sources
    else
        if [ "${SWIFT}" = true ] ; then
            mkdir -p "${OUTPUT_DIR}/iOS"
            cp -r ./swiftResources/ "${OUTPUT_DIR}"
            # empty swift file - activate swift support in Xcode
            cp -r ./swiftDummy/ "${OUTPUT_DIR}/iOS"
        fi
        pre_build build/ios
        cmake -G "${IOS_GENERATOR}" -DSWIFT=${SWIFT} -DXCODE=${XCODE} -DTARGET_DIR="${OUTPUT_DIR}" -DCMAKE_BUILD_TYPE=${BUILD_TYPE} "-DCMAKE_OSX_ARCHITECTURES=arm64;armv7" -DCMAKE_TOOLCHAIN_FILE=${PLUGIN_DIR}/iOS.cmake ${PLUGIN_DIR}
        if [ "${IOS_GENERATOR}" = "Unix Makefiles" ] ; then
            make VERBOSE=1
        elif [ "${XCODE}" = true ] ; then
            xcodebuild -configuration "${BUILD_TYPE}" -target ALL_BUILD build
        fi
        post_build
    fi
}

function build_mac()
{
    if [ "${SOURCE}" = true ]; then
        echo "OSX doesn't support native sources. Force compile library"
    fi
    pre_build build/mac
    cmake -G "${MAC_GENERATOR}" -DSWIFT=${SWIFT} -DXCODE=${XCODE} -DTARGET_DIR="${OUTPUT_DIR}" -DCMAKE_BUILD_TYPE=${BUILD_TYPE} ${PLUGIN_DIR}
    if [ "${MAC_GENERATOR}" = "Unix Makefiles" ] ; then
        make VERBOSE=1
    elif [ "${XCODE}" = true ] ; then
        xcodebuild -configuration "${BUILD_TYPE}" -target ALL_BUILD build
    fi
    post_build
}

function build_mac_editor()
{
    if [ "${SOURCE}" = true ]; then
        echo "Editor doesn't support native sources. Force compile library"
    fi
    pre_build build/mac_editor
    cmake -G "${MAC_GENERATOR}" -DSWIFT=${SWIFT} -DXCODE=${SWIFT} -DTARGET_DIR="${OUTPUT_DIR}" -DCMAKE_BUILD_TYPE=${BUILD_TYPE} -DEDITOR=TRUE ${PLUGIN_DIR}
    if [ "${MAC_GENERATOR}" = "Unix Makefiles" ] ; then
        make VERBOSE=1
    elif [ "${XCODE}" = true ] ; then
        xcodebuild -configuration "${BUILD_TYPE}" -target ALL_BUILD build
    fi
    post_build
}

function build_android()
{
    ANDROID_ABI=$1
    pre_build build/android/${ANDROID_ABI}
    cmake -G "Unix Makefiles" -DCMAKE_TOOLCHAIN_FILE=${ANDROID_NDK_HOME}/build/cmake/android.toolchain.cmake -DTARGET_DIR="${OUTPUT_DIR}" -DCMAKE_BUILD_TYPE=${BUILD_TYPE} -DANDROID_ABI=${ANDROID_ABI} ${PLUGIN_DIR}
    make VERBOSE=1
    post_build
}

function build_windows()
{
    pre_build build/windows
    cmake -G "Visual Studio 15 2017 Win64" -DCMAKE_BUILD_TYPE=${BUILD_TYPE} -DTARGET_DIR="${OUTPUT_DIR}" ${PLUGIN_DIR}
    post_build
}

function build_windows_editor()
{
    pre_build build/windows_editor
    cmake -G "Visual Studio 15 2017 Win64" -DEDITOR=TRUE -DCMAKE_BUILD_TYPE=${BUILD_TYPE} -DTARGET_DIR="${OUTPUT_DIR}" ${PLUGIN_DIR}
    post_build
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
