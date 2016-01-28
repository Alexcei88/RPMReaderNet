namespace RpmReaderNet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int a = 0x2;

            using (RpmReader reader = new RpmReader("esbadapterhost.rpm"))
            {
                if(reader.Validate())
                {
                    System.Console.WriteLine(reader.Version);
                }
            }
        }
    }
}