using NReJSON;
using StackExchange.Redis;
using System.Text.Json;

namespace Lab.Redis.CachedApi.Cache
{
    public sealed class TestJsonSerializer: ISerializerProxy
    {

        public string Serialize<TObjectType>(TObjectType obj)
        {
            return JsonSerializer.Serialize<TObjectType>(obj);
        }

        TResult ISerializerProxy.Deserialize<TResult>(RedisResult serializedValue)
        {
            return JsonSerializer.Deserialize<TResult>(serializedValue.ToString());
        }
    }
}
