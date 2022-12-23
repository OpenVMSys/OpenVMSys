using System.Text;
using OpenSDK;
using Path = OpenSDK.Path;

namespace OpenVMSys.Core.Console.Module.Security;

internal class Service
{
    //private static readonly List<SecurityKey> Keys = new();
    private static readonly string KeyLocation = Path.Join("Keys");

    private static List<SecurityKey> Refresh()
    {
        var securityKeys = new List<SecurityKey>();
        try
        {
            //声明读取流
            var getStream = new FileStream(KeyLocation, FileMode.Open);
            var getReader = new StreamReader(getStream, Encoding.UTF8);
            //清空密钥列表
            securityKeys.Clear();
            var keys = getReader.ReadToEnd().Split("\n");
            securityKeys.AddRange(from lineObject in keys where !lineObject.Contains("#") where lineObject.Split("\t").Length == 3 select lineObject.Split("\t") into keyPair select new SecurityKey { Key = keyPair[0], Permission = int.Parse(keyPair[1]), Ident = keyPair[2] });
            return securityKeys;
        }
        catch (Exception ex)
        {
            OpenSDK.Logger<Service>.Error("SecurityKey",ex.Message);
            return securityKeys;
        }
    }

    public static bool VerifyKey(string authKey, int permission)
    {
        var keys = Refresh();
        return keys
            .Select(key => key.Key != null && key.Key.Equals(authKey) && permission.CompareTo(key.Permission) >= 0)
            .FirstOrDefault();
    }

    public static void Get()
    {
        System.Console.WriteLine("Key Ident \t Permission \t Value");
        foreach (var securityKey in Refresh())
        {
            System.Console.WriteLine("{0}\t{1}\t{2}", securityKey.Ident, securityKey.Permission, securityKey.Key);
        }
    }
    public static bool Create(string keyValue, int keyPermission, string ident)
    {
        try
        {
            var keys = Refresh();
            keys.Insert(keys.Count,new SecurityKey
            {
                Key = keyValue,
                Permission = keyPermission,
                Ident = ident
            });
            var keyStream = new FileStream(KeyLocation, FileMode.Append);
            var keyWriter = new StreamWriter(keyStream);
            keyWriter.WriteLine(keyValue + "\t" + keyPermission + "\t" + ident);
            keyStream.Flush();
            keyWriter.Flush();
            keyWriter.Close();
            keyStream.Close();
            OpenSDK.Logger<Service>.Result("CLI","key added");
            return true;
        }
        catch (Exception e)
        {
            OpenSDK.Logger<Service>.Error("SecurityKey",e.Message);
            return false;
        }
    }

    public static void Delete(string ident)
    {
        var keys = Refresh();
        var findResult = keys.Find(key => key.Ident != null && key.Ident.Equals(ident));
        if (findResult!=null)
        {
            keys.Remove(findResult);
        }
        //应用更新的密钥列表
        var keyStream = new FileStream(KeyLocation, FileMode.CreateNew);
        var keyWriter = new StreamWriter(keyStream);
        foreach (var key in keys)
        {
            keyWriter.WriteLine(key.Key + "\t" + key.Permission + "\t" + key.Ident);
        }
        OpenSDK.Logger<Service>.Result("SecurityKey","Operation Complete,",keys.Count.ToString(),"Left");
        keyWriter.Flush();
        keyStream.Flush();
        keyWriter.Close();
        keyStream.Close();
    }

    public static string Gen(int permission, string ident)
    {
        var value = RandomGenerator.Gen();
        var result = Create(value, permission, ident);
        if (!result)
        {
            return "";
        }
        OpenSDK.Logger<Service>.Result("SecurityKey", "Key", value, "added to db");
        return value;
    }
}