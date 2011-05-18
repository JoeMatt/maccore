// 
// CGFont.cs: Implements the managed CGFont
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

#if !(GENERATOR || MONOMAC)
using MonoMac.CoreText;
#endif

using CGGlyph = System.UInt16;

namespace MonoMac.CoreGraphics {

	public class CGFont : INativeObject, IDisposable {
		internal IntPtr handle;

		[Preserve (Conditional=true)]
		internal CGFont (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");

			this.handle = handle;

			if (!owns)
				CGFontRetain (handle);
		}
		
		~CGFont ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontRetain (IntPtr font);
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGFontRelease (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGFontRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontCreateWithDataProvider(IntPtr provider);

		public static CGFont CreateFromProvider (CGDataProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");
			return new CGFont (CGFontCreateWithDataProvider (provider.Handle), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontCreateWithFontName(IntPtr CFStringRef_name);
		public static CGFont CreateWithFontName (string name)
		{
			using (CFString s = name){
				return new CGFont (CGFontCreateWithFontName (s.handle), true);
			}
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static IntPtr CGFontCreateCopyWithVariations(IntPtr font, IntPtr cf_dictionaryref_variations);
		//public static CGFont CreateCopyWithVariations ()
		//{
		//	throw new NotImplementedException ();
		//}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetNumberOfGlyphs(IntPtr font);
		public int NumberOfGlyphs {
			get {
				return CGFontGetNumberOfGlyphs (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetUnitsPerEm(IntPtr font);
		public int UnitsPerEm {
			get {
				return CGFontGetUnitsPerEm (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontCopyPostScriptName(IntPtr font);
		public string PostScriptName {
			get {
				using (var s = new CFString (CGFontCopyPostScriptName (handle), true))
					return (string) s;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontCopyFullName(IntPtr font);
		public string FullName {
			get {
				using (var s = new CFString (CGFontCopyFullName (handle), true))
					return (string) s;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetAscent(IntPtr font);
		public int Ascent {
			get {
				return CGFontGetAscent (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetDescent(IntPtr font);
		public int Descent {
			get {
				return CGFontGetDescent (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetLeading(IntPtr font);
		public int Leading {
			get {
				return CGFontGetLeading (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetCapHeight(IntPtr font);
		public int CapHeight {
			get {
				return CGFontGetCapHeight (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGFontGetXHeight(IntPtr font);
		public int XHeight {
			get {
				return CGFontGetXHeight (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static RectangleF CGFontGetFontBBox(IntPtr font);
		public RectangleF FontBBox {
			get {
				return CGFontGetFontBBox (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static float CGFontGetItalicAngle(IntPtr font);
		public float ItalicAngle {
			get {
				return CGFontGetItalicAngle (handle);
			}
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static float CGFontGetStemV(IntPtr font);
		public float StemV {
			get {
				return CGFontGetStemV (handle);
			}
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
	        //extern static CFArrayRef CGFontCopyVariationAxes(IntPtr font);

		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static CFDictionaryRef CGFontCopyVariations(IntPtr font);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGFontGetGlyphAdvances(IntPtr font, [In] CGGlyph [] glyphs, int count, [Out] int [] advances);
        public int[] GetGlyphAdvances (CGGlyph[] glyphs, int [] advances)
        {
            int count = glyphs.Length;
            
		    AssertCount  (count);
		    AssertLength ("glyphs",         glyphs, count);
			AssertLength ("advances",       advances, count, true);
			
			bool rtn = CGFontGetGlyphAdvances(handle, glyphs, count, advances);

 		    if(rtn == false)
 		        throw new Exception("Error in CGFontGetGlyphAdvances");

 		   return advances;
        }
        
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGFontGetGlyphBBoxes(IntPtr font, [In] CGGlyph[] glyphs, int count, [Out] RectangleF[] boundingRects);
		public RectangleF[] GetGlyphBoundingBoxes (CGGlyph [] glyphs, RectangleF [] boundingBoxes)
		{
		    int count = glyphs.Length;
            
		    AssertCount  (count);
		    AssertLength ("glyphs",         glyphs, count);
			AssertLength ("boundingBoxes",  boundingBoxes, count, true);
            
		    bool rtn = CGFontGetGlyphBBoxes(handle, glyphs, count, boundingBoxes);
		    
		    if(rtn == false)
		        throw new Exception("Error in CGFontGetGlyphBBoxes");
		        
		   return boundingBoxes;
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static ushort CGFontGetGlyphWithGlyphName(IntPtr font, IntPtr CFStringRef_name);
		public ushort GetGlyphWithGlyphName (string s)
		{
			using (var cs = new CFString (s)){
				return CGFontGetGlyphWithGlyphName (handle, cs.handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFontCopyGlyphNameForGlyph(IntPtr font, ushort glyph) ;
		public string GlyphNameForGlyph (ushort glyph)
		{
			using (var s = new CFString (CGFontCopyGlyphNameForGlyph (handle, glyph), true))
				return (string) s;
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static bool CGFontCanCreatePostScriptSubset(IntPtr font, CGFontPostScriptFormat format);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static CFDataRef CGFontCreatePostScriptSubset(IntPtr font, CFStringRef subsetName, CGFontPostScriptFormat format, ushort [] glyphs, size_t count, ushort [256] glyph_encodins);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static CFDataRef CGFontCreatePostScriptEncoding(IntPtr font, ushort [256] CGGlyph_encoding);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static /* CFArrayRef */ IntPtr CGFontCopyTableTags(IntPtr font);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static /* CFDataRef */ IntPtr CGFontCopyTableForTag(IntPtr font, int tag);

		// CFStringRef kCGFontVariationAxisName;
		// CFStringRef kCGFontVariationAxisMinValue;
		// CFStringRef kCGFontVariationAxisMaxValue;
		// CFStringRef kCGFontVariationAxisDefaultValue;

#if !(GENERATOR || MONOMAC)
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithGraphicsFont (IntPtr graphicsFont, float size, IntPtr matrix, IntPtr attributes);
		[Since(3,2)]
		public CTFont ToCTFont (float size)
		{
			return new CTFont (CTFontCreateWithGraphicsFont (handle, size, IntPtr.Zero, IntPtr.Zero), true);
		}

		[Since (3,2)]
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithGraphicsFont (IntPtr graphicsFont, float size, ref CGAffineTransform matrix, IntPtr attributes);
		public CTFont ToCTFont (float size, ref CGAffineTransform matrix)
		{
			return new CTFont (CTFontCreateWithGraphicsFont (handle, size, ref matrix, IntPtr.Zero), true);
		}
#endif
	
#if TODO
		ToCTFont() overloads where attributes is CTFontDescriptorRef
#endif // TODO

		[DllImport (Constants.CoreTextLibrary, EntryPoint="CGFontGetTypeID")]
		public extern static int GetTypeID ();
		
		// Copy from CTFont - should be somewhere more global
		static void AssertCount (int count)
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "cannot be negative");
		}

		static void AssertLength<T>(string name, T[] array, int count)
		{
			AssertLength (name, array, count, false);
		}

		static void AssertLength<T>(string name, T[] array, int count, bool canBeNull)
		{
			if (canBeNull && array == null)
				return;
			if (array == null)
				throw new ArgumentNullException (name);
			if (array.Length < count)
				throw new ArgumentException (string.Format ("{0}.Length cannot be < count", name), name);
		}
	}
}
