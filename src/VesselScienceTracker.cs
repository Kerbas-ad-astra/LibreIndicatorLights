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

namespace LibreIndicatorLights
{
    /// <summary>
    /// Utility class for tracking which science subjects are contained in vessels.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class VesselScienceTracker : VesselRegistrar<VesselScienceContents>
    {
        private static VesselScienceTracker instance = null;

        public void Start()
        {
            instance = this;
        }

        /// <summary>
        /// Gets the science contents information for the specified vessel, or null if not found.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        public static VesselScienceContents Get(Vessel vessel)
        {
            return (instance == null) ? null : instance[vessel];
        }

        /// <summary>
        /// Here when a vessel is added.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        protected override VesselScienceContents OnAddVessel(Vessel vessel)
        {
            return new VesselScienceContents(vessel);
        }

        /// <summary>
        /// Here when a vessel is modified.
        /// </summary>
        /// <param name="vessel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override VesselScienceContents OnModifyVessel(Vessel vessel, VesselScienceContents data)
        {
            return new VesselScienceContents(vessel);
        }

        private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            throw new NotImplementedException();
        }
    }
}
