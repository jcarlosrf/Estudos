using System;
using System.Collections.Generic;
using System.Text;

namespace BasicTests
{
    public class AlladinResult
    {
        public static int optimalPoint(List<int> magic, List<int> dist)
        {

            int n = magic.Count;
            int start = 0;
            int totalMagic = 0;
            int currentMagic = 0;

            for (int i = 0; i < n; i++)
            {
                currentMagic += magic[i] - dist[i];
                totalMagic += magic[i] - dist[i];
                if (currentMagic < 0)
                {
                    currentMagic = 0;
                    start = i + 1;
                }
            }

            return totalMagic >= 0 ? start : -1;

        }
    }
}
