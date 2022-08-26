using System.IO;
using System.Security.Cryptography;

namespace Kzrnm.TwitterJikkyo.Logic;
public partial record AesCrypt(byte[] Key, byte[] IV)
{
    Aes CreateAes()
    {
        var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        return aes;
    }
    public byte[] Encrypt(string input)
    {
        using var aes = CreateAes();
        using var encryptor = aes.CreateEncryptor();

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(input);
        }
        return ms.ToArray();
    }
    public string Decrypt(byte[] cipher)
    {
        using var aes = CreateAes();
        using var decryptor = aes.CreateDecryptor();

        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
