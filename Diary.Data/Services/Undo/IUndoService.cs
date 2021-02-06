using System.Collections.Generic;
using Diary.Lib.UndoAction;

namespace Diary.Data.Services.Undo
{
    public interface IUndoService
    {
        void Add();
        void Undo();
        void Undo(int times);

        IEnumerable<IUndoAction> GetUndoActionsAsync(int count);
    }
}