using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Diary.Lib.UndoAction
{
    public class RemoveUndoAction : IUndoAction
    {
        [Key]
        public int Id { get; set; }
        public UndoActionType UndoActionType { get; } = UndoActionType.RemoveUndoActionType;
        [NotNull]
        public Entry.Entry Changed { get; set; }
    }
}