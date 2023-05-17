using System;
using System.Collections.Generic;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace PKA.PAKCore;
public class ElGamalKey
{
    private int _keyLength;
    private readonly SecureRandom _random;
    private ElGamalPublicKeyParameters _publicKey;
    private BigInteger _privateKey;

    public ElGamalKey(int keyLength)
    {
        _keyLength = keyLength;
        _random = new SecureRandom();
    }

    public void GenerateElGamalKey()
    {
        var p = BigInteger.ProbablePrime(_keyLength, _random);
        BigInteger alpha;
        do
        {
            alpha = FindPrimitiveRoot(p);
        } while (alpha.Equals(BigInteger.Zero));
        var x = GenerateRandomInteger(p);
        var y = alpha.ModPow(x, p);
        
        _publicKey = new ElGamalPublicKeyParameters(alpha, p, y);
        _privateKey = y.ModPow(x, p);
    }

    private BigInteger FindPrimitiveRoot(BigInteger p)
    {
        // 获取 p - 1 的素因子
        List<BigInteger> primeFactors = GetPrimeFactors(p.Subtract(BigInteger.One));

        // 尝试不同的 g 值
        for (BigInteger g = BigInteger.Two; g.CompareTo(p) < 0; g = g.Add(BigInteger.One))
        {
            bool isPrimitiveRoot = true;

            // 检查 g 是否满足原根的条件
            foreach (BigInteger factor in primeFactors)
            {
                BigInteger exp = p.Subtract(BigInteger.One).Divide(factor);
                BigInteger result = g.ModPow(exp, p);

                if (result.Equals(BigInteger.One))
                {
                    isPrimitiveRoot = false;
                    break;
                }
            }

            if (isPrimitiveRoot)
            {
                return g;
            }
        }

        return BigInteger.Zero;
    }

    
    private List<BigInteger> GetPrimeFactors(BigInteger number)
    {
        List<BigInteger> factors = new List<BigInteger>();

        BigInteger i = BigInteger.Two;
        while (i.Multiply(i).CompareTo(number) <= 0)
        {
            while (Equals(number.Mod(i), BigInteger.Zero))
            {
                factors.Add(i);
                number = number.Divide(i);
            }
            i = i.Add(BigInteger.One);
        }

        if (number.CompareTo(BigInteger.One) > 0)
        {
            factors.Add(number);
        }

        return factors;
    }



    private BigInteger GenerateRandomInteger(BigInteger p)
    {
        // 随机生成一个整数，满足 1 ≤ x < p-1
        BigInteger x;
        do
        {
            x = new BigInteger(p.BitLength, _random);
        } while (x.CompareTo(BigInteger.One) < 0 || x.CompareTo(p.Subtract(BigInteger.One)) >= 0);

        return x;
    }

    public int KeyLength
    {
        get => _keyLength;
        set => _keyLength = value;
    }

    public ElGamalPublicKeyParameters PublicKey
    {
        get => _publicKey;
        set => _publicKey = value ?? throw new ArgumentNullException(nameof(value));
    }

    public BigInteger PrivateKey
    {
        get => _privateKey;
        set => _privateKey = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class ElGamalPublicKeyParameters
{
    private BigInteger _alpha;
    private BigInteger _p;
    private BigInteger _y;

    public ElGamalPublicKeyParameters(BigInteger alpha, BigInteger p, BigInteger y)
    {
        _alpha = alpha;
        _p = p;
        _y = y;
    }

    public BigInteger Alpha
    {
        get => _alpha;
        set => _alpha = value ?? throw new ArgumentNullException(nameof(value));
    }

    public BigInteger P
    {
        get => _p;
        set => _p = value ?? throw new ArgumentNullException(nameof(value));
    }

    public BigInteger Y
    {
        get => _y;
        set => _y = value ?? throw new ArgumentNullException(nameof(value));
    }
}
