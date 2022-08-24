namespace Kzrnm.Wpf.Common;
public class WrappingStreamTest
{
    [Fact]
    public void ReadArray()
    {
        WrappingStream ws;
        byte[] buffer = new byte[6];
        using (var ms = new MemoryStream(Enumerable.Range(10, 100).Select(i => (byte)i).ToArray()))
        using (ws = new WrappingStream(ms))
        {
            ws.Read(buffer, 1, 3);
        }
        buffer.Should().Equal(0, 10, 11, 12, 0, 0);
        ws.baseStream.Should().BeNull();
    }
    [Fact]
    public void ReadSpan()
    {
        WrappingStream ws;
        byte[] buffer = new byte[6];
        using (var ms = new MemoryStream(Enumerable.Range(10, 100).Select(i => (byte)i).ToArray()))
        using (ws = new WrappingStream(ms))
        {
            ws.Read(buffer.AsSpan()[1..4]);
        }
        buffer.Should().Equal(0, 10, 11, 12, 0, 0);
        ws.baseStream.Should().BeNull();
    }
    [Fact]
    public async Task ReadAsyncMemory()
    {
        WrappingStream ws;
        byte[] buffer = new byte[6];
        using (var ms = new MemoryStream(Enumerable.Range(10, 100).Select(i => (byte)i).ToArray()))
        using (ws = new WrappingStream(ms))
        {
            await ws.ReadAsync(new Memory<byte>(buffer)[1..4]);
        }
        buffer.Should().Equal(0, 10, 11, 12, 0, 0);
        ws.baseStream.Should().BeNull();
    }
}