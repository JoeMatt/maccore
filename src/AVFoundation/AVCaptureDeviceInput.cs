//
// AVFoundation.cs: This file describes the API that the generator will produce for AVFoundation
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AVFoundation {
	public partial class AVCaptureDeviceInput {
		[Obsolete ("Use FromDevice (AVCaptureDevice, ref NSError) instead")]
		static public AVCaptureDeviceInput FromDevice (AVCaptureDevice device, IntPtr handle)
		{
			NSError error;
			return FromDevice (device, out error);
		}

		static public AVCaptureDeviceInput FromDevice (AVCaptureDevice device)
		{
			NSError error;
			return FromDevice (device, out error);
		}
		
		static NSError globalerr;
		
		[Obsolete ("Use AVCaptureDeviceInput (AVCaptureDevice, ref NSError) instead")]
		public AVCaptureDeviceInput (AVCaptureDevice device, IntPtr handle) : this (device, out globalerr)
		{
		}
	}
}