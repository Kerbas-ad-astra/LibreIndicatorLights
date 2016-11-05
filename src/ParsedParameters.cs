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
using System.Text.RegularExpressions;

namespace LibreIndicatorLights
{
    /// <summary>
    /// Utility class for parsing strings of the form "identifier(param1, param2, param3)", where
    /// the params themselves may contain nested parentheses & commas.
    /// </summary>
    public class ParsedParameters
    {
        private static readonly Regex PATTERN = new Regex("^([A-Za-z]+)\\((.*)\\)$");
        private readonly string identifier;
        private readonly string[] parameters;

        private ParsedParameters(string identifier, string[] parameters)
        {
            this.identifier = identifier;
            this.parameters = parameters;
        }

        public string Identifier
        {
            get { return identifier; }
        }

        public int Count
        {
            get { return parameters.Length; }
        }

        public string this[int index]
        {
            get { return parameters[index]; }
        }

        /// <summary>
        /// Attempt to extract a ParsedParameters object from the specified string. Returns
        /// null if it's not a valid format to be extracted.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ParsedParameters TryParse(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            text = text.Replace(" ", string.Empty);
            Match match = PATTERN.Match(text);
            if (!match.Success) return null;
            string identifier = match.Groups[1].Value;
            string[] parameters = TryParseParameters(match.Groups[2].Value);
            if (parameters == null) return null;
            return new ParsedParameters(identifier, parameters);
        }

        private static string[] TryParseParameters(string text)
        {
            List<string> parameters = new List<string>();
            int depth = 0;
            int prev = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                char current = text[i];
                if (current == '(')
                {
                    ++depth;
                    continue;
                }
                if (current == ')')
                {
                    if (depth < 1)
                    {
                        Logging.Warn("Mismatched parentheses in string: " + text);
                        return null;
                    }
                    --depth;
                    continue;
                }
                if ((current == ',') && (depth == 0))
                {
                    parameters.Add(text.Substring(prev, i - prev));
                    prev = i + 1;
                }
            }
            if (depth != 0)
            {
                Logging.Warn("Mismatched parentheses in string: " + text);
                return null;
            }
            if (prev < text.Length) parameters.Add(text.Substring(prev, text.Length - prev));
            return parameters.ToArray();
        }
    }
}
