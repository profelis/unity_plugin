# Native plugin for Unity3D (sample)

Native plugin for "all" unity targets with shared c++ code.

## Main features

- Supports macOS (+editor), Windows (+editor), iOS, Android
- Simple and clear build process

# Compiling

I compile all targets on macOS and use Windows only for windows(+editor) targets. 

## macOS

- Setup environment variable `ANDROID_HOME` target to Android NDK
- Open `Plugin` folder
- run `buildall.sh`

## windows

- Install CMake 3.6+, MSVS
- Open `Plugin` folder
- run `buildall.bat`

## make or MSVS or Xcode

This project is just a sample, I use "Unix Makefiles" on macOS and MSVS projects for windows, but theoretically you can generate Xcode projects for ios and mac targets. See `Plugin/buildall.[sh|bat]` for more information.

## MORE INFO

- [UnityNativeScripting](https://github.com/jacksondunstan/UnityNativeScripting) and [articles](https://jacksondunstan.com/articles/3938) by Jackson Dunstan
- Unity manuals [iOS native plugins](https://docs.unity3d.com/Manual/PluginsForIOS.html) and [Android native plugins](https://docs.unity3d.com/Manual/AndroidNativePlugins.html)
- [Android CMake guide](https://developer.android.com/ndk/guides/cmake) and [ABI management](https://developer.android.com/ndk/guides/abis)

## TODO

- [webgl] support webgl target
- [android] test x86 library (arm64???)
- [ios] skip library compilation, just copy src files to Plugin folder
- [all] debug/release builds
- [ios] obj-c sample code
- [ios] swift sample
