using System.Runtime.Intrinsics.Arm;

namespace apimoney.services
{
    public class GlobalFunction
    {
        public static string ConvertToUnixTimestamp(DateTime date)
        {
            Random rdn = new Random();
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            var a = rdn.Next(100000, 999999);
            var b = rdn.Next(10, 99);
            string t = Math.Floor(diff.TotalSeconds).ToString();
            string par1 = t.Substring(0, 5);
            int i = t.Length - 5;
            string par2 = t.Substring(5, i);
            string concate = par1 + "" + b + "" + par2 + a;
            if (concate.Length > 18)
            {
                concate = concate.Substring(0, 18);
            }
            return concate;
        }
    }
}
