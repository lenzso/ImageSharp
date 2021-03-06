﻿namespace SixLabors.ImageSharp.Tests
{
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing.Quantization;

    using Xunit;

    public class QuantizedImageTests
    {
        [Fact]
        public void QuantizersDitherByDefault()
        {
            var palette = new PaletteQuantizer<Rgba32>();
            var octree = new OctreeQuantizer<Rgba32>();
            var wu = new WuQuantizer<Rgba32>();

            Assert.True(palette.Dither);
            Assert.True(octree.Dither);
            Assert.True(wu.Dither);
        }

        [Theory]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, true)]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, false)]
        public void PaletteQuantizerYieldsCorrectTransparentPixel<TPixel>(TestImageProvider<TPixel> provider, bool dither)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                Assert.True(image[0, 0].Equals(default(TPixel)));

                IQuantizer<TPixel> quantizer = new PaletteQuantizer<TPixel> { Dither = dither };

                foreach (ImageFrame<TPixel> frame in image.Frames)
                {
                    QuantizedFrame<TPixel> quantized = quantizer.Quantize(frame, 256);

                    int index = this.GetTransparentIndex(quantized);
                    Assert.Equal(index, quantized.Pixels[0]);
                }
            }
        }

        [Theory]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, true)]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, false)]
        public void OctreeQuantizerYieldsCorrectTransparentPixel<TPixel>(TestImageProvider<TPixel> provider, bool dither)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                Assert.True(image[0, 0].Equals(default(TPixel)));

                IQuantizer<TPixel> quantizer = new OctreeQuantizer<TPixel> { Dither = dither };

                foreach (ImageFrame<TPixel> frame in image.Frames)
                {
                    QuantizedFrame<TPixel> quantized = quantizer.Quantize(frame, 256);

                    int index = this.GetTransparentIndex(quantized);
                    Assert.Equal(index, quantized.Pixels[0]);
                }
            }
        }

        [Theory]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, true)]
        [WithFile(TestImages.Gif.Giphy, PixelTypes.Rgba32, false)]
        public void WuQuantizerYieldsCorrectTransparentPixel<TPixel>(TestImageProvider<TPixel> provider, bool dither)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                Assert.True(image[0, 0].Equals(default(TPixel)));

                IQuantizer<TPixel> quantizer = new WuQuantizer<TPixel>() { Dither = dither };

                foreach (ImageFrame<TPixel> frame in image.Frames)
                {
                    QuantizedFrame<TPixel> quantized = quantizer.Quantize(frame, 256);

                    int index = this.GetTransparentIndex(quantized);
                    Assert.Equal(index, quantized.Pixels[0]);
                }
            }
        }

        private int GetTransparentIndex<TPixel>(QuantizedFrame<TPixel> quantized)
            where TPixel : struct, IPixel<TPixel>
        {
            // Transparent pixels are much more likely to be found at the end of a palette
            int index = -1;
            var trans = default(Rgba32);
            for (int i = quantized.Palette.Length - 1; i >= 0; i--)
            {
                quantized.Palette[i].ToRgba32(ref trans);

                if (trans.Equals(default(Rgba32)))
                {
                    index = i;
                }
            }

            return index;
        }
    }
}