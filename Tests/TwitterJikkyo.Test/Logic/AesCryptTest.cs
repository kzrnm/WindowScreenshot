namespace Kzrnm.TwitterJikkyo.Logic;
public class AesCryptTest
{
    [Fact]
    public void EncryptDecrypt()
    {
        {
            var aesCrypt = new AesCrypt(new byte[32], new byte[16]);
            var cipher = aesCrypt.Encrypt("文字列");
            cipher.Should().Equal(new byte[] { 0xDD, 0x8B, 0xE9, 0xEC, 0xF2, 0xFE, 0x96, 0x9F, 0x4F, 0x45, 0x3F, 0x27, 0x15, 0xE9, 0x16, 0x12 });
            aesCrypt.Decrypt(cipher).Should().Be("文字列");
        }
        {
            var bytes = new byte[150];
            Random.Shared.NextBytes(bytes);
            Random.Shared.NextBytes(bytes);
            var aesCrypt = new AesCrypt(
                new byte[32] { 40, 218, 78, 74, 111, 175, 191, 67, 191, 32, 250, 56, 0, 154, 199, 47, 214, 215, 75, 73, 180, 182, 80, 90, 49, 53, 164, 181, 236, 33, 220, 102, },
                new byte[16] { 186, 24, 170, 51, 245, 145, 103, 127, 51, 178, 203, 98, 87, 200, 35, 91 }
            );
            var cipher = aesCrypt.Encrypt("文字列");
            cipher.Should().Equal(new byte[] { 0x3E, 0x16, 0x86, 0x60, 0xFE, 0x9B, 0x8F, 0xBF, 0x4F, 0x3C, 0x9F, 0xAE, 0xB6, 0x33, 0xCA, 0x01 });
            aesCrypt.Decrypt(cipher).Should().Be("文字列");
        }
    }
}
