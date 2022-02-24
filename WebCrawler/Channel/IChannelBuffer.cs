using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Simple_web_crawler.Channel
{
    public interface IChannelBuffer
    {
        //int Add(string testTable);
        void Clear();
        Task Flush(string item);
    }
}