//
// Created by deep on 24/08/2018.
//

#include "MathUtils.h"
#include "sample.h"

extern "C" int UNITY_INTERFACE_EXPORT plugin_abs(int a) {
  MathUtils utils;
  return utils.Abs(a);
}

extern "C" int UNITY_INTERFACE_EXPORT plugin_sum(int a, int b) {
  MathUtils utils;
  return utils.Sum(a, b);
}