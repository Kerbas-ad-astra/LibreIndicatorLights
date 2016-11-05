//
//  This file is part of LibreIndicatorLights.
//
//  Copyright (c) 2016 Kerbas-ad-astra
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace LibreIndicatorLights
{
    /// <summary>
    /// Used for decorating module properties that are intended to be parsed as color source IDs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColorSourceIDField : Attribute
    {
        /// <summary>
        /// Gets whether the specified field is a color source ID field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool Is(BaseField field)
        {
            return field.FieldInfo.IsDefined(typeof(ColorSourceIDField), true)
                && (typeof(string).IsAssignableFrom(field.FieldInfo.FieldType));
        }
    }
}
