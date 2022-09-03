using CoreTweet;
using Kzrnm.WindowScreenshot.Image;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Twitter;

public interface ITwitterApiService
{
    Task<Status> PostContentAsync(Tokens tokens, string text, long inReplyToId, CaptureImage[] images);
}

public class TwitterApiService : ITwitterApiService
{
    public async Task<Status> PostContentAsync(Tokens tokens, string text, long inReplyToId, CaptureImage[] images)
    {
        IEnumerable<long>? mediaIds = null;
        if (images.Length > 0)
        {
            var imgUploadTasks = images
                .Select(img => img.ToStream())
                .Select(s => tokens.Media.UploadAsync(s));
            mediaIds = (await Task.WhenAll(imgUploadTasks).ConfigureAwait(false)).Select(t => t.MediaId);
        }

        return await tokens.Statuses.UpdateAsync(
            status: text,
            in_reply_to_status_id: inReplyToId,
            auto_populate_reply_metadata: true,
            media_ids: mediaIds
        ).ConfigureAwait(false);
    }
}
