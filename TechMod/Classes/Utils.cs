using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Classes
{
    public static class Utils
    {
        public static int BarLength = 27;
        public static string GetProgressBar(DateTime start, DateTime end, DateTime current, string replaceFill=":green_square:")
        {
            if (current < start)
            {
                current = start;
            }

            if (current > end)
            {
                current = end;
            }

            double progress = (current - start).TotalDays / (end - start).TotalDays;
            int progressBarLength = BarLength;
            int completedChars = (int)(progress * progressBarLength);

            string progressBar = new string('X', completedChars) + new string('-', progressBarLength - completedChars);

            return progressBar.Replace("X", replaceFill).Replace("-", ":black_large_square:");
        }
    }
}
