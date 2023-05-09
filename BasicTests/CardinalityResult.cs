using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicTests
{
    public class CardinalityResult
    {
        /*
    * Complete the 'cardinalitySort' function below.
    *
    * The function is expected to return an INTEGER_ARRAY.
    * The function accepts INTEGER_ARRAY nums as parameter.
    */

        public static List<int> cardinalitySort(List<int> nums)
        {
            return nums.OrderBy(num => countBitsOfNum(num)).ThenBy(num => num).ToList();
        }

        static int countBitsOfNum(int num)
        {
            int count = 0;
            while (num > 0)
            {
                count += num & 1;
                num >>= 1;
            }
            return count;
        }
    }
}
