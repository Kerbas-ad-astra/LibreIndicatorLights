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
    /// Base class for modules indicator controllers that depend on some other
    /// PartModule (e.g. a stock one) for their color.
    /// </summary>
    public abstract class ModuleSourceIndicator<T> : ModuleEmissiveController where T : PartModule
    {
        private T sourceModule = null;

        /// <summary>
        /// Called when the module is starting up.
        /// </summary>
        /// <param name="state"></param>
        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            sourceModule = FindFirst<T>();
            if (sourceModule == null)
            {
                Logging.Warn("No " + typeof(T).Name + " found for " + part.GetTitle());
            }
        }

        public override bool HasColor
        {
            get { return sourceModule != null; }
        }

        protected T SourceModule
        {
            get { return sourceModule; }
        }
    }
}
