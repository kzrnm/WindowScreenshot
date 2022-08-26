using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.TwitterJikkyo.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Models;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Kzrnm.TwitterJikkyo.Models;

public  class ContentServiceTest : ObservableObject
{
    [Theory]
    [InlineData("https://twitter.com/kzlogos/status/1563235104331218944", 1563235104331218944)]
    [InlineData("1563235104331218944", 1563235104331218944)]
    public void TryParseTweetId(string input, long? tweetId)
    {
        ContentService.TryParseTweetId(input, out var result).Should().Be(tweetId.HasValue);
        result.Should().Be(tweetId!.Value);
    }
}
