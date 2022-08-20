using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.Wpf.Configs;

public class ConfigUpdatedEventArgs<T> : EventArgs
{
    public ConfigUpdatedEventArgs(T config)
    {
        Config = config;
    }
    public T Config { get; }
}
public class ConfigWrapper<T> where T : new()
{
    private ConfigWrapper(T value, string loadPath)
    {
        _value = value;
        LoadPath = loadPath;
    }

    public string LoadPath { get; }
    private T _value;
    public T Value
    {
        set
        {
            _value = value;
            ConfigUpdated?.Invoke(this, new(value));
        }
        get => _value;
    }

    public event EventHandler<ConfigUpdatedEventArgs<T>>? ConfigUpdated;

    public static async Task<ConfigWrapper<T>> LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        T value;
        try
        {
            using var fs = new FileStream(path, FileMode.Open);
            value = await LoadAsync(fs, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            value = new T();
        }

        return new ConfigWrapper<T>(value, path);
    }

    private static async Task<T> LoadAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));

        try
        {
            return (await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken).ConfigureAwait(false)) ?? new();
        }
        catch { }
        return new();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
        => await SaveAsync(LoadPath, cancellationToken).ConfigureAwait(false);

    public async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        var tmpPath = $"{path}.tmp";
        using (var fs = new FileStream(tmpPath, FileMode.Create))
            await SaveAsync(fs, cancellationToken).ConfigureAwait(false);

        File.Move(tmpPath, path, true);
    }
    public async Task SaveAsync(Stream stream, CancellationToken cancellationToken)
    {
        await JsonSerializer.SerializeAsync(
            stream,
            Value,
            new JsonSerializerOptions
            {
                WriteIndented = true,
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
