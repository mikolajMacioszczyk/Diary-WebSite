using System;
using System.ComponentModel.DataAnnotations;

namespace Diary.Lib.UndoAction
{
    public interface IUndoAction
    {
        [Key]
        public int Id { get; set; }

        public UndoActionType UndoActionType { get; }
        public Entry.Entry Changed { get; set; }
    }
}