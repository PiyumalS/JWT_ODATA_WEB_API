using System.Collections.Generic;

namespace JWT_ODATA_WEB_API.Utils
{
    public class ODataMetadata<T> where T : class
    {
        public ODataMetadata(IEnumerable<T> result, long? count)
        {
            Count = count;
            Results = result;
        }

        public IEnumerable<T> Results { get; }

        public long? Count { get; }
    }
}