using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StackExchange.Redis;

namespace HotelWeb.Data
{
    public class RedisClient
    {
        private static ConnectionMultiplexer Connection => LazyConnection.Value;
        private static ConfigurationOptions Options = new ConfigurationOptions
        {
            Password = ConfigurationManager.AppSettings["RedisPwd"],
            EndPoints = { ConfigurationManager.AppSettings["RedisHost"] },
        };
        private static int Database => int.Parse(ConfigurationManager.AppSettings["RedisDatabase"]);
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect(Options));


        private static IDatabase GetDb()
        {
            return Connection.GetDatabase(Database);
        }

        public static IEnumerable<string> GetSet(string key)
        {
            var db = GetDb();
            return db.SetMembers(key).Select(x => (string)x); ;
        }

        public static IEnumerable<string> GetSortedSet(string key, string sortBy, string sortOrder, string sortType, long offset = long.MinValue, long size = long.MaxValue)
        {
            var db = GetDb();

            if (string.IsNullOrWhiteSpace(sortBy))
                return GetSet(key);


            return db.Sort(key, offset, size, GetRedisSortOrder(sortOrder), GetRedisSortType(sortType), $"{sortBy}_*", new RedisValue[] { "*" }).Select(x => (string)x);
        }

        public static string GetString(string key)
        {
            var db = GetDb();
            return db.StringGet(key);
        }

        public static IEnumerable<string> GetStrings(IEnumerable<string> keys)
        {
            var db = GetDb();
            return db.StringGet(keys.Select(x => (RedisKey)x).ToArray()).Select(x => (string)x);
        }

        public static long GetSetCount(string key)
        {
            var db = GetDb();
            if (db.KeyType(key) == RedisType.Set)
                return db.SetLength(key);
            return db.SortedSetLength(key);
        }

        public static void UnionAndStore(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SetCombineAndStore(SetOperation.Union, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            db.KeyExpire(destinationKey, TimeSpan.FromMinutes(5));
        }

        public static void IntersectAndStore(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SetCombineAndStore(SetOperation.Intersect, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            db.KeyExpire(destinationKey, TimeSpan.FromMinutes(5));
        }

        public static void UnionAndStoreSortedSet(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SortedSetCombineAndStore(SetOperation.Union, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            db.KeyExpire(destinationKey, TimeSpan.FromMinutes(5));
        }

        public static void IntersectAndStoreSortedSet(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            db.KeyExpire(destinationKey, TimeSpan.FromMinutes(5));
        }

        public static IEnumerable<string> Intersect(IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            return db.SetCombine(SetOperation.Intersect, sourceKeys.Select(x => (RedisKey)x).ToArray()).Select(x => (string)x);
        }

        public static IEnumerable<string> IntersectSortedSet(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            var set = db.SortedSetRangeByRank(destinationKey);
            db.KeyDelete(destinationKey);
            return set.Select(x => (string)x);
        }

        public static void DifferenceAndStore(string destinationKey, IEnumerable<string> sourceKeys)
        {
            var db = GetDb();
            db.SetCombineAndStore(SetOperation.Difference, destinationKey, sourceKeys.Select(x => (RedisKey)x).ToArray());
            db.KeyExpire(destinationKey, TimeSpan.FromMinutes(5));
        }

        public static void RemoveStartingRange(string sourceKey, double toScore)
        {
            var db = GetDb();
            db.SortedSetRemoveRangeByScore(sourceKey, double.MinValue, toScore, Exclude.Stop);
        }

        public static void RemoveEndingRange(string sourceKey, double fromScore)
        {
            var db = GetDb();
            db.SortedSetRemoveRangeByScore(sourceKey, fromScore, double.MaxValue, Exclude.Start);
        }

        public static Range GetRange(string key)
        {
            var db = GetDb();
            var range = db.SortedSetRangeByScoreWithScores(key);
            return range.Any() ? new Range { TotalCurrentMin = range[0].Score.ToString(), TotalCurrentMax = range[range.Length - 1].Score.ToString() } : new Range();
        }

        private static Order GetRedisSortOrder(string sortOrder)
        {
            return sortOrder.ToLower() == "asc" ? Order.Ascending : Order.Descending;
        }

        private static SortType GetRedisSortType(string sortType)
        {
            return sortType.ToLower() == "alphabetic" ? SortType.Alphabetic : SortType.Numeric;
        }


    }
}