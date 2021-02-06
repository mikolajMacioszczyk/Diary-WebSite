using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diary.Data.Repositories.Entry;
using Diary.Data.Services.Undo;

namespace Diary.Data.Services.Entry
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _repository;
        private readonly IUndoService _undoService;
        private int _currentIndex;
        private int _currentByDateIdx;
        private int ValidIdx(int count) => Math.Max(_repository.GetRepositorySize() - count, 0);
        
        public EntryService(IEntryRepository repository, IUndoService undoService)
        {
            _repository = repository;
            _undoService = undoService;
            _currentIndex = 0;
            _currentByDateIdx = 0;
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Max(_repository.GetRepositorySize() - count, 0);
            return await _repository.GetEntriesAsync(_currentIndex, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Min(_currentIndex + count, ValidIdx(count));
            return await _repository.GetEntriesAsync(_currentIndex, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Max(_currentIndex - count, 0);
            return await _repository.GetEntriesAsync(_currentIndex, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            return await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = 0;
            return await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd, _currentByDateIdx, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = Math.Min(_currentByDateIdx + count, ValidIdx(count));
            return await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd, _currentByDateIdx, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = Math.Min(_currentByDateIdx - count, ValidIdx(count));
            return await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd, _currentByDateIdx, count);
        }

        public async Task<Lib.Entry.Entry> GetEntryByDateAsync(DateTime date)
        {
            return await _repository.GetEntryByDateAsync(date);
        }

        public async Task<Lib.Entry.Entry> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Lib.Entry.Entry> AddOrUpdateEntryAsync(Lib.Entry.Entry entry)
        {
            return await _repository.AddOrUpdateEntryAsync(entry);
        }

        public async Task<bool> UpdateEntryAsync(int id, Lib.Entry.Entry entry)
        {
            return await _repository.UpdateEntryAsync(id, entry);
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            return await _repository.DeleteEntryAsync(id);
        }
        
        private void NormalizeDates(ref DateTime start, ref DateTime end)
        {
            if (start > end)
            {
                DateTime temp = start;
                start = end;
                end = temp;    
            }
        }
    }
}