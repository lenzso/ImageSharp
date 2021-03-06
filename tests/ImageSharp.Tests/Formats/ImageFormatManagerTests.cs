﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using Moq;
using Xunit;


namespace SixLabors.ImageSharp.Tests
{
    public class ImageFormatManagerTests
    {
        public ImageFormatManager FormatsManagerEmpty { get; private set; }
        public ImageFormatManager DefaultFormatsManager { get; private set; }

        public ImageFormatManagerTests()
        {
            this.DefaultFormatsManager = Configuration.Default.ImageFormatsManager;
            this.FormatsManagerEmpty = new ImageFormatManager();
        }        

        [Fact]
        public void IfAutoloadWellKnownFormatsIsTrueAllFormatsAreLoaded()
        {
            Assert.Equal(1, this.DefaultFormatsManager.ImageEncoders.Select(item => item.Value).OfType<PngEncoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageEncoders.Select(item => item.Value).OfType<BmpEncoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageEncoders.Select(item => item.Value).OfType<JpegEncoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageEncoders.Select(item => item.Value).OfType<GifEncoder>().Count());

            Assert.Equal(1, this.DefaultFormatsManager.ImageDecoders.Select(item => item.Value).OfType<PngDecoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageDecoders.Select(item => item.Value).OfType<BmpDecoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageDecoders.Select(item => item.Value).OfType<JpegDecoder>().Count());
            Assert.Equal(1, this.DefaultFormatsManager.ImageDecoders.Select(item => item.Value).OfType<BmpDecoder>().Count());
        }

        [Fact]
        public void AddImageFormatDetectorNullthrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.AddImageFormatDetector(null);
            });
        }

        [Fact]
        public void RegisterNullMimeTypeEncoder()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetEncoder(null, new Mock<IImageEncoder>().Object);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetEncoder(ImageFormats.Bmp, null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetEncoder(null, null);
            });
        }

        [Fact]
        public void RegisterNullSetDecoder()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetDecoder(null, new Mock<IImageDecoder>().Object);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetDecoder(ImageFormats.Bmp, null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.DefaultFormatsManager.SetDecoder(null, null);
            });
        }

        [Fact]
        public void RegisterMimeTypeEncoderReplacesLast()
        {
            IImageEncoder encoder1 = new Mock<IImageEncoder>().Object;
            this.FormatsManagerEmpty.SetEncoder(TestFormat.GlobalTestFormat, encoder1);
            IImageEncoder found = this.FormatsManagerEmpty.FindEncoder(TestFormat.GlobalTestFormat);
            Assert.Equal(encoder1, found);

            IImageEncoder encoder2 = new Mock<IImageEncoder>().Object;
            this.FormatsManagerEmpty.SetEncoder(TestFormat.GlobalTestFormat, encoder2);
            IImageEncoder found2 = this.FormatsManagerEmpty.FindEncoder(TestFormat.GlobalTestFormat);
            Assert.Equal(encoder2, found2);
            Assert.NotEqual(found, found2);
        }

        [Fact]
        public void RegisterMimeTypeDecoderReplacesLast()
        {
            IImageDecoder decoder1 = new Mock<IImageDecoder>().Object;
            this.FormatsManagerEmpty.SetDecoder(TestFormat.GlobalTestFormat, decoder1);
            IImageDecoder found = this.FormatsManagerEmpty.FindDecoder(TestFormat.GlobalTestFormat);
            Assert.Equal(decoder1, found);

            IImageDecoder decoder2 = new Mock<IImageDecoder>().Object;
            this.FormatsManagerEmpty.SetDecoder(TestFormat.GlobalTestFormat, decoder2);
            IImageDecoder found2 = this.FormatsManagerEmpty.FindDecoder(TestFormat.GlobalTestFormat);
            Assert.Equal(decoder2, found2);
            Assert.NotEqual(found, found2);
        }        

        [Fact]
        public void AddFormatCallsConfig()
        {
            var provider = new Mock<IConfigurationModule>();
            var config = new Configuration();
            config.Configure(provider.Object);

            provider.Verify(x => x.Configure(config));
        }
    }
}
