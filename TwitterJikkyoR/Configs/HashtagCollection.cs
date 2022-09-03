using Kzrnm.WindowScreenshot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kzrnm.TwitterJikkyo.Configs;


[JsonConverter(typeof(JsonConverter))]
public class HashtagCollection : SelectorObservableCollection<string>
{
    private class JsonConverter : JsonConverter<HashtagCollection>
    {
        public override HashtagCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var j = JsonSerializer.Deserialize<HashtagCollectionJson>(ref reader, options);
            return new HashtagCollection((IEnumerable<string>?)j?.Hashtags ?? Array.Empty<string>(), j?.MaxTagNum ?? 100);
        }

        public override void Write(Utf8JsonWriter writer, HashtagCollection value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, new HashtagCollectionJson(value.MaxTagNum, value), options);
        }

        private record HashtagCollectionJson(int MaxTagNum, SelectorObservableCollection<string>? Hashtags);
    }
    public HashtagCollection() : this(100) { }
    public HashtagCollection(int maxTagNum) : base()
    {
        if (maxTagNum <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxTagNum), nameof(maxTagNum) + " must be positive number");
        MaxTagNum = maxTagNum;
    }
    public HashtagCollection(IEnumerable<string> collection, int maxTagNum = 100) : base(collection)
    {
        if (maxTagNum <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxTagNum), nameof(maxTagNum) + " must be positive number");
        MaxTagNum = maxTagNum;
    }

    public int MaxTagNum { get; }

    /// <summary>
    /// <paramref name="hashtag"/> を先頭に置く。
    /// 既に存在する場合は <see langword="false"/>, 新規に追加した場合は <see langword="true"/> を返す。
    /// </summary>
    /// <param name="hashtag"></param>
    /// <returns></returns>
    public bool AddUnique(string hashtag)
    {
        switch (IndexOf(hashtag))
        {
            case < 0:
                InsertItem(0, hashtag);
                return true;
            case var index:
                MoveItem(index, 0);
                return false;
        }
    }

    protected override void InsertItem(int index, string item)
    {
        base.InsertItem(index, item);
        while (Count > MaxTagNum)
            RemoveAt(Count - 1);
    }
}