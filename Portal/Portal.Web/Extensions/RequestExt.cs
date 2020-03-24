using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Extensions
{
    public class RequestExt
    {
        public static async Task<T> GetObject<T>(Stream stream)
        {
            if (!(stream is null))
            {
                using (var reader = new StreamReader(stream, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var bodyString = await reader.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<T>(bodyString);
                }
            }
            return default;
        }
    }
}