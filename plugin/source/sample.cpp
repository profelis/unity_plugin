/*
* sample.cpp
* Visit https://github.com/profelis/unity_plugin for documentation, updates and examples.
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

#include <cstring>
#include "MathUtils.h"
#include "sample.h"

extern "C" int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API plugin_abs(int a) {
    MathUtils utils;
    return utils.Abs(a);
}

extern "C" int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API plugin_sum(int a, int b) {
    MathUtils utils;
    return utils.Sum(a, b);
}

extern "C" bool UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API isEqualsString(const char *a, const char *b) {
    return strcmp(a, b) == 0;
}

extern "C" const char* UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API getString()
{
    char* str = new char[200];
    strcpy(str, "Dynamic string");
    return str;
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API getStringTo(char* str, size_t strLength)
{
    strncpy(str, "Hello from c++", strLength);
}

extern "C" const char* UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API getConstString()
{
    return "some const string";
}