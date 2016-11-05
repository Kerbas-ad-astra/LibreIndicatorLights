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
    /// A simple module that doesn't actually do anything, other than "own" a particular toggle state.
    /// </summary>
    public class ModuleIndicatorToggle : PartModule, Identifiers.IIdentifiable, IToggle
    {
        [KSPField(guiName = "Status", isPersistant = true, guiActive = true, guiActiveEditor = true), UI_Toggle(affectSymCounterparts = UI_Scene.Editor, controlEnabled = true, enabledText = "On", disabledText = "Off")]
        public bool status = false;
        private BaseField StatusField { get { return Fields["status"]; } }

        [KSPField]
        public bool guiActive = true;

        [KSPField]
        public bool guiActiveEditor = true;

        [KSPField]
        public string toggleName = null;

        [KSPField]
        public string statusLabel = "Status";

        [KSPField]
        public string statusOn = "On";

        [KSPField]
        public string statusOff = "Off";

        [KSPField]
        public string toggleAction = "Toggle";

        [KSPField]
        public string activateAction = "Activate";

        [KSPField]
        public string deactivateAction = "Deactivate";

        /// <summary>
        /// Action-group method for toggling status.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Toggle")]
        public void OnToggleAction(KSPActionParam actionParam)
        {
            status = actionParam.type != KSPActionType.Deactivate;
        }
        private BaseAction ToggleAction {  get { return Actions["OnToggleAction"]; } }

        /// <summary>
        /// Action-group method for setting status to true.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Activate")]
        public void OnActivateAction(KSPActionParam actionParam)
        {
            status = true;
        }
        private BaseAction ActivateAction { get { return Actions["OnActivateAction"]; } }

        /// <summary>
        /// Action-group method for setting status to false.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("Deactivate")]
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

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            StatusField.guiActive = guiActive;
            StatusField.guiActiveEditor = guiActiveEditor;
            StatusField.guiName = statusLabel;
            UI_Toggle toggle = StatusField.uiControlEditor as UI_Toggle;
            if (toggle != null)
            {
                toggle.enabledText = statusOn;
                toggle.disabledText = statusOff;
            }
            toggle = StatusField.uiControlFlight as UI_Toggle;
            if (toggle != null)
            {
                toggle.enabledText = statusOn;
                toggle.disabledText = statusOff;
            }
            ToggleAction.guiName = toggleAction;
            ActivateAction.guiName = activateAction;
            DeactivateAction.guiName = deactivateAction;
        }
    }
}
