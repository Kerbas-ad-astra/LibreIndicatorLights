﻿//
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
    /// A module that sets the display based on the current status of a reaction wheel.
    /// </summary>
    class ModuleReactionWheelIndicator : ModuleSourceIndicator<ModuleReactionWheel>
    {
        private static readonly Animations.Blink BLINK = Animations.Blink.of(250, 250, 0);

        private StartState startState = StartState.None;
        private IColorSource problemSource = null;
        private IColorSource normalSource = null;
        private IColorSource pilotOnlySource = null;
        private IColorSource sasOnlySource = null;

        /// <summary>
        /// The color to display when the reaction wheel has a problem.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string problemColor = Colors.ToString(DefaultColor.Warning);

        /// <summary>
        /// The color to display when the reaction wheel is operating in "normal" mode.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string normalColor = Colors.ToString(DefaultColor.ReactionWheelNormal);

        /// <summary>
        /// The color to display when the reaction wheel is operating in "pilot only" mode.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string pilotOnlyColor = Colors.ToString(DefaultColor.ReactionWheelPilotOnly);

        /// <summary>
        /// The color to display when the reaction wheel is operating in "SAS only" mode.
        /// </summary>
        [KSPField]
        [ColorSourceIDField]
        public string sasOnlyColor = Colors.ToString(DefaultColor.ReactionWheelSASOnly);


        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            this.startState = state;
            this.problemSource = FindColorSource(problemColor);
            this.normalSource = FindColorSource(normalColor);
            this.pilotOnlySource = FindColorSource(pilotOnlyColor);
            this.sasOnlySource = FindColorSource(sasOnlyColor);
        }

        public override bool HasColor
        {
            get { return base.HasColor && CurrentSource.HasColor; }
        }

        public override Color OutputColor
        {
            get
            {
                Color baseColor = CurrentSource.OutputColor;
                if (IsDeprived) return BLINK.State ? baseColor : DefaultColor.Off.Value();
                return (IsAutopilotActive) ? baseColor : (0.5f * baseColor);
            }
        }

        /// <summary>
        /// Gets whether SAS is turned on. Returns true if we're in a situation where it's
        /// irrelevant (e.g. in the vehicle editor).
        /// </summary>
        private bool IsAutopilotActive
        {
            get
            {
                if (startState == StartState.Editor) return true;
                Vessel vessel = FlightGlobals.ActiveVessel;
                if (vessel == null) return true;
                return vessel.Autopilot.Enabled;
            }
        }

        private bool IsDeprived
        {
            get
            {
                if (startState == StartState.Editor) return false;
                if ((SourceModule == null) ) return false;
				if (SourceModule.operational) return true;
                return false;
            }
        }

        private IColorSource CurrentSource
        {
            get
            {
                switch (SourceModule.State)
                {
                    case ModuleReactionWheel.WheelState.Disabled:
                        return ColorSources.BLACK;
                    case ModuleReactionWheel.WheelState.Broken:
                        return problemSource;
                    default:
                        break;
                }
                switch ((VesselActuatorMode)SourceModule.actuatorModeCycle)
                {
                    case VesselActuatorMode.Pilot:
                        return pilotOnlySource;
                    case VesselActuatorMode.SAS:
                        return sasOnlySource;
                    default:
                        return normalSource;
                }
            }
        }
    }
}
