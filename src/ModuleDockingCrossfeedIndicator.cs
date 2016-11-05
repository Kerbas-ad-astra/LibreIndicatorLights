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
    /// Picks a solid color, based on crossfeed enabled/disabled status.
    /// </summary>
    public class ModuleDockingCrossfeedIndicator : ModuleSourceIndicator<ModuleDockingNode>, IToggle
    {
        [KSPField]
        [ColorSourceIDField]
        public string crossfeedOnSource = Colors.ToString(DefaultColor.DockingCrossfeedOn);

        [KSPField]
        [ColorSourceIDField]
        public string crossfeedOffSource = Colors.ToString(DefaultColor.DockingCrossfeedOff);

        private IColorSource onSource = null;
        private IColorSource offSource = null;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            onSource = FindColorSource(crossfeedOnSource);
            offSource = FindColorSource(crossfeedOffSource);
        }

        public override Color OutputColor
        {
            get
            {
                return SourceModule.crossfeed ? onSource.OutputColor : offSource.OutputColor;
            }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get
            {
                return SourceModule.crossfeed;
            }
        }
    }
}
