using System.Collections.Generic;
using Diary.Lib.UndoAction;

namespace Diary.Data.Services.Undo
{
    public interface IUndoService
    {
        void Add(UndoAction action);
        void Undo();
        void Undo(int times);

        IEnumerable<UndoAction> GetUndoActionsAsync(int count);
    }
}