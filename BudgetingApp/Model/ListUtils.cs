using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingApp.Model
{
    public static class ListUtils
    {
        public static T PassThrough<T>(T param1)
        {
            return param1;
        }

        public static void MatchListChanges<T>(IList<T> toChange, IEnumerable<T> toMatch)
        {
            MatchListChanges<T, T>(toChange, toMatch, PassThrough, PassThrough);
        }

        public static void MatchListChanges<T1, T2>(IList<T1> toChange, IEnumerable<T2> toMatch, Func<T2, T1> convertToAdd, Func<T1, T2> convertToCheck)
        {
            int index = 0;

            List<T1> toRemove = new List<T1>();
            foreach (var checkRemove in toChange)
            {
                T2 check = convertToCheck(checkRemove);
                if (!toMatch.Contains(check)) toRemove.Add(checkRemove);
            }

            foreach (var remove in toRemove)
            {
                toChange.Remove(remove);
            }


            foreach (var t in toMatch)
            {
                int location = toChange.Select(c => convertToCheck(c)).ToList().IndexOf(t);

                T1 tConverted = convertToAdd(t);
                if (location == -1)
                {
                    toChange.Insert(index, tConverted);
                }
                else if (location != index)
                {
                    toChange.RemoveAt(location);
                    toChange.Insert(index, tConverted);
                }

                index++;
            }
        }

        public static ObservableCollection<T1> WrapEnumerable<T1, T2>(IEnumerable<T2> toMatch, Func<T2, T1> convertToAdd)
        {
            return new ObservableCollection<T1>(toMatch.Select(t => convertToAdd(t)));
        }

    }
}
