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

namespace LibreIndicatorLights
{
    /// <summary>
    /// A specialized toggle for turning crew indicators on/off.
    /// </summary>
    public class ModuleCrewIndicatorToggle : PartModule, Identifiers.IIdentifiable, IToggle
    {
        [KSPField(guiName = "Crew LEDs", isPersistant = true, guiActive = true, guiActiveEditor = true),
         UI_Toggle(affectSymCounterparts = UI_Scene.Editor, controlEnabled = true, enabledText = "On", disabledText = "Off")]
        public bool status = Configuration.crewIndicatorDefaultStatus;

        [KSPField]
        public string toggleName = null;

        /// <summary>
        /// Action-group method for toggling status.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Toggle Crew LEDs")]
        public void OnToggleAction(KSPActionParam actionParam)
        {
            status = actionParam.type != KSPActionType.Deactivate;
        }
        private BaseAction ToggleAction { get { return Actions["OnToggleAction"]; } }

        /// <summary>
        /// Action-group method for setting status to true.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Activate Crew LEDs")]
        public void OnActivateAction(KSPActionParam actionParam)
        {
            status = true;
        }
        private BaseAction ActivateAction { get { return Actions["OnActivateAction"]; } }

        /// <summary>
        /// Action-group method for setting status to false.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Deactivate Crew LEDs")]
        public void OnDeactivateAction(KSPActionParam actionParam)
        {
            status = false;
        }
        private BaseAction DeactivateAction { get { return Actions["OnDeactivateAction"]; } }

        public string Identifier
        {
            get { return toggleName; }
        }

        public bool ToggleStatus
        {
            get { return status; }
        }
    }
}
