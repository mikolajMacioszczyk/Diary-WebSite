using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.Data.Repositories.Entry
{
    public interface IEntryRepository
    {
        public int GetRepositorySize();
        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesAsync(int startIdx, int count);
        
        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd);

        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int startIdx, int count);

        public Task<Lib.Entry.Entry> GetEntryByDateAsync(DateTime date);
        
        public Task<Lib.Entry.Entry> GetByIdAsync(int id);

        public Task<Lib.Entry.Entry> AddOrUpdateEntryAsync(Lib.Entry.Entry entry);

        public Task<bool> UpdateEntryAsync(int id, Lib.Entry.Entry entry);

        public Task<bool> DeleteEntryAsync(int id);
    }
}