# Native plugin for Unity3D (sample)

Native plugin for "all" unity targets with shared c++ code.

## Main features

- Supports macOS (+editor), Windows (+UWP) (+editor), iOS, Android, WebGL
- Simple and clear build process
- Shared c++ code
- Shared Objective-C shared code for ios, mac targets
- Shared Swift code for ios, mac targets
- Debug/Release configuration
- Source code sample for iOS target (useful for debug)

## Compiling

I compile all targets on macOS and use Windows only for windows targets.

### macOS

- Setup environment variable `ANDROID_NDK_HOME` target to Android NDK
- Open `plugin` folder
- run `buildall.sh`, special params `[-w add swift sample code] [-x generate xcode projects] [-r Release configuration] [-s copy sources for iOS target (useful for debug)] [-l webgl build]`

#### Swift

- Define `SWIFT` in Unity Player Settings window - `Scripting Define Symbols` setting
- Swift target copy additional files in project, feel free to edit or remove them. `Dummy.swift` can be removed if you have another `*.swift` files. `SwiftPostProcessor.cs` configures Xcode project.

MacOS(+editor) dylibs are huge because contains all swift dylibs (see `ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES`)

### WebGL static library (WIP)

Can't get working *.bc library right now (WIP)

```text
opt: error loading file '<projectpath>/Assets/Plugins/unity_plugin/webgl/libunity_plugin.bc'ERROR:root:Failed to run llvm optimizations: 
   UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr)
```

- Install [Emscripten](https://kripken.github.io/emscripten-site/docs/getting_started/downloads.html)
- Activate Emscripten `source ./emsdk_env.sh`

### WebGL sources

`buildall.sh -l -s` works perfect, some include directives fail sometimes, simple place all *.cpp and *.h files in root Plugins folder

### Windows

- Install CMake 3.6+, MSVS, UWP SDK (optional)
- Open `plugin` folder
- run `buildall.bat`, special params `[-r Release configuration]`

### make or MSVS or Xcode

See `plugin/buildall.[sh|bat]` for more information.

## More info

- [UnityNativeScripting](https://github.com/jacksondunstan/UnityNativeScripting) and [articles](https://jacksondunstan.com/articles/3938) by Jackson Dunstan

### iOS & MacOS

- Unity3D manual [iOS native plugins](https://docs.unity3d.com/Manual/PluginsForIOS.html)
- [Integrating native iOS code into Unity](https://medium.com/@rolir00li/integrating-native-ios-code-into-unity-e844a6131c21)
- [Unity — How to Build a Bridge: iOS to Unity with Swift](https://medium.com/@SoCohesive/unity-how-to-build-a-bridge-ios-to-unity-with-swift-f23653f6261)

### Android

- Unity3D manual [Native c++ plugins for Android](https://docs.unity3d.com/Manual/AndroidNativePlugins.html)
- [Android CMake guide](https://developer.android.com/ndk/guides/cmake)
- [ABI management](https://developer.android.com/ndk/guides/abis)

### WebGL

- [Emscripten](https://kripken.github.io/emscripten-site/index.html)
- [Building Emscripten project](https://kripken.github.io/emscripten-site/docs/compiling/Building-Projects.html)
- [How to use native libraries on Node.js with Emscripten](https://willowtreeapps.com/ideas/how-to-use-native-libraries-on-node-js-with-emscripten)

### Marshaling

- [Interop with Native Libraries](https://www.mono-project.com/docs/advanced/pinvoke/#manual-marshaling)
- [unity-native-array](https://github.com/keijiro/unity-native-array) by keijiro - example with array marshaling
- [Unity c++ Native Plugin Examples](https://bravenewmethod.com/2017/10/30/unity-c-native-plugin-examples/)

## Todo

- [android] test x86 library
- [webgl] static library
