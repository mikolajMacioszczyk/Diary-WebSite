using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.Entry
{
    public class Entry
    {
        [Key] 
        public int Id { get; set; }
        [NotNull]
        public DateTime Date { get; set; }
        [NotNull]
        public string Text { get; set; }

        public string FilePath { get; set; }
        
        public bool IsFavorite { get; set; }

        public void UpdateFrom(Entry from)
        {
            Date = from.Date;
            Text = from.Text;
            FilePath = from.FilePath;
            IsFavorite = from.IsFavorite;
        }
    }
}