using System.Security.Cryptography;

namespace PKA.PAKCore;

using System;
using System.Numerics;

public class RsaKey
{
    private int _keyLength;
    private int _iterations;
    private BigInteger _modulus;
    private BigInteger _publicExponent;
    private BigInteger _privateExponent;


    public RsaKey(int keyLength, int iterations, BigInteger modulus, BigInteger publicExponent,
        BigInteger privateExponent)
    {
        _keyLength = keyLength;
        _iterations = iterations;
        _modulus = modulus;
        _publicExponent = publicExponent;
        _privateExponent = privateExponent;
    }

    public RsaKey(int keyLength, int iterations)
    {
        _keyLength = keyLength;
        _iterations = iterations;
    }

    public void GenerateRsaKey()
    {
        // 1. 选择两个大素数 p 和 q
        BigInteger p = GenerateLargePrime(_keyLength / 2);
        BigInteger q = GenerateLargePrime(_keyLength / 2);

        // 2. 计算模数 n = p * q
        BigInteger n = BigInteger.Multiply(p, q);

        // 3. 计算欧拉函数 φ(n) = (p-1) * (q-1)
        BigInteger phi = BigInteger.Multiply(BigInteger.Subtract(p, 1), BigInteger.Subtract(q, 1));

        // 4. 选择公钥指数 e，使得 1 < e < φ(n)，且 e 和 φ(n) 互质
        BigInteger e = GeneratePublicKeyExponent(phi);

        // 5. 计算私钥指数 d，满足 (d * e) % φ(n) = 1
        BigInteger d = GeneratePrivateKeyExponent(e, phi);

        _modulus = n;
        _publicExponent = e;
        _privateExponent = d;
    }

    private BigInteger GenerateLargePrime(int bitLength)
    {
        // 在指定位数范围内生成一个大素数
        BigInteger prime;
        do
        {
            prime = GenerateRandom(bitLength);
        } while (!IsPrime(prime, _iterations));

        return prime;
    }

    private BigInteger GenerateRandom(int bitLength)
    {
        var random = new Random();
        byte[] data = new byte[(bitLength + 7) / 8];
        random.NextBytes(data);
        data[^1] &= (byte)(0xFF >> (data.Length * 8 - bitLength));
        return new BigInteger(data);
    }
    
    private bool IsPrime(BigInteger n, int iterations)
    {
        // 如果 n 小于等于 1，则不是素数
        if (n <= 1)
            return false;

        // 处理一些小的素数
        if (n == 2 || n == 3)
            return true;

        // 如果 n 是偶数，则不是素数（除了 2）
        if (BigInteger.Remainder(n, 2) == 0)
            return false;

        // 将 n-1 分解为 (2^r) * d，其中 d 是奇数
        BigInteger d = BigInteger.Subtract(n, 1);
        int r = 0;
        while (BigInteger.Remainder(d, 2) == 0)
        {
            d = BigInteger.Divide(d, 2);
            r++;
        }

        // 进行 Miller-Rabin 素数测试
        for (int i = 0; i < iterations; i++)
        {
            BigInteger a = BigInteger.Add(RandomBase(BigInteger.Subtract(n, 2)), 2);
            BigInteger x = BigInteger.ModPow(a, d, n);

            if (x == 1 || BigInteger.Equals(x, BigInteger.Subtract(n, 1)))
                continue;

            for (int j = 0; j < r - 1; j++)
            {
                x = BigInteger.ModPow(x, 2, n);
                if (x == 1)
                    return false;
                if (BigInteger.Equals(x, BigInteger.Subtract(n, 1)))
                    break;
            }

            if (!BigInteger.Equals(x, BigInteger.Subtract(n, 1)))
                return false;
        }

        return true;
    }

    private BigInteger RandomBase(BigInteger max)
    {
        byte[] bytes = max.ToByteArray();
        BigInteger result;
        do
        {
            RandomNumberGenerator.Fill(bytes);
            bytes[^1] &= 0x7F; // Ensure positive number
            result = new BigInteger(bytes);
        } while (BigInteger.Compare(result, max) >= 0);

        return result;
    }

    private BigInteger GeneratePrivateKeyExponent(BigInteger bigInteger, BigInteger phi)
    {
        BigInteger d = ExtendedEuclidean(bigInteger, phi);
        if (d < 0)
        {
            // 如果 d 是负数，将其变为正数
            d = BigInteger.Add(d, phi);
        }
        return d;
    }

    private BigInteger GeneratePublicKeyExponent(BigInteger phi)
    {
        BigInteger e = 65537; // 常用的公钥指数
        while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
        {
            BigInteger.Add(e,1); // 递增公钥指数直到与 phi 互质
        }
        return e;
    }

    private BigInteger ExtendedEuclidean(BigInteger a, BigInteger b)
    {
        BigInteger oldR = a;
        BigInteger r = b;
        BigInteger oldS = BigInteger.One;
        BigInteger s = BigInteger.Zero;
        BigInteger oldT = BigInteger.Zero;
        BigInteger t = BigInteger.One;

        while (!BigInteger.Zero.Equals(r))
        {
            BigInteger quotient = BigInteger.Divide(oldR, r);

            BigInteger temp = r;
            r = BigInteger.Subtract(oldR, BigInteger.Multiply(quotient, r));
            oldR = temp;

            temp = s;
            s = BigInteger.Subtract(oldS, BigInteger.Multiply(quotient, s));
            oldS = temp;

            temp = t;
            t = BigInteger.Subtract(oldT, BigInteger.Multiply(quotient, t));
            oldT = temp;
        }

        return oldS;
    }

    public int KeyLength
    {
        get => _keyLength;
        set => _keyLength = value;
    }

    public int Iterations
    {
        get => _iterations;
        set => _iterations = value;
    }

    public BigInteger Modulus
    {
        get => _modulus;
        set => _modulus = value;
    }

    public BigInteger PublicExponent
    {
        get => _publicExponent;
        set => _publicExponent = value;
    }

    public BigInteger PrivateExponent
    {
        get => _privateExponent;
        set => _privateExponent = value;
    }
}