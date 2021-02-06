using System;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.UndoAction
{
    public class AddUndoAction : UndoAction
    {
        public override UndoActionType GetUndoActionType => UndoActionType.AddUndoActionType;
    }
}