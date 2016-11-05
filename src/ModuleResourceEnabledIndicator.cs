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
using UnityEngine;

namespace LibreIndicatorLights
{
    /// <summary>
    /// A controller that sets the display based on whether a resource is enabled/disabled.
    /// </summary>
    class ModuleResourceEnabledIndicator : ModuleResourceIndicator, IToggle
    {
        private IColorSource enabledSource = null;
        private IColorSource disabledSource = null;

        /// <summary>
        /// The color to display when the resource is enabled.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string enabledColor = Colors.ToString(DefaultColor.ToggleLED.Value());

        /// <summary>
        /// The color to display when the resource is disabled.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string disabledColor = ColorSources.Blink(
            ColorSources.Constant(DefaultColor.ToggleLED),
            900,
            ColorSources.Constant(DefaultColor.Off),
            300).ColorSourceID;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            enabledSource = FindColorSource(enabledColor);
            disabledSource = FindColorSource(disabledColor);
        }

        public override bool HasColor
        {
            get
            {
                return base.HasColor && CurrentSource.HasColor;
            }
        }

        public override Color OutputColor
        {
            get { return CurrentSource.OutputColor; }
        }

        private IColorSource CurrentSource
        {
            get { return Resource.flowState ? enabledSource : disabledSource; }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get { return Resource.flowState; }
        }
    }
}
