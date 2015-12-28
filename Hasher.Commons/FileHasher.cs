using System;
using System.IO;

namespace SharpIT.Tools.Hasher.Commons
{
    public class FileHasher
    {
        protected readonly string _FilePath;
        protected readonly HashAlgorithmType _Algorithm;

        public FileHasher(string filePath, HashAlgorithmType hashingAlgorithm)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            _FilePath = filePath;
            _Algorithm = hashingAlgorithm;
        }

        public void Compute()
        {
            var hash = GetHash();
            SaveHash(hash);
        }

        protected byte[] GetHash()
        {
            using (var stream = File.OpenRead(_FilePath))
            {
                using (var hasher = HashAlgorithmFactory.Create(_Algorithm))
                {
                    return hasher.ComputeHash(stream);
                }
            }
        }

        protected void SaveHash(byte[] hash)
        {
            var textHash = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            File.WriteAllText(GetFileHashFileName(_FilePath), textHash);
        }

        protected string GetFileHashFileName(string filePath)
        {
            var fi = new FileInfo(filePath);
            var directory = fi.Directory.FullName;
            var hashFileName = Path.GetFileName(filePath);

            return string.Format(Path.Combine(directory, string.Format("{0}.{1}", hashFileName, GetFileExtensionBy(_Algorithm))));
        }

        protected string GetFileExtensionBy(HashAlgorithmType algorithm)
        {
            switch (algorithm)
            {
                case HashAlgorithmType.Md5:
                    return "md5";

                case HashAlgorithmType.Sha1:
                    return "sha1";

                case HashAlgorithmType.Sha256:
                    return "sha256";
            }

            throw new ArgumentOutOfRangeException("algorithm");
        }
    }


}

