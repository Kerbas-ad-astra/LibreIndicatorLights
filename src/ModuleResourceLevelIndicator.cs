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
    /// A controller that sets the display based on the empty/full percentage of a
    /// particular resource.
    /// </summary>
    class ModuleResourceLevelIndicator : ModuleResourceIndicator, IToggle
    {
        private IColorSource highSource = null;
        private IColorSource mediumSource = null;
        private IColorSource lowSource = null;
        private IColorSource criticalSource = null;
        private IColorSource emptySource = null;

        /// <summary>
        /// The color to display when the resource is full or nearly full.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string highColor = Colors.ToString(DefaultColor.HighResource);

        /// <summary>
        /// The color to display when the resource is partially full. (Or, depending on your
        /// outlook on life, partially empty.)
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string mediumColor = Colors.ToString(DefaultColor.MediumResource);

        /// <summary>
        /// The color to display when the resource is mostly empty.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string lowColor = Colors.ToString(DefaultColor.LowResource);

        /// <summary>
        /// The color to display when the resource is almost completely empty.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string criticalColor = ColorSources.Pulsate(ColorSources.Constant(DefaultColor.LowResource), 1200, 0.6f).ColorSourceID;

        /// <summary>
        /// The color to display when the resource is completely empty.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string emptyColor = Colors.ToString(DefaultColor.Off);

        /// <summary>
        /// If resource content is above this fraction, display using the "high" color.
        /// </summary>
        [KSPField]
        public double highThreshold = 0.7;

        /// <summary>
        /// If resource content is below this fraction, display using the "low" color.
        /// </summary>
        [KSPField]
        public double lowThreshold = 0.3;

        /// <summary>
        /// If resource content is below this faction, use a pulsating animation for
        /// the light's brightness.
        /// </summary>
        [KSPField]
        public double criticalThreshold = 0.03;

        /// <summary>
        /// Called when the module is starting up.
        /// </summary>
        /// <param name="state"></param>
        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            highSource = FindColorSource(highColor);
            mediumSource = FindColorSource(mediumColor);
            lowSource = FindColorSource(lowColor);
            criticalSource = FindColorSource(criticalColor);
            emptySource = FindColorSource(emptyColor);
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
            get
            {
                if (Resource.amount == 0) return emptySource;
                double fraction = Resource.amount / Resource.maxAmount;
                if (fraction > highThreshold) return highSource;
                if (fraction < criticalThreshold) return criticalSource;
                if (fraction < lowThreshold) return lowSource;
                return mediumSource;
            }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get
            {
                return Resource.amount > 0.0;
            }
        }
    }
}
