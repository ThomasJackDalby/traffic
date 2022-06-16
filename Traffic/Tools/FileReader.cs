namespace Traffic.Tools
{
    public class FileReader : IDisposable
    {
        private readonly Stream stream;
        private readonly StreamReader reader;

        private string[] current = new string[0];
        private int index;

        public FileReader(Stream stream)
        {
            this.stream = stream;
            reader = new StreamReader(stream);
        }

        public void Dispose()
        {
            stream.Dispose();
            reader.Dispose();
        }

        public T Read<T>()
        {
            while (!reader.EndOfStream && index == current.Length)
            {
                current = reader.ReadLine()?.Split(" ") ?? new string[0];
                index = 0;
            }
            return (T)Convert.ChangeType(current[index++], typeof(T));
        }
    }
}