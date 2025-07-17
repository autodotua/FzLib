using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace FzLib.Cryptography
{
    public static class RsaExtension
    {
        public static int GetEncryptBufferLength(this RSACryptoServiceProvider rsaProvider, bool useOaep)
        {
            if (useOaep)
            {
                return rsaProvider.KeySize / 8 - rsaProvider.KeySize / 16 - 2;
            }
            else
            {
                return rsaProvider.KeySize / 8 - 11;
            }
        }

        public static int GetDecryptBufferLength(this RSACryptoServiceProvider rsaProvider) => rsaProvider.KeySize / 8;

        public static RSACryptoServiceProvider CreateProvider(RSAParameters parameters, int keySize = 1024)
        {
            var rsa = Create(keySize);
            rsa.ImportParameters(parameters);
            return rsa;
        }

        public static RSACryptoServiceProvider Create(int keySize = 1024)
        {
            return new RSACryptoServiceProvider(keySize);
        }

        public static RSAParameters GetPrivateParameters(this RSACryptoServiceProvider rsaProvider) => rsaProvider.ExportParameters(true);

        public static RSAParameters GetPublicParameters(this RSACryptoServiceProvider rsaProvider) => rsaProvider.ExportParameters(false);

        public static byte[] EncryptLong(this RSACryptoServiceProvider rsaProvider, byte[] input, bool useOaep)
        {
            try
            {
                var temp = rsaProvider.GetPublicParameters();
            }
            catch (CryptographicException ex)
            {
                throw new Exception("公钥不存在", ex);
            }
            List<byte> encrypted = new List<byte>(input.Length);
            using (MemoryStream msInput = new MemoryStream(input))
            {
                int bufferSize = rsaProvider.GetEncryptBufferLength(useOaep);
                byte[] buffer = new byte[bufferSize];
                int size;
                while ((size = msInput.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (size == bufferSize)
                    {
                        encrypted.AddRange(rsaProvider.Encrypt(buffer, useOaep));
                    }
                    else
                    {
                        byte[] current = new byte[size];
                        Array.Copy(buffer, current, size);
                        encrypted.AddRange(rsaProvider.Encrypt(current, useOaep));
                    }
                }
            }
            return encrypted.ToArray();
        }

        public static byte[] DecryptLong(this RSACryptoServiceProvider rsaProvider, byte[] input, bool useOaep)
        {
            try
            {
                var temp = rsaProvider.GetPrivateParameters();
            }
            catch (CryptographicException ex)
            {
                throw new Exception("私钥不存在", ex);
            }
            List<byte> decrypted = new List<byte>();
            MemoryStream msInput = new MemoryStream(input);
            int bufferSize = rsaProvider.GetDecryptBufferLength();
            byte[] buffer = new byte[bufferSize];
            int size;

            while ((size = msInput.Read(buffer, 0, bufferSize)) > 0)
            {
                if (size == bufferSize)
                {
                    decrypted.AddRange(rsaProvider.Decrypt(buffer, useOaep));
                }
                else
                {
                    byte[] current = new byte[size];
                    Array.Copy(buffer, current, size);
                    decrypted.AddRange(rsaProvider.Decrypt(current, useOaep));
                }
            }
            return decrypted.ToArray();
        }


        public static string ToPemPublicKey(this RSACryptoServiceProvider provider)
        {
            return PemKeyUtils.ExportPublicKey(provider);
        }

        public static RSACryptoServiceProvider ImportPemKey(this RSACryptoServiceProvider provider, string pem)
        {
            var parameters = PemKeyUtils.GetParametersFromPemString(pem);
            if (parameters == null)
            {
                throw new FormatException("PEM格式不正确");
            }
            provider.ImportParameters(parameters.Value);
            return provider;
        }

        public static RSACryptoServiceProvider ImportXmlKey(this RSACryptoServiceProvider provider, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus":
                            parameters.Modulus = Convert.FromBase64String(node.InnerText);
                            break;

                        case "Exponent":
                            parameters.Exponent = Convert.FromBase64String(node.InnerText);
                            break;

                        case "P":
                            parameters.P = Convert.FromBase64String(node.InnerText);
                            break;

                        case "Q":
                            parameters.Q = Convert.FromBase64String(node.InnerText);
                            break;

                        case "DP":
                            parameters.DP = Convert.FromBase64String(node.InnerText);
                            break;

                        case "DQ":
                            parameters.DQ = Convert.FromBase64String(node.InnerText);
                            break;

                        case "InverseQ":
                            parameters.InverseQ = Convert.FromBase64String(node.InnerText);
                            break;

                        case "D":
                            parameters.D = Convert.FromBase64String(node.InnerText);
                            break;
                    }
                }
            }
            provider.ImportParameters(parameters);

            return provider;
        }
    }
}