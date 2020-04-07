using kwd.BoxOBlazor.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kwd.BoxOBlazor.Tests.Util
{
	[TestClass]
	public class ColorHelperTests
	{
		[TestMethod]
		public void HexConversions()
        {
			var (r, g, b) = ColorHelper.HexToRgb("#FF03FF ");
			
			Assert.AreEqual((byte)255, r);
			Assert.AreEqual((byte)3, g);
            Assert.AreEqual((byte)255, b);

			var hex = ColorHelper.RGBToHex(255, 3, 255);

			Assert.AreEqual("#FF03FF", hex);
		}

		[TestMethod]
        public void HSLConversions()
        {
            var rgbClr = ColorHelper.HSLToRGB(0, 1.0f, .5f);

            Assert.IsTrue(rgbClr.R == 255);

            var hsl = ColorHelper.RGBToHSL(255, 255, 255);

            Assert.IsTrue(hsl.Lightness.Like(1F));
        }
	}
}
