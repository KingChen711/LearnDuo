using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Utils
{
    public class TeachingScheduleHelper
    {
        public static Random random = new Random();
        public static string GenerateMeetRoomCode()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            string Random3Char() => new string(new char[]
            {
                chars[random.Next(chars.Length)],
                chars[random.Next(chars.Length)],
                chars[random.Next(chars.Length)]
            });

            return $"{Random3Char()}-{Random3Char()}-{Random3Char()}";
        }

        public static string GenerateRandomWeekdays()
        {
            var weekdays = Enum.GetValues<Weekdays>().Cast<Weekdays>().ToList();
            var numberOfdays = random.Next(2, 4); // at least [2-3] days
            var selectedDays = weekdays.OrderBy(x => random.Next()).Take(numberOfdays).ToList();

            return String.Join(",", selectedDays);
        }
    }
}
