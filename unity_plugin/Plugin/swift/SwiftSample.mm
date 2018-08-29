//
// Created by deep on 27/08/2018.
//

#import "SwiftSample.h"
#import "unity_plugin-Swift.h"

int swift_abs(int a) {
	SwiftMathUtils *utils = [SwiftMathUtils new];
	return [utils absWithA:a];
}

int swift_sum(int a, int b) {
	SwiftMathUtils *utils = [SwiftMathUtils new];
	return [utils sumWithA:a b:b];
}
