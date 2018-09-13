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
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestPlugin : MonoBehaviour
{
    public Text Label;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_WEBGL || UNITY_WEBGL_API)
    const string PLUGIN_NAME = "__Internal";
#else
    const string PLUGIN_NAME = "unity_plugin";
#endif

    [DllImport(PLUGIN_NAME)]
    static extern int plugin_abs(int a);

    [DllImport(PLUGIN_NAME)]
    static extern int plugin_sum(int a, int b);

    [DllImport(PLUGIN_NAME)]
    static extern bool isEqualsString([MarshalAs(UnmanagedType.LPStr)] string a, [MarshalAs(UnmanagedType.LPStr)] string b);

    [DllImport(PLUGIN_NAME)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    static extern string getString();

    [DllImport(PLUGIN_NAME)]
    static extern void getStringTo(StringBuilder str, int strLength);

    [DllImport(PLUGIN_NAME)]
    static extern IntPtr getConstString();

    [DllImport(PLUGIN_NAME)]
    static extern void WriteIntArray(int[] array, int length);

    [DllImport(PLUGIN_NAME)]
    static extern void WriteVector3Array(IntPtr array, int length);

    [DllImport(PLUGIN_NAME)]
    public static extern float PassVector3ByValue(Vector3 s);

    [DllImport(PLUGIN_NAME)]
    public static extern float PassVector3ByRefIn(ref Vector3 s);

    [DllImport(PLUGIN_NAME)]
    public static extern void PassVector3ByRefOut(out Vector3 s); // [Out] for classes

    [DllImport(PLUGIN_NAME)]
    public static extern void PassVector3ByRefInOut(ref Vector3 s); // [In, Out] for classes

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
#endif // SWIFT

#endif // UNITY_EDITOR_OSX || UNITY_IOS || UNITY_STANDALONE_OSX

    void Start()
    {
        var res = new StringBuilder();
        res.AppendLine("Start test:");

        var abs1 = plugin_abs(1);
        res.AppendFormat("{0} {1}", abs1, Res(abs1 == 1));
        var abs2 = plugin_abs(-2);
        res.AppendFormat(" {0} {1}", abs2, Res(abs2 == 2));
        var sum1 = plugin_sum(-4, 7);
        res.AppendFormat(" {0} {1}\n", sum1, Res(sum1 == 3));
        var sum2 = plugin_sum(2, 2);
        res.AppendFormat("{0} {1}", sum2, Res(sum2 == 4));
#if UNITY_EDITOR_OSX || UNITY_IOS || UNITY_STANDALONE_OSX
        var abs3 = objc_abs(5);
        res.AppendFormat(" {0} {1}", abs3, Res(abs3 == 5));
        var sum3 = objc_sum(5, 1);
        res.AppendFormat(" {0} {1}\n", sum3, Res(sum3 == 6));
#if SWIFT
        res.AppendLine("Swift tests:");
        var swiftFailed = false;
        try
        {
            var abs4 = swift_abs(-7);
            res.AppendFormat("{0} {1}", abs4, Res(abs4 == 7));
            var sum4 = swift_sum(1, 7);
            res.AppendFormat(" {0} {1}\n", sum4, Res(sum4 == 8));
        }
        catch (Exception e)
        {
            Debug.Log(e);
            swiftFailed = true;
            res.AppendLine("swift test failed");
        }
        if (swiftFailed)
            res.AppendLine("Math Test ended");
#endif // SWIFT
#endif
        res.AppendLine("Math Test ended");

        res.AppendLine("\nStart string tests:");

        res.AppendFormat("equals {0} and not equals {1}\n", Res(isEqualsString("foo", "foo")), Res(!isEqualsString("foo", "FOO")));

        var dynamicString = getString();
        res.AppendFormat("getString returns: {0} {1}\n", dynamicString, Res(dynamicString.Equals("Dynamic string")));

        var sb = new StringBuilder(200);
        getStringTo(sb, sb.Capacity);
        res.AppendFormat("StringBuilder contains {0} {1}\n", sb, Res(sb.ToString().Equals("Hello from c++")));

        var constString = MarshalingCommon.PtrToAnsiString(getConstString());
        res.AppendFormat("const string: {0} {1}\n", constString, Res(constString.Equals("some const string")));

        res.AppendLine("String test ended");

        res.AppendLine("\nArrays test start:");

        var intArray = new int [3];
        WriteIntArray(intArray, intArray.Length);
        var valid = intArray[0] == 1 && intArray[1] == 2 && intArray[2] == 3;
        res.AppendFormat("int array:{0}, {1}, {2} {3}\n", intArray[0], intArray[1], intArray[2], Res(valid));

        var vector3Array = new Vector3 [3];
        var vector3ArrayHandle = GCHandle.Alloc(vector3Array, GCHandleType.Pinned);
        WriteVector3Array(vector3ArrayHandle.AddrOfPinnedObject(), vector3Array.Length);
        res.AppendFormat("vec3 array[0]:{0} {1}\n", vector3Array[0], Res(Vector3.right.Equals(vector3Array[0])));
        res.AppendFormat("vec3 array[1]:{0} {1}\n", vector3Array[1], Res(Vector3.up.Equals(vector3Array[1])));
        res.AppendFormat("vec3 array[2]:{0} {1}\n", vector3Array[2], Res(Vector3.forward.Equals(vector3Array[2])));
        vector3ArrayHandle.Free();

        res.AppendLine("Arrays test ended");

        res.AppendLine("\nStructs test start:");
        var len = PassVector3ByValue(Vector3.one);
        res.AppendFormat("Vector3.one len: {0} {1}\n", len, Res(Mathf.Approximately(len, Vector3.one.magnitude)));

        var vec = Vector3.one;
        var len2 = PassVector3ByRefIn(ref vec);
        res.AppendFormat("ref Vector3.one len: {0} {1}\n", len2, Res(Mathf.Approximately(len2, Vector3.one.magnitude)));

        var vec2 = Vector3.zero;
        PassVector3ByRefOut(out vec2);
        res.AppendFormat("out Vector3.zero->one : {0} {1}\n", vec2, Res(Vector3.one.Equals(vec2)));

        var vec3 = Vector3.one;
        PassVector3ByRefInOut(ref vec3);
        res.AppendFormat("int out Vector3.one * 2 : {0} {1}\n", vec3, Res((Vector3.one * 2).Equals(vec3)));

        res.AppendLine("Structs test ended");
        Label.text = res.ToString();
    }

    string Res(bool res)
    {
        return res ? "<color=#00ff00>Correct</color>" : "<color=#ff0000>Error</color>";
    }
}