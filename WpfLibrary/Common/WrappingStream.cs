using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.Wpf.Common;
public class WrappingStream : Stream
{
    internal Stream? baseStream;
    public WrappingStream(Stream baseStream)
    {
        ArgumentNullException.ThrowIfNull(baseStream);
        this.baseStream = baseStream;
    }
    public override bool CanRead => true;
    public override bool CanSeek => baseStream?.CanSeek ?? false;
    public override bool CanWrite => false;
    public override long Length => baseStream?.Length ?? 0;
    public override long Position
    {
        set
        {
            if (baseStream is not null)
                baseStream.Position = value;
        }
        get => baseStream?.Position ?? 0;
    }


    [MemberNotNull(nameof(baseStream))]
    private void ThrowIfDisposed()
    {
        if (baseStream == null)
            ThrowDisposedException();
        [DoesNotReturn]
        static void ThrowDisposedException() => throw new ObjectDisposedException(nameof(WrappingStream));
    }

    public override void Flush()
    {
        ThrowIfDisposed();
        baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        return baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        ThrowIfDisposed();
        return baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        ThrowIfDisposed();
        baseStream.SetLength(value);
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        baseStream.Write(buffer, offset, count);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            baseStream?.Dispose();
            baseStream = null;
        }
        base.Dispose(disposing);
    }
#if !NETFRAMEWORK
    public override int Read(Span<byte> buffer)
    {
        ThrowIfDisposed();
        return baseStream.Read(buffer);
    }
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        return baseStream.ReadAsync(buffer, offset, count, cancellationToken);
    }
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return baseStream.ReadAsync(buffer, cancellationToken);
    }
#endif
}