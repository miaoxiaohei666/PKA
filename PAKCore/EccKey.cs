using System;
using System.Numerics;
using System.Security.Cryptography;

namespace PKA.PAKCore
{
    public class EccKey
    {
        private int _keyLength;
        private ECDsa _eccAlgorithm;
        private BigInteger _publicKey;
        private BigInteger _privateKey;

        public EccKey(int keyLength)
        {
            _eccAlgorithm = ECDsa.Create(); // 创建 ECC 算法实例
            _keyLength = keyLength;
        }

        public void GenerateEccKey()
        {
            switch (_keyLength)
            {
                case 256:
                    _eccAlgorithm.GenerateKey(ECCurve.NamedCurves.nistP256); // 选择椭圆曲线参数
                    break;
                case 384:
                    _eccAlgorithm.GenerateKey(ECCurve.NamedCurves.nistP384); // 选择椭圆曲线参数
                    break;
                case 521:
                    _eccAlgorithm.GenerateKey(ECCurve.NamedCurves.nistP521); // 选择椭圆曲线参数
                    break;
                
            }
            

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
        
        public int KeyLength
        {
            get => _keyLength;
            set => _keyLength = value;
        }
    }
}
