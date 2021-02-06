using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.UndoAction
{
    public class UpdateUndoAction : UndoAction
    {
        public override UndoActionType GetUndoActionType => UndoActionType.UpdateUndoActionType;
    }
}