# Native plugin for Unity3D (sample)

Native plugin for "all" unity targets with shared c++ code.

## Main features

- Supports macOS (+editor), Windows (+editor), iOS, Android
- Simple and clear build process
- Shared c++ code
- Shared Objective-C shared code for ios, mac targets
- Shared Swift code for ios, mac targets
- Debug/Release configuration
- Source code sample for iOS target (useful for debug)

## Compiling

I compile all targets on macOS and use Windows only for windows(+editor) targets. 

### macOS

- Setup environment variable `ANDROID_NDK_HOME` target to Android NDK
- Open `plugin` folder
- run `buildall.sh`, special params `[-w add swift sample code] [-x generate xcode projects] [-r Release configuration] [-s copy sources for iOS target (useful for debug)]`

#### Swift

- Define `SWIFT` in Unity Player Settings window - `Scripting Define Symbols` setting
- Swift target copy additional files in project, feel free to edit or remove them. `Dummy.swift` can be removed if you have another `*.swift` files. `SwiftPostProcessor.cs` configures Xcode project.

MacOS(+editor) dylibs are huge because contains all swift dylibs (see `ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES`)

### windows

- Install CMake 3.6+, MSVS
- Open `plugin` folder
- run `buildall.bat`, open MSVS projects from `build/windows` and `build/windows_editor`

### make or MSVS or Xcode

This project is just a sample, I use "Unix Makefiles" on macOS and MSVS projects for windows, but theoretically you can generate Xcode projects for ios and mac targets. See `Plugin/buildall.[sh|bat]` for more information.

## MORE INFO

- [UnityNativeScripting](https://github.com/jacksondunstan/UnityNativeScripting) and [articles](https://jacksondunstan.com/articles/3938) by Jackson Dunstan
- Unity manuals [iOS native plugins](https://docs.unity3d.com/Manual/PluginsForIOS.html) and [Android native plugins](https://docs.unity3d.com/Manual/AndroidNativePlugins.html)
- [Android CMake guide](https://developer.android.com/ndk/guides/cmake) and [ABI management](https://developer.android.com/ndk/guides/abis)
- [Unity — How to Build a Bridge: iOS to Unity with Swift](https://medium.com/@SoCohesive/unity-how-to-build-a-bridge-ios-to-unity-with-swift-f23653f6261)

## TODO

- [webgl] support webgl target
- [android] test x86 library (arm64???)
- [windows] auto build MSVS projects
