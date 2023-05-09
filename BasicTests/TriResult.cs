using System;
using System.Collections.Generic;
using System.Text;

namespace BasicTests
{
    public class TriResult
    {
        public static int pointsBelong(int x1, int y1, int x2, int y2, int x3, int y3, int xp, int yp, int xq, int yq)
        {
            double ab = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            double bc = Math.Sqrt(Math.Pow(x3 - x2, 2) + Math.Pow(y3 - y2, 2));
            double ac = Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2));

            if (ab + bc <= ac || bc + ac <= ab || ab + ac <= bc)
            {
                // The three points do not form a valid triangle
                return 0;
            }

            bool pInside = isPointInsideTriangle(x1, y1, x2, y2, x3, y3, xp, yp);
            bool qInside = isPointInsideTriangle(x1, y1, x2, y2, x3, y3, xq, yq);

            if (pInside && !qInside)
            {
                return 1;
            }
            else if (qInside && !pInside)
            {
                return 2;
            }
            else if (pInside && qInside)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        static bool isPointInsideTriangle(int x1, int y1, int x2, int y2, int x3, int y3, int xp, int yp)
        {
            double areaABC = Math.Abs((x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)) / 2.0);
            double areaPBC = Math.Abs((xp * (y2 - y3) + x2 * (y3 - yp) + x3 * (yp - y2)) / 2.0);
            double areaPCA = Math.Abs((x1 * (yp - y3) + xp * (y3 - y1) + x3 * (y1 - yp)) / 2.0);
            double areaPAB = Math.Abs((x1 * (y2 - yp) + x2 * (yp - y1) + xp * (y1 - y2)) / 2.0);
            return areaPBC + areaPCA + areaPAB == areaABC;
        }

    }
}
