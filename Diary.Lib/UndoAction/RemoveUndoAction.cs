using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.UndoAction
{
    public class RemoveUndoAction : UndoAction
    {
        public override UndoActionType GetUndoActionType => UndoActionType.RemoveUndoActionType;
    }
}