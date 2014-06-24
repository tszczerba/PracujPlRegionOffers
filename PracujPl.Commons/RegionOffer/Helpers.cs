using System;
using System.Collections.Generic;
using System.Linq;
using PracujPl.Commons.Database;

namespace PracujPl.Commons.RegionOffer
{
    public static class Helpers
    {
        /// <summary>
        /// Small helper method to capitalize first letter.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string str)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException("Empty string");
            return str.First().ToString().ToUpper() + String.Join("", str.Skip(1));
        }

    }
}