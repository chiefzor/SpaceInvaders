using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public sealed class UpdateableComparer : IComparer<IUpdateable>
    {
        public static readonly UpdateableComparer Default;

        static UpdateableComparer()
        {
            Default = new UpdateableComparer();
        }

        private UpdateableComparer()
        {
        }

        public int Compare(IUpdateable x, IUpdateable y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.UpdateOrder > y.UpdateOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }
    }
}
