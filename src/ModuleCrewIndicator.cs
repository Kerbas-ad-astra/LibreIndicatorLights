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
    /// Controls color based on the occupancy of a seat in a crewable part.
    /// </summary>
    class ModuleCrewIndicator : ModuleEmissiveController, IToggle
    {
        private const int NO_SLOT = -1;

        private IColorSource emptySource = null;
        private IColorSource pilotSource = null;
        private IColorSource engineerSource = null;
        private IColorSource scientistSource = null;
        private IColorSource touristSource = null;

        [KSPField(isPersistant = true)]
        public int slot = NO_SLOT;

        [KSPField]
        public string toggleName = null;

        [KSPField]
        [ColorSourceIDField]
        public string emptyColor = Colors.ToString(DefaultColor.Off);

        [KSPField]
        [ColorSourceIDField]
        public string pilotColor = Colors.ToString(DefaultColor.CrewPilot);

        [KSPField]
        [ColorSourceIDField]
        public string engineerColor = Colors.ToString(DefaultColor.CrewEngineer);

        [KSPField]
        [ColorSourceIDField]
        public string scientistColor = Colors.ToString(DefaultColor.CrewScientist);

        [KSPField]
        [ColorSourceIDField]
        public string touristColor = Colors.ToString(DefaultColor.CrewTourist);

        private IToggle toggle = null;

        /// <summary>
        /// Here when we're starting up.
        /// </summary>
        /// <param name="state"></param>
        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            toggle = Identifiers.FindFirst<IToggle>(part, toggleName);

            emptySource = FindColorSource(emptyColor);
            pilotSource = FindColorSource(pilotColor);
            engineerSource = FindColorSource(engineerColor);
            scientistSource = FindColorSource(scientistColor);
            touristSource = FindColorSource(touristColor);

            // The default value for slot is NO_SLOT. When we start up, we scan for all ModuleCrewIndicators
            // on the part, and assign them sequentially to slots, if available.
            if (part == null) return;
            if (part.CrewCapacity < 1) return;
            // First, go through and note which slots already have a ModuleCrewIndicator assigned to them.
            bool[] slotAssignments = new bool[part.CrewCapacity];
            for (int moduleIndex = 0; moduleIndex < part.Modules.Count; ++moduleIndex)
            {
                ModuleCrewIndicator indicator = part.Modules[moduleIndex] as ModuleCrewIndicator;
                if (indicator == null) continue;
                if ((indicator.slot < 0) || (indicator.slot >= part.CrewCapacity))
                {
                    indicator.slot = NO_SLOT;
                }
                else
                {
                    slotAssignments[indicator.slot] = true;
                }
            }
            // Next, go through and assign any unassigned ModuleCrewIndicators to any open slots.
            int slotIndex = 0;
            for (int moduleIndex = 0; moduleIndex < part.Modules.Count; ++moduleIndex)
            {
                ModuleCrewIndicator indicator = part.Modules[moduleIndex] as ModuleCrewIndicator;
                if (indicator == null) continue;
                if (indicator.slot != NO_SLOT) continue; // explicitly specifies a slot
                while (slotAssignments[slotIndex])
                {
                    ++slotIndex;
                    if (slotIndex >= part.CrewCapacity) return;
                }
                indicator.slot = slotIndex;
                slotAssignments[slotIndex] = true;
            }
        }

        public override bool HasColor
        {
            get
            {
                return (slot >= 0) && (part != null) && (slot < part.CrewCapacity) && CurrentSource.HasColor;
            }
        }

        public override Color OutputColor
        {
            get
            {
                if ((toggle != null) && (!toggle.ToggleStatus)) return DefaultColor.Off.Value();
                return CurrentSource.OutputColor;
            }
        }

        /// <summary>
        /// Gets the crew member currently driving this indicator (null if the seat is empty).
        /// </summary>
        public ProtoCrewMember Crew
        {
            get
            {
                if ((part == null)
                    || (part.protoModuleCrew == null)
                    || (slot < 0)
                    || (slot >= part.protoModuleCrew.Count))
                {
                    return null;
                }
                else
                {
                    return part.protoModuleCrew[slot];
                }
            }
        }

        private IColorSource CurrentSource
        {
            get
            {
                ProtoCrewMember crew = Crew;
                if (crew == null) return emptySource;
                if ((crew.type == ProtoCrewMember.KerbalType.Tourist)
                    || (crew.experienceTrait == null))
                {
                    return touristSource;
                }
                string title = crew.experienceTrait.Title;
                switch (title)
                {
                    case "Pilot":
                        return pilotSource;
                    case "Engineer":
                        return engineerSource;
                    case "Scientist":
                        return scientistSource;
                    default:
                        // Should never happen, but put this as a placeholder so we'll know if it does.
                        return ColorSources.ERROR;
                }
            }
        }

        /// <summary>
        /// IToggle implementation.
        /// </summary>
        public bool ToggleStatus
        {
            get
            {
                if ((toggle != null) && (!toggle.ToggleStatus)) return false;
                return Crew != null;
            }
        }
    }
}
