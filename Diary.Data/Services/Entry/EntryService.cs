using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diary.Data.Exceptions;
using Diary.Data.Repositories.Entry;
using Diary.Data.Services.Undo;
using Diary.Lib.UndoAction;

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

        private async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesValidIndex(int count)
        {
            var result = await _repository.GetEntriesAsync(_currentIndex, count);
            if (!result.Succeded)
            {
                throw new InternalError("GetEntriesValidIndex: returns false");
            }
            return result.Data;
        }
        
        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Max(_repository.GetRepositorySize() - count, 0);
            
            return await GetEntriesValidIndex(count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Min(_currentIndex + count, ValidIdx(count));
            
            return await GetEntriesValidIndex(count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesAsync(int count)
        {
            count = Math.Max(count, 0);
            _currentIndex = Math.Max(_currentIndex - count, 0);
            
            return await GetEntriesValidIndex(count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd)
        {
            NormalizeDates(ref dateStart, ref dateEnd);

            var data = await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd);
            if (!data.Succeded)
            {
                throw new InternalError("GetEntriesByDateBetweenAsync: returns false");
            }
            
            return data.Data;
        }
        
        private async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateValidIndex(DateTime dateStart, DateTime dateEnd, int count)
        {
            var data = await _repository.GetEntriesByDateBetweenAsync(dateStart, dateEnd, _currentByDateIdx, count);
            if (!data.Succeded)
            {
                throw new InternalError("GetEntriesByDateValidIndex: returns false");
            }
            
            return data.Data;
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = 0;

            return await GetEntriesByDateValidIndex(dateStart, dateEnd, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetNextEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = Math.Min(_currentByDateIdx + count, ValidIdx(count));
            
            return await GetEntriesByDateValidIndex(dateStart, dateEnd, count);
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetPreviousEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int count)
        {
            NormalizeDates(ref dateStart, ref dateEnd);
            _currentByDateIdx = Math.Min(_currentByDateIdx - count, ValidIdx(count));
            
            return await GetEntriesByDateValidIndex(dateStart, dateEnd, count);
        }

        public async Task<Lib.Entry.Entry> GetEntryByDateAsync(DateTime date) 
        {
            var result = await _repository.GetEntryByDateAsync(date);

            return result.Data;
        }

        public async Task<Lib.Entry.Entry> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result.Succeded)
            {
                return result.Data;
            }

            throw new ArgumentException($"No entry with id = {id}");
        }

        public async Task<Lib.Entry.Entry> AddOrUpdateEntryAsync(Lib.Entry.Entry entry)
        {
            var result = await _repository.AddOrUpdateEntryAsync(entry);
            if (result.Type == EntryRepositoryResultType.Add)
            {
                _undoService.Add(new AddUndoAction() {Changed = result.Data});
                return result.Data;
            }
            if (result.Type == EntryRepositoryResultType.Update)
            {
                _undoService.Add(new UpdateUndoAction(){Changed = result.Data});
                return result.Data;
            }
            throw new InternalError("AddOrUpdateEntryAsync: invalid result type");
        }

        public async Task<bool> UpdateEntryAsync(int id, Lib.Entry.Entry entry)
        {
            var result = await _repository.UpdateEntryAsync(id, entry);
            if (result.Type != EntryRepositoryResultType.Update)
            {
                throw new InternalError("UpdateEntryAsync returns invalid type");
            }
            if (result.Succeded)
            {
                _undoService.Add(new UpdateUndoAction(){Changed = result.Data});
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            var result = await _repository.DeleteEntryAsync(id);
            if (result.Type != EntryRepositoryResultType.Delete)
            {
                throw new InternalError("DeleteEntryAsync returns invalid type");
            }

            if (result.Succeded)
            {
                _undoService.Add(new RemoveUndoAction(){Changed = result.Data});
                return true;
            }

            return false;
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