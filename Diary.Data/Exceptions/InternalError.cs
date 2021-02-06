using System;

namespace Diary.Data.Exceptions
{
    public class InternalError : Exception
    {
        public InternalError(string cause) : base(cause)
        {
        }
    }
}