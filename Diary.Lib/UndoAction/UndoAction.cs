using System;
using System.ComponentModel.DataAnnotations;

namespace Diary.Lib.UndoAction
{
    public abstract class UndoAction
    {
        public Entry.Entry Changed { get; set; }

        public abstract UndoActionType GetUndoActionType { get; }
    }
}