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
    static class Vessels
    {
        /// <summary>
        /// Get the current biome of the ship. Returns null if none.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        public static string GetCurrentBiome(this Vessel vessel)
        {
            if (vessel == null) return null;
            double lat = ResourceUtilities.Deg2Rad((vessel.latitude + 180.0 + 90.0) % 180.0 - 90.0);
            double lon = ResourceUtilities.Deg2Rad((vessel.longitude + 360.0 + 180.0) % 360.0 - 180.0);
            CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(lat, lon, vessel.mainBody);
            return (biome == null) ? null : biome.name;
        }
    }
}
