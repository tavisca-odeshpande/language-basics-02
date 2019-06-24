using System;

namespace Tavisca.Bootcamp.LanguageBasics.Exercise1
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Test(new[] {"12:12:12"}, new [] { "few seconds ago" }, "12:12:12");
            Test(new[] { "23:23:23", "23:23:23" }, new[] { "59 minutes ago", "59 minutes ago" }, "00:22:23");
            Test(new[] { "00:10:10", "00:10:10" }, new[] { "59 minutes ago", "1 hours ago" }, "impossible");
            Test(new[] { "11:59:13", "11:13:23", "12:25:15" }, new[] { "few seconds ago", "46 minutes ago", "23 hours ago" }, "11:59:23");
            Console.ReadKey(true);
        }

        private static void Test(string[] postTimes, string[] showTimes, string expected)
        {
            var result = GetCurrentTime(postTimes, showTimes).Equals(expected) ? "PASS" : "FAIL";
            var postTimesCsv = string.Join(", ", postTimes);
            var showTimesCsv = string.Join(", ", showTimes);
            Console.WriteLine($"[{postTimesCsv}], [{showTimesCsv}] => {result}");
        }

        public static string GetCurrentTime(string[] exactPostTime, string[] showPostTime)
        {
            int len = exactPostTime.Length;
            int[,] times = new int[len,2];
            string result = "";
            for(int i=0;i<len;i++)
            {
                int exact_seconds = ToSeconds(exactPostTime[i]);
                int[] show_seconds = SecondsElapsed(showPostTime[i]);
                times[i,0] = exact_seconds + show_seconds[0];
                times[i,0] = times[i,0] % 86400;
                times[i,1] = exact_seconds + show_seconds[1];
                times[i,1] = times[i,1] % 86400;
            }
            result = GetFinalDate(times, len);
            Console.WriteLine(result);
            return result;
        }

        public static int ToSeconds(string time)
        {
            String[] times = time.Split(":");
            int seconds = 0;
            seconds = int.Parse(times[0]) * 3600 + int.Parse(times[1]) * 60 + int.Parse(times[2]);
            return seconds;
        }

        public static string ToDate(int seconds)
        {
            String date = "";
            string hr = ((seconds / 3600)%24).ToString();
            if (hr.Length < 2) { hr = "0" + hr; };
            seconds = seconds % 3600;
            string min = date + (seconds / 60).ToString();
            if (min.Length < 2) { min = "0" + min; };
            seconds = seconds % 60;
            string sec = date + seconds.ToString();
            if (sec.Length < 2) { sec = "0" + sec; };
            date = hr + ":" + min + ":" + sec;
            return date;
        }

        public static int[] SecondsElapsed(string showtime)
        {
            string[] showtimearr = showtime.Split(' ');
            int[] seconds = new int[2];
            if (showtimearr[1] == "seconds")
            {
                seconds[0] = 0;
                seconds[1] = 59;
            }
            else if(showtimearr[1] == "minutes")
            {
                seconds[0] = int.Parse(showtimearr[0]) * 60;
                seconds[1] = seconds[0] + 59;
            }
            else
            {
                seconds[0] = int.Parse(showtimearr[0]) * 3600;
                seconds[1] = seconds[0] + 3599;
            }
            return seconds;
        }

        public static string GetFinalDate(int[,] times,int len)
        {
            int min= times[0, 0], max= times[0, 1];
            for(int i=0;i<len;i++)
            {
                if (times[i, 0] <= max)
                {
                    if(times[i, 0] > min)
                    {
                        min = times[i, 0];
                    }
                }
                else
                {
                    return "impossible";
                }
                if(times[i, 1] >= min)
                {
                    if(times[i, 1] < max)
                    {
                        max = times[i, 1];
                    }
                }
                else
                {
                    return "impossible";
                }
            }
            return ToDate(min);
        }
    }
}
