﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Primitives;
using SixLabors.ImageSharp.Processing.Drawing.Brushes;
using SixLabors.Shapes;

namespace SixLabors.ImageSharp.Processing.Drawing
{
    /// <summary>
    /// Adds extensions that allow the filling of polygon outlines to the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static class FillPathExtensions
    {
        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="path">The shape.</param>
        /// <param name="options">The graphics options.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, IPath path, GraphicsOptions options)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(brush, new ShapeRegion(path), options);

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="path">The path.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, IPath path)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(brush, new ShapeRegion(path), GraphicsOptions.Default);

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="path">The path.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, IPath path, GraphicsOptions options)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(new SolidBrush<TPixel>(color), path, options);

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="path">The path.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, IPath path)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(new SolidBrush<TPixel>(color), path);
    }
}