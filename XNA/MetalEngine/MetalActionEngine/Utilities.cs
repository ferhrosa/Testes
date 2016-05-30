using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace MetalActionEngine
{
    internal class Utilities
    {
        /// <summary>
        /// Default XNA color used when its not possible to convert a string into a color.
        /// </summary>
        private static readonly Color DefaultColor = Color.Magenta;


        /// <summary>
        /// Converts a string into a XNA color.
        /// </summary>
        /// <param name="colorString">String expression to be converted into a XNA color.</param>
        /// <param name="defaultColor">Color to be return in case of an invalid string expression.</param>
        internal static Color ColorFromString(string colorString, Color defaultColor)
        {
            // Returns the default color if null or empty string is passed.
            if ( String.IsNullOrWhiteSpace(colorString) )
                return defaultColor;

            // Converts hexadecimal string into a XNA color.
            if ( colorString.Substring(0, 1) == "#" )
            {
                colorString = colorString.Replace("#", String.Empty);

                if ( !IsHexadecimalNumber(colorString) )
                    return defaultColor;

                switch ( colorString.Length )
                {
                    case 6:
                    case 8:
                        

                    default:
                        return defaultColor;
                }
            }
            else
            {
                var availableColors = typeof(Color).GetProperties();

                foreach ( var color in availableColors )
                {
                    if ( color.Name.ToLower() == colorString.ToLower() )
                    {
                        return (Color)color.GetValue(null, null);
                    }
                }
            }

            return defaultColor;
        }

        /// <summary>
        /// Converts a string into a XNA color.
        /// </summary>
        /// <param name="colorString">String expression to be converted into a XNA color.</param>
        internal static Color ColorFromString(string colorString)
        {
            return ColorFromString(colorString, DefaultColor);
        }


        /// <summary>
        /// Determines if the expression contains only hexadecimal digits.
        /// </summary>
        /// <param name="expression">Expression to be evaluated.</param>
        private static bool IsHexadecimalNumber(string expression)
        {
            return Regex.IsMatch(expression, @"\A\b[0-9a-fA-F]+\b\Z");
        }
    }
}
