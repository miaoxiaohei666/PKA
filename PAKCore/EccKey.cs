using System;
using System.Numerics;
using System.Security.Cryptography;

namespace PKA.PAKCore
{
    public class EccKey
    {
        private ECDsa _eccAlgorithm;
        private BigInteger _publicKey;
        private BigInteger _privateKey;

        public EccKey(int keyLength)
        {
            _eccAlgorithm = ECDsa.Create(); // 创建 ECC 算法实例
            _eccAlgorithm.KeySize = keyLength;
        }

        public void GenerateEccKey()
        {
            _eccAlgorithm.GenerateKey(ECCurve.NamedCurves.nistP256); // 选择椭圆曲线参数

            // 获取生成的公钥和私钥
            var publicKey = _eccAlgorithm.ExportSubjectPublicKeyInfo();
            var privateKey = _eccAlgorithm.ExportPkcs8PrivateKey();

            // 将公钥和私钥转换为 BigInteger
            _publicKey = new BigInteger(publicKey);
            _privateKey = new BigInteger(privateKey);
        }
        
        public ECDsa EccAlgorithm
        {
            get => _eccAlgorithm;
            set => _eccAlgorithm = value ?? throw new ArgumentNullException(nameof(value));
        }

        public BigInteger PublicKey
        {
            get => _publicKey;
            set => _publicKey = value;
        }

        public BigInteger PrivateKey
        {
            get => _privateKey;
            set => _privateKey = value;
        }
    }
}
