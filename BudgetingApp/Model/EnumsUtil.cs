using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingApp.Model
{
    public static class EnumsUtil
    {
        /// <summary>
        /// Convert an Enumerated type into an enumerable of all its values
        /// </summary>
        /// <typeparam name="TEnum">An Enum type to be converted to a list</typeparam>
        /// <returns>An enumerable that contains a list of all values of the enum</returns>
        /// <exception cref="ArgumentException">The type provided was not an Enum Type</exception>
        public static IEnumerable<TEnum> ToEnumerable<TEnum>() where TEnum : struct, IConvertible  // Enum type is a struct that implements the IConvertible interface. Help the future developer out by providing this restriction
        {

            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be of type System.Enum");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static IList<TEnum> ToList<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be of type System.Enum");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }

        /// <summary>
        /// Get the Component Model description of the Enum value provided
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }
    }
}
