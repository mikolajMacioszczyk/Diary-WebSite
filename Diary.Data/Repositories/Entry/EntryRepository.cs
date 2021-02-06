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

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesAsync(int startIdx, int count)
        {
            return await _context.EntryItems.Skip(startIdx).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd)
        {
            return await _context.EntryItems
                .Where(e => e.Date >= dateStart && e.Date <= dateEnd)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lib.Entry.Entry>> GetEntriesByDateBetweenAsync(DateTime dateStart, DateTime dateEnd, int startIdx, int count)
        {
            return await _context.EntryItems
                .Where(e => e.Date >= dateStart && e.Date <= dateEnd)
                .Skip(startIdx)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Lib.Entry.Entry> GetEntryByDateAsync(DateTime date)
        {
            return await _context.EntryItems.FirstOrDefaultAsync(e => e.Date == date);
        }

        public async Task<Lib.Entry.Entry> GetByIdAsync(int id)
        {
            return await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Lib.Entry.Entry> AddOrUpdateEntryAsync(Lib.Entry.Entry entry)
        {
            var fromDb = await GetEntryByDateAsync(entry.Date);
            if (fromDb == null)
            {
                await _context.EntryItems.AddAsync(entry);
                await _context.SaveChangesAsync();
                size++;
                return entry;
            }
            fromDb.UpdateFrom(entry);
            await _context.SaveChangesAsync();
            return fromDb;
        }

        public async Task<bool> UpdateEntryAsync(int id, Lib.Entry.Entry entry)
        {
            var fromDb = await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);
            if (fromDb == null)
            {
                return false;
            }
            fromDb.UpdateFrom(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            var fromDb = await _context.EntryItems.FirstOrDefaultAsync(e => e.Id == id);
            if (fromDb == null)
            {
                return false;
            }
            _context.Remove(fromDb);
            await _context.SaveChangesAsync();
            size--;
            return true;
        }
    }
}