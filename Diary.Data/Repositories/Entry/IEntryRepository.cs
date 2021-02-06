using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.Data.Repositories.Entry
{
    public interface IEntryRepository
    {
        public int GetRepositorySize();
        public Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesAsync(int startIdx, int count);
        
        public Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd);

        public Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int startIdx, int count);

        public Task<EntryRepositoryResult<Lib.Entry.Entry>> GetEntryByDateAsync(DateTime date);
        
        public Task<EntryRepositoryResult<Lib.Entry.Entry>> GetByIdAsync(int id);

        public Task<EntryRepositoryResult<Lib.Entry.Entry>> AddOrUpdateEntryAsync(Lib.Entry.Entry entry);

        public Task<EntryRepositoryResult<Lib.Entry.Entry>> UpdateEntryAsync(int id, Lib.Entry.Entry entry);

        public Task<EntryRepositoryResult<Lib.Entry.Entry>> DeleteEntryAsync(int id);
    }
}