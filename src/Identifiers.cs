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

using System.Collections.Generic;

namespace LibreIndicatorLights
{
    public static class Identifiers
    {
        /// <summary>
        /// Interface for objects that can be found via identifiers.
        /// </summary>
        public interface IIdentifiable
        {
            /// <summary>
            /// Gets the identifier string used to locate this object.
            /// </summary>
            string Identifier { get; }
        }

        /// <summary>
        /// Tries to find the module of the specified type that has the specified identifier.
        /// Returns null if not found.
        /// </summary>
        /// <typeparam name="T">The module type to search for.</typeparam>
        /// <param name="part">The part to examine.</param>
        /// <param name="identifier">The identifier to look for. If null or empty, the first module of the specified type will be returned.</param>
        /// <returns></returns>
        public static T FindFirst<T>(Part part, string identifier) where T : class
        {
            if (part == null) return null;
            bool findFirst = string.IsNullOrEmpty(identifier);
            for (int i = 0; i < part.Modules.Count; ++i)
            {
                T candidate = part.Modules[i] as T;
                if (candidate == null) continue;
                IIdentifiable identifiable = candidate as IIdentifiable;
                if (identifiable == null) return null;
                if (findFirst) return candidate;
                if (identifier.Equals(identifiable.Identifier)) return candidate;
            }
            return null;
        }

        /// <summary>
        /// Tries to find all the modules of the specified type that have the specified identifier.
        /// Returns null if not found.
        /// </summary>
        /// <typeparam name="T">The module type to search for.</typeparam>
        /// <param name="part">The part to examine.</param>
        /// <param name="identifier">The identifier to look for. If null or empty, all modules of the specified type will be found.</param>
        /// <returns></returns>
        public static List<T> FindAll<T>(Part part, string identifier) where T : class
        {
            if (part == null) return null;
            bool findAll = string.IsNullOrEmpty(identifier);
            List<T> items = new List<T>();
            for (int i = 0; i < part.Modules.Count; ++i)
            {
                T candidate = part.Modules[i] as T;
                if (candidate == null) continue;
                IIdentifiable identifiable = candidate as IIdentifiable;
                if (identifiable == null) continue;
                if (findAll || identifier.Equals(identifiable.Identifier))
                {
                    // got a match!
                    items.Add(candidate);
                }
            }
            if (items.Count > 0) return items;
            Logging.Warn("No " + typeof(T).Name + " named '" + identifier + "' found for " + part.GetTitle());
            return null;
        }
    }
}
