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
    /// Indicates whether the current biome can be surface-scanned or not.
    /// </summary>
    class ModuleBiomeScannerIndicator : ModuleSourceIndicator<ModuleBiomeScanner>, IToggle
    {
        private IColorSource readySource;
        private IColorSource inactiveSource;

        /// <summary>
        /// The color to display when it's ready to take a biome scan.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string readyColor = string.Empty;

        /// <summary>
        /// The color to display when it's inactive (either this biome has already been
        /// scanned, or else it's not in a usable situation).
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string inactiveColor = Colors.ToString(DefaultColor.Off);

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            readySource = FindColorSource(readyColor);
            inactiveSource = FindColorSource(inactiveColor);
        }

        public override bool HasColor
        {
            get
            {
                return CurrentSource.HasColor;
            }
        }

        public override Color OutputColor
        {
            get
            {
                return CurrentSource.OutputColor;
            }
        }

        private IColorSource CurrentSource
        {
            get
            {
                return ToggleStatus ? readySource : inactiveSource;
            }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get
            {
                // If we're in the editor, or not landed, then it's inactive.
                if (!HighLogic.LoadedSceneIsFlight
                    || (vessel == null)
                    || !vessel.LandedOrSplashed)
                    return false;

                // Figure out what biome we're in
                string biomeName = vessel.GetCurrentBiome() ?? vessel.situation.ToString();
                return !ResourceMap.Instance.IsBiomeUnlocked(vessel.mainBody.flightGlobalsIndex, biomeName);
            }
        }
    }
}
