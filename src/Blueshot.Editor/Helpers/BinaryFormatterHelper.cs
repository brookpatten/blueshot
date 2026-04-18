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
using System.Runtime.Serialization;
using System.ServiceModel.Security;
using blueshot.Base.Interfaces.Drawing;
using blueshot.Editor.Drawing;
using blueshot.Editor.Drawing.Emoji;
using blueshot.Editor.Drawing.Fields;
using blueshot.Editor.Drawing.Filters;
using log4net;
using static blueshot.Editor.Drawing.FilterContainer;

namespace blueshot.Editor.Helpers
{
    /// <summary>
    /// This helps to map the serialization of the old .blueshot file to the newer.
    /// It also prevents misuse.
    /// </summary>
    internal class BinaryFormatterHelper : SerializationBinder
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(BinaryFormatterHelper));
        private static readonly IDictionary<string, Type> TypeMapper = new Dictionary<string, Type>
        {
            {"System.Guid",typeof(Guid) },
            {"System.Drawing.Rectangle",typeof(System.Drawing.Rectangle) },
            {"System.Drawing.Point",typeof(System.Drawing.Point) },
            {"System.Drawing.Color",typeof(System.Drawing.Color) },
            {"System.Drawing.Bitmap",typeof(System.Drawing.Bitmap) },
            {"System.Drawing.Icon",typeof(System.Drawing.Icon) },
            {"System.Drawing.Size",typeof(System.Drawing.Size) },
            {"System.IO.MemoryStream",typeof(System.IO.MemoryStream) },
            {"System.Drawing.StringAlignment",typeof(System.Drawing.StringAlignment) },
            {"System.Collections.Generic.List`1[[blueshot.Base.Interfaces.Drawing.IFieldHolder", typeof(List<IFieldHolder>)},
            {"System.Collections.Generic.List`1[[blueshot.Base.Interfaces.Drawing.IField", typeof(List<IField>)},
            {"System.Collections.Generic.List`1[[System.Drawing.Point", typeof(List<System.Drawing.Point>)},
            {"blueshot.Editor.Drawing.ArrowContainer", typeof(ArrowContainer) },
            {"blueshot.Editor.Drawing.ArrowContainer+ArrowHeadCombination", typeof(ArrowContainer.ArrowHeadCombination) },
            {"blueshot.Editor.Drawing.LineContainer", typeof(LineContainer) },
            {"blueshot.Editor.Drawing.TextContainer", typeof(TextContainer) },
            {"blueshot.Editor.Drawing.SpeechbubbleContainer", typeof(SpeechbubbleContainer) },
            {"blueshot.Editor.Drawing.RectangleContainer", typeof(RectangleContainer) },
            {"blueshot.Editor.Drawing.EllipseContainer", typeof(EllipseContainer) },
            {"blueshot.Editor.Drawing.FreehandContainer", typeof(FreehandContainer) },
            {"blueshot.Editor.Drawing.HighlightContainer", typeof(HighlightContainer) },
            {"blueshot.Editor.Drawing.IconContainer", typeof(IconContainer) },
            {"blueshot.Editor.Drawing.ObfuscateContainer", typeof(ObfuscateContainer) },
            {"blueshot.Editor.Drawing.StepLabelContainer", typeof(StepLabelContainer) },
            {"blueshot.Editor.Drawing.SvgContainer", typeof(SvgContainer) },
            {"blueshot.Editor.Drawing.Emoji.EmojiContainer", typeof(EmojiContainer) },
            {"blueshot.Editor.Drawing.VectorGraphicsContainer", typeof(VectorGraphicsContainer) },
            {"blueshot.Editor.Drawing.MetafileContainer", typeof(MetafileContainer) },
            {"blueshot.Editor.Drawing.ImageContainer", typeof(ImageContainer) },
            {"blueshot.Editor.Drawing.FilterContainer", typeof(FilterContainer) },
            {"blueshot.Editor.Drawing.DrawableContainer", typeof(DrawableContainer) },
            {"blueshot.Editor.Drawing.DrawableContainerList", typeof(DrawableContainerList) },
            {"blueshot.Editor.Drawing.CursorContainer", typeof(CursorContainer) },
            {"blueshot.Editor.Drawing.CursorContainer+CaptureCursorSerializationWrapper", typeof(CursorContainer.CaptureCursorSerializationWrapper) },
            {"blueshot.Editor.Drawing.Filters.HighlightFilter", typeof(HighlightFilter) },
            {"blueshot.Editor.Drawing.Filters.GrayscaleFilter", typeof(GrayscaleFilter) },
            {"blueshot.Editor.Drawing.Filters.MagnifierFilter", typeof(MagnifierFilter) },
            {"blueshot.Editor.Drawing.Filters.BrightnessFilter", typeof(BrightnessFilter) },
            {"blueshot.Editor.Drawing.Filters.BlurFilter", typeof(BlurFilter) },
            {"blueshot.Editor.Drawing.Filters.PixelizationFilter", typeof(PixelizationFilter) },
            {"blueshot.Base.Interfaces.Drawing.IDrawableContainer", typeof(IDrawableContainer) },
            {"blueshot.Base.Interfaces.Drawing.EditStatus", typeof(EditStatus) },
            {"blueshot.Base.Interfaces.Drawing.IFieldHolder", typeof(IFieldHolder) },
            {"blueshot.Base.Interfaces.Drawing.IField", typeof(IField) },
            {"blueshot.Base.Interfaces.Drawing.FieldFlag", typeof(FieldFlag) },
            {"blueshot.Editor.Drawing.Fields.Field", typeof(Field) },
            {"blueshot.Editor.Drawing.Fields.FieldType", typeof(FieldType) },
            {"blueshot.Editor.Drawing.FilterContainer+PreparedFilter", typeof(PreparedFilter) },
        };

        /// <summary>
        /// Do the type mapping
        /// </summary>
        /// <param name="assemblyName">Assembly for the type that was serialized</param>
        /// <param name="typeName">Type that was serialized</param>
        /// <returns>Type which was mapped</returns>
        /// <exception cref="SecurityAccessDeniedException">If something smells fishy</exception>
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            var typeNameCommaLocation = typeName.IndexOf(",");
            var comparingTypeName = typeName.Substring(0, typeNameCommaLocation > 0 ? typeNameCommaLocation : typeName.Length);

            // Correct wrong types
            comparingTypeName = comparingTypeName.Replace("blueshot.Drawing", "blueshot.Editor.Drawing");
            comparingTypeName = comparingTypeName.Replace("blueshot.Plugin.Drawing", "blueshot.Base.Interfaces.Drawing");
            comparingTypeName = comparingTypeName.Replace("blueshotPlugin.Interfaces.Drawing", "blueshot.Base.Interfaces.Drawing");
            comparingTypeName = comparingTypeName.Replace("blueshot.Drawing.Fields", "blueshot.Editor.Drawing.Fields");
            comparingTypeName = comparingTypeName.Replace("blueshot.Drawing.Filters", "blueshot.Editor.Drawing.Filters");

            if (TypeMapper.TryGetValue(comparingTypeName, out var returnType))
            {
                LOG.Info($"Mapped {assemblyName} - {typeName} to {returnType.FullName}");
                return returnType;
            }
            LOG.Warn($"Unexpected blueshot type in .blueshot file detected, maybe vulnerability attack created with ysoserial? Suspicious type: {assemblyName} - {typeName}");
            throw new SecurityAccessDeniedException($"Suspicious type in .blueshot file: {assemblyName} - {typeName}");
        }
    }
}
