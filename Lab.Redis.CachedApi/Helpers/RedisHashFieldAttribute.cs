namespace Lab.Redis.CachedApi.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RedisHashFieldAttribute : Attribute
    {
        public RedisHashFieldAttribute(string redisFieldName, bool isArray = false) { }
    }
}
