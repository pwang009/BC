using System.Security.Cryptography;

namespace BlockChainCourse.Cryptography
{
    public class DigitalSignature
    {
        private readonly RSAParameters _publicKey;
        private readonly RSAParameters _privateKey;

        public DigitalSignature()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
         => RSAFormatter(_privateKey).CreateSignature(hashOfDataToSign);

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
            => RSADeformatter(_publicKey).VerifySignature(hashOfDataToSign, signature);

        private RSAPKCS1SignatureDeformatter RSADeformatter(RSAParameters key)
        {
            var rsa = new RSACryptoServiceProvider(2048);
                //rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(key);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                return rsaDeformatter;
        }

        private RSAPKCS1SignatureFormatter RSAFormatter(RSAParameters key)
        {
            var rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;
            rsa.ImportParameters(key);

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");
            return rsaFormatter;
        }
    }
}
