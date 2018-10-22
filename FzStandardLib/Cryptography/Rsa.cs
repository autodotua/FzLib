using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace FzLib.Cryptography
{
    public class Rsa : CryptographyBase
    {
        public static Rsa Create(int keySize=1024)
        {
            return new Rsa(keySize);
        }

        public static Rsa Create(RSAParameters parameters, int keySize = 1024)
        {
            Rsa rsa = new Rsa(keySize);
            rsa.rsaProvider.ImportParameters(parameters);
            return rsa;
        }

        public static Rsa Create(string key, int keySize = 1024)
        {
            Rsa rsa = new Rsa(keySize);
            rsa.ImportParametersFromXml(key);
            return rsa;
        }

        private Rsa(int keySize)
        {
            rsaProvider = new RSACryptoServiceProvider(keySize);
        }
        private RSACryptoServiceProvider rsaProvider;
        public RSAParameters PrivateParameters => rsaProvider.ExportParameters(true);
        public RSAParameters PublicParameters => rsaProvider.ExportParameters(false);
        public string PublicKey =>GetXmlStringInNotSupportPlatform(rsaProvider,false);// rsaProvider.ToXmlString(false);
        public string PrivateKey => GetXmlStringInNotSupportPlatform(rsaProvider, true);// rsaProvider.ToXmlString(true);
        public byte[] Encrypte(byte[] input)
        {
            try
            {
                var temp = PublicParameters;
            }
            catch (CryptographicException ex)
            {
                throw new Exception("公钥不存在", ex);
            }
            List<byte> encrypted = new List<byte>(input.Length);
            using (MemoryStream msInput = new MemoryStream(input))
            {
                int bufferSize = EncrypteBufferLength;
                byte[] buffer = new byte[bufferSize];
                int size;
                while ((size = msInput.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (size == bufferSize)
                    {
                        encrypted.AddRange(rsaProvider.Encrypt(buffer, UseOaep));
                    }
                    else
                    {
                        byte[] current = new byte[size];
                        Array.Copy(buffer, current, size);
                        encrypted.AddRange(rsaProvider.Encrypt(current, UseOaep));
                    }
                }

            }
            return encrypted.ToArray();
        }

        public byte[] Decrypte(byte[] input)
        {
            try
            {
                var temp = PrivateParameters;
            }
            catch(CryptographicException ex)
            {
                throw new Exception("私钥不存在",ex);
            }
            List<byte> decrypted = new List<byte>();
            MemoryStream msInput = new MemoryStream(input);
            int bufferSize = DecrypteBufferLength;
            byte[] buffer = new byte[bufferSize];
            int size;

            while ((size = msInput.Read(buffer, 0, bufferSize)) > 0)
            {
                if (size == bufferSize)
                {
                    decrypted.AddRange(rsaProvider.Decrypt(buffer, UseOaep));
                }
                else
                {
                    byte[] current = new byte[size];
                    Array.Copy(buffer, current, size);
                    decrypted.AddRange(rsaProvider.Decrypt(current, UseOaep));
                }

            }
            return decrypted.ToArray();
        }

        private void ImportParametersFromXml(string xmlString)
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
            rsaProvider.ImportParameters(parameters);
        }

        public int EncrypteBufferLength
        {
            get
            {
                if (UseOaep)
                {
                    return rsaProvider.KeySize / 8 - rsaProvider.KeySize / 16 - 2;
                }
                else
                {
                    return rsaProvider.KeySize / 8 - 11;
                }
            }
        }
        public int DecrypteBufferLength => rsaProvider.KeySize / 8;

        public new int BufferLength => throw new Exception();
        public bool UseOaep { get; set; } = false;

        #region 过去的
        //public static string ToPriavateXmlString(RSACryptoServiceProvider rsa)
        //{
        //    RSAParameters parameters = rsa.ExportParameters(true);

        //    return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
        //        Convert.ToBase64String(parameters.Modulus),
        //        Convert.ToBase64String(parameters.Exponent),
        //        Convert.ToBase64String(parameters.P),
        //        Convert.ToBase64String(parameters.Q),
        //        Convert.ToBase64String(parameters.DP),
        //        Convert.ToBase64String(parameters.DQ),
        //        Convert.ToBase64String(parameters.InverseQ),
        //        Convert.ToBase64String(parameters.D));
        //}
        //public static string ToPublicXmlString(RSACryptoServiceProvider rsa)
        //{
        //    RSAParameters parameters = rsa.ExportParameters(false);

        //    return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
        //        Convert.ToBase64String(parameters.Modulus),
        //        Convert.ToBase64String(parameters.Exponent));
        //}
        #endregion

        public override void Dispose()
        {
            rsaProvider.Dispose();
        }


        // We can provide a default implementation of ToXmlString because we require 
        // every RSA implementation to implement ImportParameters
        // If includePrivateParameters is false, this is just an XMLDSIG RSAKeyValue
        // clause.  If includePrivateParameters is true, then we extend RSAKeyValue with 
        // the other (private) elements.
        public  string GetXmlStringInNotSupportPlatform(RSACryptoServiceProvider provider, bool includePrivateParameters)
        {
            // From the XMLDSIG spec, RFC 3075, Section 6.4.2, an RSAKeyValue looks like this:
            /* 
               <element name="RSAKeyValue"> 
                 <complexType> 
                   <sequence>
                     <element name="Modulus" type="ds:CryptoBinary"/> 
                     <element name="Exponent" type="ds:CryptoBinary"/>
                   </sequence> 
                 </complexType> 
               </element>
            */
            // we extend appropriately for private components
            RSAParameters rsaParams = provider.ExportParameters(includePrivateParameters);
            StringBuilder sb = new StringBuilder();
            sb.Append("<RSAKeyValue>");
            // Add the modulus
            sb.Append("<Modulus>" + Convert.ToBase64String(rsaParams.Modulus) + "</Modulus>");
            // Add the exponent
            sb.Append("<Exponent>" + Convert.ToBase64String(rsaParams.Exponent) + "</Exponent>");
            if (includePrivateParameters)
            {
                // Add the private components
                sb.Append("<P>" + Convert.ToBase64String(rsaParams.P) + "</P>");
                sb.Append("<Q>" + Convert.ToBase64String(rsaParams.Q) + "</Q>");
                sb.Append("<DP>" + Convert.ToBase64String(rsaParams.DP) + "</DP>");
                sb.Append("<DQ>" + Convert.ToBase64String(rsaParams.DQ) + "</DQ>");
                sb.Append("<InverseQ>" + Convert.ToBase64String(rsaParams.InverseQ) + "</InverseQ>");
                sb.Append("<D>" + Convert.ToBase64String(rsaParams.D) + "</D>");
            }
            sb.Append("</RSAKeyValue>");
            return (sb.ToString());
        }

    }
}
