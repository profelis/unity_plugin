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

/// <summary>
/// Common marshaling utils
/// https://www.mono-project.com/docs/advanced/pinvoke/#manual-marshaling
/// </summary>
public static class MarshalingCommon
{
    public static string PtrToAnsiString(IntPtr p)
    {
        if (p == IntPtr.Zero)
            return null;
        return Marshal.PtrToStringAnsi(p);
    }

    public static string PtrToUniString(IntPtr p)
    {
        if (p == IntPtr.Zero)
            return null;
        return Marshal.PtrToStringUni(p);
    }

    public static string PtrTBSTRString(IntPtr p)
    {
        if (p == IntPtr.Zero)
            return null;
        return Marshal.PtrToStringBSTR(p);
    }

    public static string[] PtrToStringArray(IntPtr stringArray, Func<IntPtr, string> transformer)
    {
        if (stringArray == IntPtr.Zero)
            return new string[] { };

        int argc = CountStrings(stringArray);
        return PtrToStringArray(argc, stringArray, transformer);
    }

    private static int CountStrings(IntPtr stringArray)
    {
        int count = 0;
        while (Marshal.ReadIntPtr(stringArray, count * IntPtr.Size) != IntPtr.Zero)
            ++count;
        return count;
    }

    private static string[] PtrToStringArray(int count, IntPtr stringArray, Func<IntPtr, string> transformer)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException("count", "< 0");
        if (stringArray == IntPtr.Zero)
            return new string[count];

        string[] members = new string[count];
        for (int i = 0; i < count; ++i)
        {
            IntPtr s = Marshal.ReadIntPtr(stringArray, i * IntPtr.Size);
            members[i] = transformer(s);
        }

        return members;
    }
}