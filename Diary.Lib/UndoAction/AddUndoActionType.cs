using System;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.UndoAction
{
    public class AddUndoActionType : IUndoAction
    {
        public int Id { get; set; }
        public UndoActionType UndoActionType { get; } = UndoActionType.AddUndoActionType;
        [NotNull]
        public Entry.Entry Changed { get; set; }
    }
}