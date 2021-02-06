namespace Diary.Data.Repositories.Entry
{
    public struct EntryRepositoryResult<T>
    {
        public EntryRepositoryResultType Type { get; set; }
        public T Data { get; set; }
        public bool Succeded { get; set; }
    }
}