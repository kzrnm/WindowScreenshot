using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kzrnm.TwitterJikkyo.Twitter;
public  class TwitterService
{
    public TwitterService(string apiKey, string apiSecret)
    {
        ApiKey = apiKey;
        ApiSecret =apiSecret;

    }
    public string ApiKey { get; }
       public string ApiSecret{get;}
}
