using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace PKA.PAKCore
{
    public class RabinKey
    {
        private readonly SecureRandom _random;
        private int _keyLength;
        private BigInteger _publicKey;
        private int _e;
        
        public RabinKey(int keyLength)
        {
            _keyLength = keyLength;
            _e = 2;
            _random = new SecureRandom();
        }

        public void GenerateRabinKey()
        {
            BigInteger p, q, n;

            do
            {
                p = BigInteger.ProbablePrime(_keyLength/2, _random);
                ;
                q = BigInteger.ProbablePrime(_keyLength/2, _random);
                ;
                n = p.Multiply(q);
            } while (p.Mod(BigInteger.ValueOf(4)).CompareTo(BigInteger.Three) != 0 ||
                     q.Mod(BigInteger.ValueOf(4)).CompareTo(BigInteger.Three) != 0 ||
                     !Equals(GetGreatestCommonDivisor(BigInteger.ValueOf(_e), n), BigInteger.One));

            _publicKey = n;
        }

        private BigInteger GetGreatestCommonDivisor(BigInteger a, BigInteger b)
        {
            while (!b.Equals(BigInteger.ValueOf(0)))
            {
                BigInteger remainder = a.Mod(b);
                a = b;
                b = remainder;
            }

            return a;
        }

        public int KeyLength
        {
            get => _keyLength;
            set => _keyLength = value;
        }

        public BigInteger PublicKey
        {
            get => _publicKey;
            set => _publicKey = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int E
        {
            get => _e;
            set => _e = value;
        }
    }
}