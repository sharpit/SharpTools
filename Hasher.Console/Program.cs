using SharpIT.Tools.Hasher.Commons;

namespace SharpIT.Tools.Hasher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileHasher = FileHasherFactory.Create(args);

            if (fileHasher == null)
            {
                System.Console.ReadLine();
                return;
            }

            fileHasher.Compute();
        }
    }
}
