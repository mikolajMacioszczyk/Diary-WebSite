using Diary.Lib;
using Diary.Lib.Entry;
using Diary.Lib.UndoAction;
using Microsoft.EntityFrameworkCore;

namespace Diary.Data.Context
{
    public class DiaryDbContext : DbContext
    {
        public DiaryDbContext(DbContextOptions<DiaryDbContext> options) : base(options)
        {}
        public DbSet<Entry> EntryItems { get; set; }
    }
}