using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SharpIT.Tools.Hasher.Commons
{
    public static class HashAlgorithmFactory
    {
        private static readonly Dictionary<HashAlgorithmType, Func<HashAlgorithm>> _map = new Dictionary<HashAlgorithmType, Func<HashAlgorithm>>()
        {
            {HashAlgorithmType.Md5, () => new MD5CryptoServiceProvider()},
            {HashAlgorithmType.Sha1, () => new SHA1Managed()},
            {HashAlgorithmType.Sha256, () => new SHA256Managed()},
        };

        public static HashAlgorithm Create(HashAlgorithmType algorithm)
        {
            Func<HashAlgorithm> value;
            if (!_map.TryGetValue(algorithm, out value))
            {
                throw new ArgumentOutOfRangeException("algorithm", "algorithm not supported");
            }

            return value();
        }
    }
}