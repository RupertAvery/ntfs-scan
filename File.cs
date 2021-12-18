namespace NTFSScan
{
    public class File
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public File(string path, string name, long size)
        {
            Path = path;
            Name = name;
            Size = size;
        }
    }

}