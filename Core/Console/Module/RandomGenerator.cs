namespace OpenVMSys.Core.Console.Module;

public class RandomGenerator
{
    private static int _rep;
    public static string Create()
    {
        var str = string.Empty;
        var num2 = DateTime.Now.Ticks + _rep;
        _rep++;
        Random random = new(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
        for (int i = 0; i < 20; i++)
        {
            char ch;
            var num = random.Next();
            if ((num % 2) == 0)
            {
                ch = (char)(0x30 + ((ushort)(num % 10)));
            }
            else
            {
                ch = (char)(0x41 + ((ushort)(num % 0x1a)));
            }
            str += ch.ToString();
        }

        return str;
    }
}