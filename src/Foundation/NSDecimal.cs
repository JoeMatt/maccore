using System.Runtime.InteropServices;

namespace MonoMac.Foundation {
	[StructLayout (LayoutKind.Sequential)]
	public struct NSDecimal {
		public int fields;
		public short m1, m2, m3, m4, m5, m6, m7, m8;
	}
}
