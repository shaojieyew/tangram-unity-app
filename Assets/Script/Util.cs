using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    class Util
    {
        public static double getDistance(double x1, double y1, double x2, double y2)
        {
            double xdiff = Math.Abs(x1 - x2);
            double ydiff = Math.Abs(y1 - y2);
            return Math.Sqrt((xdiff * xdiff) + (ydiff * ydiff));
        }
    }
}
