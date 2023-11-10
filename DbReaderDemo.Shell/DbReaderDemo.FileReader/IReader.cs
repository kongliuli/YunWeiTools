namespace DbReaderDemo.FileReader
{
    public interface IReader
    {
        public T ReadFile<T>(string filepath,out ref T );







    }
}
