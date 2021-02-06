﻿using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Data.Repositories.Entry;
using Diary.Lib.UndoAction;

namespace Diary.Data.Services.Undo
{
    public class UndoService : IUndoService
    {
        private readonly IEntryRepository _entryRepository;
        private LinkedList<IUndoAction> _undoStack;
        
        public UndoService(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
            _undoStack = new LinkedList<IUndoAction>();
        }

        public void Add()
        {
            throw new System.NotImplementedException();
        }

        private void ServeRemoveUndoAction(IUndoAction undo)
        {
            _entryRepository.AddOrUpdateEntryAsync(undo.Changed);
        }

        private async void ServeUpdateUndoAction(IUndoAction undo)
        {
            if (!await _entryRepository.UpdateEntryAsync(undo.Changed.Id, undo.Changed))
            {
                throw new Exception("Internal error. ServeUpdateUndoAction: Not existing undo");
            }
        }

        private void ServeAddUndoAction(IUndoAction undo)
        {
            
        }
        
        public void Undo()
        {
            if (_undoStack.First != null)
            {
                var undo = _undoStack.First.Value;
                _undoStack.RemoveFirst();
                switch (undo.UndoActionType)
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

        public IEnumerable<IUndoAction> GetUndoActionsAsync(int count)
        {
            return _undoStack.Take(count).ToList();
        }
    }
}