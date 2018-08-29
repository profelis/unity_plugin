//
// Created by deep on 27/08/2018.
//

#import "ObjcSample.h"
#include "ObjectiveMathUtils.h"

#if ! __has_feature(objc_arc)
#error "ARC is off"
#endif

int objc_abs(int a) {
  ObjectiveMathUtils *utils = [[ObjectiveMathUtils alloc] init];
  return [utils abs:a];
}

int objc_sum(int a, int b) {
  ObjectiveMathUtils *utils = [[ObjectiveMathUtils alloc] init];
  return [utils sum:a andB:b];
}