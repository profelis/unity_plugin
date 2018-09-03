/*
* For more information visit https://github.com/profelis/unity_plugin
*
* Copyright (c) 2018 Dmitri Granetchi
*
* Permission is hereby granted, free of charge, to any person
* obtaining a copy of this software and associated documentation
* files (the "Software"), to deal in the Software without
* restriction, including without limitation the rights to use,
* copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following
* conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
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

#if SWIFT
    [DllImport(PLUGIN_NAME)]
    static extern int swift_abs(int a);

    [DllImport(PLUGIN_NAME)]
    static extern int swift_sum(int a, int b);
#endif
#endif

    void Start()
    {
        Label.text = "Start test:\n";
        Label.text += plugin_abs(1) + "\n";
        Label.text += plugin_abs(-2) + "\n";
        Label.text += plugin_sum(-4, 7) + "\n";
        Label.text += plugin_sum(2, 2) + "\n";
#if UNITY_EDITOR_OSX || UNITY_IOS || UNITY_STANDALONE_OSX
        Label.text += objc_abs(5) + "\n";
        Label.text += objc_sum(5, 1) + "\n";
#if SWIFT
        Label.text += "Swift tests:\n";
        var swiftFailed = false;
        try
        {
            Label.text += swift_abs(-7) + "\n";
            Label.text += swift_sum(1, 7) + "\n";
            Label.text += ("9 Test ended\n");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            swiftFailed = true;
            Label.text += ("swift test failed\n");
        }
        if (swiftFailed)
            Label.text += ("7 Test ended\n");
#else // SWIFT
        Label.text += ("7 Test ended\n");
#endif // SWIFT

#else
        Label.text += ("5 Test ended\n");
#endif
    }
}