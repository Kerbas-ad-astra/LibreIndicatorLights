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
using System.Collections.Generic;
using UnityEngine;

namespace LibreIndicatorLights
{
    public abstract class VesselRegistrar<T> : MonoBehaviour
    {
        private readonly Dictionary<Guid, T> registry = new Dictionary<Guid, T>();

        /// <summary>
        /// Gets the data associated with the specified vessel, or null if not present.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        public T this[Vessel vessel]
        {
            get
            {
                if (vessel == null) return default(T);

                T data;
                if (registry.TryGetValue(vessel.id, out data))
                {
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Called when a new vessel is created.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        protected abstract T OnAddVessel(Vessel vessel);

        /// <summary>
        /// Called when a vessel we're tracking has been modified.
        /// </summary>
        /// <param name="vessel"></param>
        /// <param name="data"></param>
        protected virtual T OnModifyVessel(Vessel vessel, T data)
        {
            // Default behavior is to do nothing and return the input data.
            return data;
        }

        /// <summary>
        /// Called when a vessel we're tracking has been removed.
        /// </summary>
        /// <param name="vessel"></param>
        /// <param name="data"></param>
        protected virtual void OnRemoveVessel(Vessel vessel, T data)
        {
            // Default behavior is to do nothing.
        }

        public virtual void Awake()
        {
            GameEvents.onVesselCreate.Add(OnVesselAdded);
            GameEvents.onVesselLoaded.Add(OnVesselAdded);
            GameEvents.onVesselDestroy.Add(OnVesselRemoved);
            GameEvents.onVesselWasModified.Add(OnVesselModified);
        }

        public virtual void OnDestroy()
        {
            GameEvents.onVesselCreate.Add(OnVesselAdded);
            GameEvents.onVesselLoaded.Remove(OnVesselAdded);
            GameEvents.onVesselDestroy.Remove(OnVesselRemoved);
            GameEvents.onVesselWasModified.Remove(OnVesselModified);
        }

        /// <summary>
        /// Here whenever a vessel is loaded.
        /// </summary>
        /// <param name="vessel"></param>
        private void OnVesselAdded(Vessel vessel)
        {
            if (vessel.Parts.Count == 0) return;
            if (vessel.id == Guid.Empty)
            {
                if (Configuration.isVerbose) Logging.Log("Ignoring vessel with empty ID");
                return;
            }

            if (Configuration.isVerbose)
            {
                if (registry.ContainsKey(vessel.id))
                {
                    Logging.Log("Duplicate vessel " + Describe(vessel) + " (" + vessel.Parts.Count + " parts) added, replacing");
                }
                else
                {
                    Logging.Log("Start tracking vessel " + Describe(vessel) + " (" + vessel.Parts.Count + " parts)");
                }
            }

            T data = OnAddVessel(vessel);
            registry[vessel.id] = data;
        }

        /// <summary>
        /// Here whenever a vessel is unloaded or removed.
        /// </summary>
        /// <param name="vessel"></param>
        private void OnVesselRemoved(Vessel vessel)
        {
            if (vessel.id == Guid.Empty)
            {
                if (Configuration.isVerbose) Logging.Log("Ignoring vessel with empty ID");
                return;
            }
            T data;
            if (!registry.TryGetValue(vessel.id, out data))
            {
                if (Configuration.isVerbose) Logging.Log("Unknown vessel " + Describe(vessel) + " removed, ignoring");
                return;
            }

            if (Configuration.isVerbose) Logging.Log("Stop tracking vessel " + Describe(vessel));

            registry.Remove(vessel.id);
            OnRemoveVessel(vessel, data);
        }

        /// <summary>
        /// Here when a vessel is modified.
        /// </summary>
        /// <param name="vessel"></param>
        private void OnVesselModified(Vessel vessel)
        {
            if (vessel.id == Guid.Empty)
            {
                if (Configuration.isVerbose) Logging.Log("Ignoring vessel with empty ID");
                return;
            }
            T data;
            if (!registry.TryGetValue(vessel.id, out data))
            {
                if (Configuration.isVerbose) Logging.Log("Unknown vessel " + Describe(vessel) + " modified, adding");
                data = OnAddVessel(vessel);
                registry.Add(vessel.id, data);
                return;
            }

            if (Configuration.isVerbose) Logging.Log("Modified vessel " + Describe(vessel));

            T newData = OnModifyVessel(vessel, data);
            if (!object.ReferenceEquals(newData, data))
            {
                registry[vessel.id] = newData;
            }
        }

        private static string Describe(Vessel vessel)
        {
            return "'" + vessel.vesselName + "' (" + vessel.id + ")";
        }
    }
}
