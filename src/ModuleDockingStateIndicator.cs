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
    /// Controls color based on the docking state (docked, undocked, engaged, disengaging).
    /// </summary>
    class ModuleDockingStateIndicator : ModuleSourceIndicator<ModuleDockingNode>, IToggle
    {
        private const string ACQUIRE = "Acquire";
        private const string DISENGAGE = "Disengage";

        [KSPField]
        [ColorSourceIDField]
        public string readyColor = string.Empty;

        [KSPField]
        [ColorSourceIDField]
        public string acquireColor = string.Empty;

        [KSPField]
        [ColorSourceIDField]
        public string disengageColor = string.Empty;

        private IColorSource ready = null;
        private IColorSource acquire = null;
        private IColorSource disengage = null;


        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            ready = FindColorSource(readyColor);
            acquire = FindColorSource(acquireColor);
            disengage = FindColorSource(disengageColor);
        }

        public override Color OutputColor
        {
            get
            {
                if (string.IsNullOrEmpty(SourceModule.state)) return ready.OutputColor;
                if (SourceModule.state.StartsWith(ACQUIRE)) return acquire.OutputColor;
                if (SourceModule.state.StartsWith(DISENGAGE)) return disengage.OutputColor;
                return ready.OutputColor;
            }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get
            {
                // Toggle is considered "on" when we're engaging or disengaging, off at all other times.
                return string.IsNullOrEmpty(SourceModule.state)
                    || SourceModule.state.StartsWith(ACQUIRE)
                    || SourceModule.state.StartsWith(DISENGAGE);
            }
        }
    }
}
