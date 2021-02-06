using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diary.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Diary.Data.Repositories.Entry
{
    public class EntryRepository : IEntryRepository
    {
        private readonly DiaryDbContext _context;
        private int size;

        public EntryRepository(DiaryDbContext context)
        {
            _context = context;
            size = context.EntryItems.Count();
        }

        public int GetRepositorySize()
        {
            return size;
        }

        public async Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesAsync(int startIdx, int count)
        {
            return new EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>()
            {
                Type = EntryRepositoryResultType.Get,
                Succeded = true,
                Data = await _context.EntryItems.Skip(startIdx).Take(count).ToListAsync(),
            };
        }

        public async Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd)
        {
            return new EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>()
                {
                    Type = EntryRepositoryResultType.Get,
                    Succeded = true,
                    Data = await _context.EntryItems
                        .Where(e => e.Date >= dateStart && e.Date <= dateEnd)
                        .ToListAsync()
                };
        }

        public async Task<EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int startIdx, int count)
        {
            return new EntryRepositoryResult<IEnumerable<Lib.Entry.Entry>>()
            {
                Type = EntryRepositoryResultType.Get,
                Succeded = true,
                Data = await _context.EntryItems
                    .Where(e => e.Date >= dateStart && e.Date <= dateEnd)
                    .Skip(startIdx)
                    .Take(count)
                    .ToListAsync()
            };
        }

        public async Task<EntryRepositoryResult<Lib.Entry.Entry>> GetEntryByDateAsync(DateTime date)
        {
            var entry = await _context.EntryItems.FirstOrDefaultAsync(e => e.Date == date);
            return new EntryRepositoryResult<Lib.Entry.Entry>()
            {
                Type = EntryRepositoryResultType.Get,
                Succeded = entry != null,
                Data = entry
            };
        }

        public async Task<EntryRepositoryResult<Lib.Entry.Entry>> GetByIdAsync(int id)
        {
            var entry = await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);

            return new EntryRepositoryResult<Lib.Entry.Entry>()
            {
                Type = EntryRepositoryResultType.Get,
                Succeded = entry != null,
                Data = entry
            };
        }

        public async Task<EntryRepositoryResult<Lib.Entry.Entry>> AddOrUpdateEntryAsync(Lib.Entry.Entry entry)
        {
            var fromDb = (await GetEntryByDateAsync(entry.Date)).Data;
            if (fromDb == null)
            {
                await _context.EntryItems.AddAsync(entry);
                await _context.SaveChangesAsync();
                size++;
                return new EntryRepositoryResult<Lib.Entry.Entry>()
                {
                    Type = EntryRepositoryResultType.Add,
                    Succeded = true,
                    Data = entry
                };
            }
            fromDb.UpdateFrom(entry);
            await _context.SaveChangesAsync();
            return new EntryRepositoryResult<Lib.Entry.Entry>()
            {
                Type = EntryRepositoryResultType.Update,
                Succeded = true,
                Data = fromDb
            };
        }

        public async Task<EntryRepositoryResult<Lib.Entry.Entry>> UpdateEntryAsync(int id, Lib.Entry.Entry entry)
        {
            var fromDb = await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);
            if (fromDb == null)
            {
                return new EntryRepositoryResult<Lib.Entry.Entry>()
                {
                    Type = EntryRepositoryResultType.Update,
                    Succeded = false,
                    Data = null
                };
            }
            fromDb.UpdateFrom(entry);
            await _context.SaveChangesAsync();
            return new EntryRepositoryResult<Lib.Entry.Entry>()
            {
                Type = EntryRepositoryResultType.Update,
                Succeded = true,
                Data = fromDb
            };
        }

        public async Task<EntryRepositoryResult<Lib.Entry.Entry>> DeleteEntryAsync(int id)
        {
            var fromDb = await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);
            if (fromDb == null)
            {
                return new EntryRepositoryResult<Lib.Entry.Entry>()
                {
                    Type = EntryRepositoryResultType.Delete,
                    Succeded = false,
                    Data = null
                };
            }
            _context.Remove(fromDb);
            await _context.SaveChangesAsync();
            size--;
            return new EntryRepositoryResult<Lib.Entry.Entry>()
            {
                Type = EntryRepositoryResultType.Delete,
                Succeded = true,
                Data = fromDb
            };
        }
    }
}