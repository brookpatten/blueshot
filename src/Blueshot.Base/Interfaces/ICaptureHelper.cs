/*
 * blueshot - a free and open source screenshot tool
 * Copyright (C) 2004-2026  Thomas Braun, Jens Klingen, Robin Krom
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
using blueshot.Base.Core.Enums;

namespace blueshot.Base.Interfaces
{
    /// <summary>
    /// This is a temporary solution to provide the CaptureHelper functionality via an interface
    /// </summary>
    public interface ICaptureHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowToCapture"></param>
        /// <returns></returns>
        WindowDetails SelectCaptureWindow(WindowDetails windowToCapture);

        /// <summary>
        /// Capture the specified window
        /// </summary>
        /// <param name="windowToCapture">WindowDetails</param>
        /// <param name="capture">ICapture</param>
        /// <param name="coreConfigurationWindowCaptureMode">WindowCaptureMode</param>
        /// <returns>ICapture</returns>
        ICapture CaptureWindow(WindowDetails windowToCapture, ICapture capture, WindowCaptureMode coreConfigurationWindowCaptureMode);
    }
}
