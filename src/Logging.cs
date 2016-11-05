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
    /// Utility wrapper for logging messages.
    /// </summary>
    static class Logging
    {
        public static void Log(object message)
        {
            Debug.Log("[IndicatorLights] " + message);
        }

        public static void Warn(object message)
        {
            Debug.LogWarning("[IndicatorLights] " + message);
        }

        public static void Error(object message)
        {
            Debug.LogError("[IndicatorLights] " + message);
        }

        public static void Exception(string message, Exception e)
        {
            Error(message + " (" + e.GetType().Name + ") " + e.Message + ": " + e.StackTrace);
        }

        public static void Exception(Exception e)
        {
            Error("(" + e.GetType().Name + ") " + e.Message + ": " + e.StackTrace);
        }

        public static string GetTitle(this Part part)
        {
            return (part == null) ? "<null part>" : ((part.partInfo == null) ? part.partName : part.partInfo.title);
        }

        /// <summary>
        /// Useful for debugging per-update-frame events without spamming the log to uselessness.
        /// </summary>
        public class Throttled
        {
            private readonly string label;
            private readonly TimeSpan cooldown;
            private DateTime nextLog;

            public Throttled(string label, long milliseconds)
            {
                this.label = label;
                this.cooldown = TimeSpan.FromMilliseconds(milliseconds);
                nextLog = DateTime.MinValue;
            }

            public bool Log(object message)
            {
                DateTime now = DateTime.Now;
                if (now < nextLog) return false;
                nextLog = now + cooldown;
                if (string.IsNullOrEmpty(label))
                {
                    Logging.Log(message);
                }
                else
                {
                    Logging.Log(label + ": " + message);
                }
                return true;
            }
        }
    }
}