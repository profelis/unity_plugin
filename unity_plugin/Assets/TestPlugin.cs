using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TestPlugin : MonoBehaviour
{
    public Text Label;

#if !UNITY_EDITOR && UNITY_IOS
    const string PLUGIN_NAME = "__Internal";
#elif !UNITY_EDITOR && UNITY_WEBGL
    const string PLUGIN_NAME = "__Internal";
#else
    const string PLUGIN_NAME = "unity_plugin";
#endif

    [DllImport(PLUGIN_NAME)]
    static extern int plugin_abs(int a);

    [DllImport(PLUGIN_NAME)]
    static extern int plugin_sum(int a, int b);

#if UNITY_EDITOR_OSX || UNITY_IOS || UNITY_STANDALONE_OSX
    [DllImport(PLUGIN_NAME)]
    static extern int objc_abs(int a);

    [DllImport(PLUGIN_NAME)]
    static extern int objc_sum(int a, int b);
#endif

    void Start()
    {
        Label.text = "Start test\n";
        Label.text += plugin_abs(1) + "\n";
        Label.text += plugin_abs(-2) + "\n";
        Label.text += plugin_sum(-4, 7) + "\n";
        Label.text += plugin_sum(2, 2) + "\n";
#if UNITY_EDITOR_OSX || UNITY_IOS || UNITY_STANDALONE_OSX
        Label.text += objc_abs(5) + "\n";
        Label.text += objc_sum(5, 1) + "\n";
        Label.text += ("7 Test ended");
#else
        Label.text += ("5 Test ended");
#endif
    }
}