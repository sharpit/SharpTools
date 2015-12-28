using System;

namespace SharpIT.Tools.Hasher.Commons
{
    public static class FileHasherFactory
    {
        public static FileHasher Create(string[] consoleArgs)
        {
            if (consoleArgs == null || consoleArgs.Length == 0)
            {
                Console.WriteLine("Arguments are empty!");
                return null;
            }

            if (consoleArgs.Length == 1)
            {
                return new FileHasher(consoleArgs[0], HashAlgorithmType.Sha1);
            }

            return new FileHasher(consoleArgs[0], (HashAlgorithmType)Enum.Parse(typeof(HashAlgorithmType), consoleArgs[1], true));
        }
    }
}
