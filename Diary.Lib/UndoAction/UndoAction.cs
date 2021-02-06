using System;
using System.ComponentModel.DataAnnotations;

namespace Diary.Lib.UndoAction
{
    public class UndoAction
    {
        [Key]
        public int Id { get; set; }

        public Entry.Entry Changed { get; set; }

        public virtual UndoActionType GetUndoActionType { get; set; }
    }
}