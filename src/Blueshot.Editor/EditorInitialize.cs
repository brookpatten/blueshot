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

using blueshot.Base.Core;
using Dapplo.Ini;
using blueshot.Base.Interfaces;
using blueshot.Editor.FileFormatHandlers;

namespace blueshot.Editor
{
    public static class EditorInitialize
    {
        private static readonly ICoreConfiguration CoreConfig = IniConfigRegistry.GetSection<ICoreConfiguration>();

        public static void Initialize()
        {
            SimpleServiceProvider.Current.AddService<IFileFormatHandler>(
                    // All generic things, like gif, png, jpg etc.
                    CoreConfig.IsBetaTester? new ImageSharpFileFormatHandler() : new DefaultFileFormatHandler(),
                    // blueshot format
                    new blueshotFileFormatHandler(),
                    // For .svg support
                    new SvgFileFormatHandler(),
                    // For clipboard support
                    new DibFileFormatHandler(),
                    // .ico
                    new IconFileFormatHandler(),
                    // EMF & WMF
                    new MetaFileFormatHandler(),
                    // JPG XR
                    new WpfFileFormatHandler()
                );
        }
    }
}
