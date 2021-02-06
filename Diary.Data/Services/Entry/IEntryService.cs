using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.Data.Services.Entry
{
    public interface IEntryService
    {
        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesAsync(int count);

        public Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesAsync(int count);

        public Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesAsync(int count);

        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd);

        public Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count);

        public Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count);

        public Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count);
        
        public Task<Lib.Entry.Entry> GetEntryByDateAsync(DateTime date);
        
        public Task<Lib.Entry.Entry> GetByIdAsync(int id);

        public Task<Lib.Entry.Entry> AddOrUpdateEntryAsync(Lib.Entry.Entry entry);

        public Task<bool> UpdateEntryAsync(int id, Lib.Entry.Entry entry);

        public Task<bool> DeleteEntryAsync(int id);
    }
}