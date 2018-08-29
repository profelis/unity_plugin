using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public static class SwiftPostProcessor
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // We need to construct our own PBX project path that corrently refers to the Bridging header
            // var projPath = PBXProject.GetPBXProjectPath(buildPath);
            var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
            Debug.Log("SwiftPostProcessor: Processing project: " + projPath);
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);

            var targetName = PBXProject.GetUnityTargetName();
            Debug.Log("SwiftPostProcessor: Target: " + targetName);
            var targetGuid = proj.TargetGuidByName(targetName);

            //// Configure build settings
            proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/unity_plugin/Unity-iPhone-Bridging-Header.h");
            proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "ReplayKitUnityBridge-Swift.h");
            proj.SetBuildProperty(targetGuid, "SWIFT_VERSION", "4.2");
            proj.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
//            proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

            proj.WriteToFile(projPath);
        }
    }
}