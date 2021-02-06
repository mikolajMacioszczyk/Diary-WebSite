using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Data.Repositories.Entry;
using Diary.Lib.UndoAction;

namespace Diary.Data.Services.Undo
{
    public class UndoService : IUndoService
    {
        private readonly IEntryRepository _entryRepository;
        private LinkedList<UndoAction> _undoStack;
        
        public UndoService(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
            _undoStack = new LinkedList<UndoAction>();
        }

        private void ServeRemoveUndoAction(UndoAction undo)
        {
            _entryRepository.AddOrUpdateEntryAsync(undo.Changed);
        }

        private async void ServeUpdateUndoAction(UndoAction undo)
        {
            if (!(await _entryRepository.UpdateEntryAsync(undo.Changed.Id, undo.Changed)).Succeded)
            {
                throw new Exception("Internal error. ServeUpdateUndoAction: Not existing entry");
            }
        }

        private async void ServeAddUndoAction(UndoAction undo)
        {
            if (!(await _entryRepository.DeleteEntryAsync(undo.Changed.Id)).Succeded)
            {
                throw new Exception("Internal error. ServeAddUndoAction: Not existing entry");
            }
        }

        public void Add(UndoAction action)
        {
            _undoStack.AddFirst(action);
        }

        public void Undo()
        {
            if (_undoStack.First != null)
            {
                var undo = _undoStack.First.Value;
                _undoStack.RemoveFirst();
                switch (undo.GetUndoActionType)
                {
                    case UndoActionType.RemoveUndoActionType:
                        ServeRemoveUndoAction(undo);
                        break;
                    case UndoActionType.UpdateUndoActionType:
                        ServeUpdateUndoAction(undo);
                        break;
                    case UndoActionType.AddUndoActionType:
                        ServeAddUndoAction(undo);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Undo(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Undo();
            }
        }

        public IEnumerable<UndoAction> GetUndoActionsAsync(int count)
        {
            return _undoStack.Take(count).ToList();
        }
    }
}