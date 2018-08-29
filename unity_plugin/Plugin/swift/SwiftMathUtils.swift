//
// Created by deep on 28/08/2018.
//
import Foundation

@objcMembers public class SwiftMathUtils : NSObject {

	public func abs(a:Int) -> Int {
		return a < 0 ? -a : a;
	}
	
	public func sum(a:Int, b:Int) -> Int {
		return a + b;
	}
}
