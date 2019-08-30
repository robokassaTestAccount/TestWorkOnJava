using System;
using System.Threading;

namespace WebApiCreator
{
    public static class Helpers
    {
        public static string GetRandomString(int number = 10, bool withNumber = true)
        {
            Thread.Sleep(10);
            Random random = new Random((int)DateTime.Now.Ticks);

            string rc;
            if (withNumber)
            {
                rc = "qwertyuiopasdfghjklzxcvbnm0123456789";
            }
            else
            {
                rc = "qwertyuiopasdfghjklzxcvbnm";
            }

            char[] letters = rc.ToCharArray();
            string s = "";
            for (int i = 0; i < number; i++)
            {
                s += letters[random.Next(letters.Length)].ToString();
            }

            return s;
        }
        public static string GetRandomNumber(int number = 6)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            const string rc = "0123456789";

            char[] letters = rc.ToCharArray();
            string s = "";
            for (int i = 0; i < number; i++)
            {
                s += letters[random.Next(letters.Length)].ToString();
            }

            return s;
        }
        public static string GetINN()
        {
            Random rnd = new Random();
            int a = 9;
            int b = 9;
            int c = 9;

            int z = 8;
            int v = rnd.Next(0, 9);
            int n = rnd.Next(0, 9);

            int m = rnd.Next(0, 9);
            int s = rnd.Next(0, 9);
            int d = rnd.Next(0, 9);

            int x = (a * 2) + (b * 4) + (c * 10) + (z * 3) + (v * 5) + (n * 9) + (m * 4) + (s * 6) + (d * 8);
            int das = x % 11 % 10;
            
            string result = $"{a}{b}{c}{z}{v}{n}{m}{s}{d}{das}";
            return result;
        }
        public static string GetOGRN()
        {
            Random rnd = new Random();
            int a = 1;

            int b = rnd.Next(0, 9);
            int c = rnd.Next(0, 9);

            int z = 1;
            int v = 1;

            int n = rnd.Next(0, 9);
            int m = rnd.Next(0, 9);

            int s = rnd.Next(0, 9);
            int d = rnd.Next(0, 9);
            int f = rnd.Next(0, 9);
            int g = rnd.Next(0, 9);
            int h = rnd.Next(0, 9);

            long x = long.Parse(a.ToString() + b.ToString() + c.ToString() + z.ToString() + v.ToString() + n.ToString() + m.ToString() + s.ToString() + d.ToString() + f.ToString() + g.ToString() + h.ToString());
            long das = x % 11 % 10;

            string result = $"{a}{b}{c}{z}{v}{n}{m}{s}{d}{f}{g}{h}{das}";
            return result;
        }
    }
}
