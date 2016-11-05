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

using UnityEngine;

namespace LibreIndicatorLights
{
    /// <summary>
    /// Interface for objects that can provide a color.
    /// </summary>
    public interface IColorSource
    {
        /// <summary>
        /// Whether a color is currently available.
        /// </summary>
        bool HasColor { get; }

        /// <summary>
        /// Current color.  Do not call this unless HasColor returns true.
        /// </summary>
        Color OutputColor { get; }

        /// <summary>
        /// Gets the ID of this source.
        /// </summary>
        string ColorSourceID { get; }
    }
}
