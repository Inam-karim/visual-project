using LibraryManagementSystem.Models;
using MongoDB.Driver;

namespace LibraryManagementSystem.Repositories
{
    public class CounterRepository
    {
        private readonly IMongoCollection<Counter> _counters;

        public CounterRepository(IMongoDatabase database)
        {
            _counters = database.GetCollection<Counter>("counters");
        }

        public int GetNextSequence(string collectionName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, collectionName);
            var update = Builders<Counter>.Update.Inc(c => c.Seq, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
            var counter = _counters.FindOneAndUpdate(filter, update, options);
            return counter.Seq;
        }
    }
} 