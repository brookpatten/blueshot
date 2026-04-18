/*
 * blueshot - a free and open source screenshot tool
 * Copyright (C) 2004-2026 Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: https://getblueshot.org/
 * The blueshot project is hosted on GitHub https://github.com/blueshot/blueshot
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using blueshot.Base.Core;
using blueshot.Base.Interfaces;
using blueshot.Base.Interfaces.Plugin;
using log4net;

namespace blueshot.Editor.FileFormatHandlers
{
    public class blueshotFileFormatHandler : AbstractFileFormatHandler, IFileFormatHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(blueshotFileFormatHandler));
        private readonly IReadOnlyCollection<string> _ourExtensions = new [] { ".blueshot" };
        public blueshotFileFormatHandler()
        {
            SupportedExtensions[FileFormatHandlerActions.LoadDrawableFromStream] = _ourExtensions;
            SupportedExtensions[FileFormatHandlerActions.LoadFromStream] = _ourExtensions;
            SupportedExtensions[FileFormatHandlerActions.SaveToStream] = _ourExtensions;
            SupportedExtensions[FileFormatHandlerActions.SaveToFile] = _ourExtensions;
            SupportedExtensions[FileFormatHandlerActions.LoadFromFile] = _ourExtensions;
        }

        public override bool TrySaveToStream(Bitmap bitmap, Stream stream, string extension, ISurface surface = null, SurfaceOutputSettings surfaceOutputSettings = null)
        {
            if (surface == null)
            {
                return false;
            }

            try
            {
                bitmap.Save(stream, ImageFormat.Png);
                using MemoryStream tmpStream = RecyclableMemoryStreamFactory.GetStream("blueshotFileFormatHandler.SaveToStream");
                long bytesWritten = surface.SaveElementsToStream(tmpStream);
                using BinaryWriter writer = new BinaryWriter(tmpStream);
                writer.Write(bytesWritten);
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                byte[] marker = Encoding.ASCII.GetBytes($"blueshot{v.Major:00}.{v.Minor:00}");
                writer.Write(marker);
                tmpStream.WriteTo(stream);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Couldn't save surface as .blueshot: ", ex);
            }

            return false;
        }

        public override bool TryLoadFromStream(Stream stream, string extension, out Bitmap bitmap)
        {
            try
            {
                var surface = LoadSurface(stream);
                bitmap = (Bitmap)surface.GetImageForExport();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Couldn't load .blueshot: ", ex);
            }
            bitmap = null;
            return false;
        }

        private ISurface LoadSurface(Stream surfaceFileStream)
        {
            var returnSurface = SimpleServiceProvider.Current.GetInstance<Func<ISurface>>().Invoke();
            Bitmap captureBitmap;

            // Fixed problem that the bitmap stream is disposed... by Cloning the image
            // This also ensures the bitmap is correctly created
            using (Image tmpImage = Image.FromStream(surfaceFileStream, true, true))
            {
                Log.DebugFormat("Loaded capture from .blueshot file with Size {0}x{1} and PixelFormat {2}", tmpImage.Width, tmpImage.Height, tmpImage.PixelFormat);
                captureBitmap = ImageHelper.Clone(tmpImage) as Bitmap;
            }

            // Start at -14 read "blueshotXX.YY" (XX=Major, YY=Minor)
            const int markerSize = 14;
            surfaceFileStream.Seek(-markerSize, SeekOrigin.End);
            using (var streamReader = new StreamReader(surfaceFileStream))
            {
                var blueshotMarker = streamReader.ReadToEnd();
                if (!blueshotMarker.StartsWith("blueshot"))
                {
                    throw new ArgumentException("Stream is not a blueshot file!");
                }

                Log.InfoFormat("blueshot file format: {0}", blueshotMarker);
                const int fileSizeLocation = 8 + markerSize;
                surfaceFileStream.Seek(-fileSizeLocation, SeekOrigin.End);
                using BinaryReader reader = new BinaryReader(surfaceFileStream);
                long bytesWritten = reader.ReadInt64();
                surfaceFileStream.Seek(-(bytesWritten + fileSizeLocation), SeekOrigin.End);
                returnSurface.LoadElementsFromStream(surfaceFileStream);
            }

            if (captureBitmap != null)
            {
                returnSurface.Image = captureBitmap;
                Log.InfoFormat("Information about .blueshot file: {0}x{1}-{2} Resolution {3}x{4}", captureBitmap.Width, captureBitmap.Height, captureBitmap.PixelFormat, captureBitmap.HorizontalResolution, captureBitmap.VerticalResolution);
            }

            return returnSurface;
        }
    }
}
