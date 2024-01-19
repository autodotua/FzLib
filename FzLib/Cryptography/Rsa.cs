using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
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

    internal class PemKeyUtils
    {
        private const string pemprivheader = "-----BEGIN RSA PRIVATE KEY-----";
        private const string pemprivfooter = "-----END RSA PRIVATE KEY-----";
        private const string pempubheader = "-----BEGIN PUBLIC KEY-----";
        private const string pempubfooter = "-----END PUBLIC KEY-----";
        private const string pemp8header = "-----BEGIN PRIVATE KEY-----";
        private const string pemp8footer = "-----END PRIVATE KEY-----";
        private const string pemp8encheader = "-----BEGIN ENCRYPTED PRIVATE KEY-----";
        private const string pemp8encfooter = "-----END ENCRYPTED PRIVATE KEY-----";

        private static bool verbose = false;

        public static RSAParameters? GetParametersFromPemString(string pemstr)
        {
            bool isPrivateKeyFile = true;

            if (pemstr.StartsWith(pempubheader) && pemstr.EndsWith(pempubfooter))
                isPrivateKeyFile = false;

            byte[] pemkey;
            if (isPrivateKeyFile)
                pemkey = DecodeOpenSSLPrivateKey(pemstr);
            else
                pemkey = DecodeOpenSSLPublicKey(pemstr);

            if (pemkey == null)
                return null;

            if (isPrivateKeyFile)
                return DecodeRSAPrivateKey(pemkey);
            else
                return DecodeX509PublicKey(pemkey);
        }

        //--------   Get the binary RSA PUBLIC key   --------
        private static byte[] DecodeOpenSSLPublicKey(string instr)
        {
            const string pempubheader = "-----BEGIN PUBLIC KEY-----";
            const string pempubfooter = "-----END PUBLIC KEY-----";
            string pemstr = instr.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pempubheader) || !pemstr.EndsWith(pempubfooter))
                return null;
            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pempubheader, "");  //remove headers/footers, if present
            sb.Replace(pempubfooter, "");

            string pubstr = sb.ToString().Trim();   //get string after removing leading/trailing whitespace

            try
            {
                binkey = Convert.FromBase64String(pubstr);
            }
            catch (System.FormatException)
            {       //if can't b64 decode, data is not valid
                return null;
            }
            return binkey;
        }

        private static RSAParameters? DecodeX509PublicKey(byte[] x509Key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (var mem = new MemoryStream(x509Key))
            {
                using (var binr = new BinaryReader(mem))    //wrap Memory Stream with BinaryReader for easy reading
                {
                    try
                    {
                        var twobytes = binr.ReadUInt16();
                        switch (twobytes)
                        {
                            case 0x8130:
                                binr.ReadByte();    //advance 1 byte
                                break;

                            case 0x8230:
                                binr.ReadInt16();   //advance 2 bytes
                                break;

                            default:
                                return null;
                        }

                        var seq = binr.ReadBytes(15);
                        if (!CompareBytearrays(seq, seqOid))  //make sure Sequence for OID is correct
                            return null;

                        twobytes = binr.ReadUInt16();
                        if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit string is 03 81)
                            binr.ReadByte();    //advance 1 byte
                        else if (twobytes == 0x8203)
                            binr.ReadInt16();   //advance 2 bytes
                        else
                            return null;

                        var bt = binr.ReadByte();
                        if (bt != 0x00)     //expect null byte next
                            return null;

                        twobytes = binr.ReadUInt16();
                        if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                            binr.ReadByte();    //advance 1 byte
                        else if (twobytes == 0x8230)
                            binr.ReadInt16();   //advance 2 bytes
                        else
                            return null;

                        twobytes = binr.ReadUInt16();
                        byte lowbyte = 0x00;
                        byte highbyte = 0x00;

                        if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                            lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                        else if (twobytes == 0x8202)
                        {
                            highbyte = binr.ReadByte(); //advance 2 bytes
                            lowbyte = binr.ReadByte();
                        }
                        else
                            return null;
                        byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                        int modsize = BitConverter.ToInt32(modint, 0);

                        byte firstbyte = binr.ReadByte();
                        binr.BaseStream.Seek(-1, SeekOrigin.Current);

                        if (firstbyte == 0x00)
                        {   //if first byte (highest order) of modulus is zero, don't include it
                            binr.ReadByte();    //skip this null byte
                            modsize -= 1;   //reduce modulus buffer size by 1
                        }

                        byte[] modulus = binr.ReadBytes(modsize); //read the modulus bytes

                        if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                            return null;
                        int expbytes = binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                        byte[] exponent = binr.ReadBytes(expbytes);

                        // We don't really need to print anything but if we insist to...
                        //showBytes("\nExponent", exponent);
                        //showBytes("\nModulus", modulus);

                        // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                        RSAParameters rsaKeyInfo = new RSAParameters
                        {
                            Modulus = modulus,
                            Exponent = exponent
                        };
                        return rsaKeyInfo;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }

        //------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
        private static RSAParameters? DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");
                if (verbose)
                {
                    showBytes("\nModulus", MODULUS);
                    showBytes("\nExponent", E);
                    showBytes("\nD", D);
                    showBytes("\nP", P);
                    showBytes("\nQ", Q);
                    showBytes("\nDP", DP);
                    showBytes("\nDQ", DQ);
                    showBytes("\nIQ", IQ);
                }

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                return RSAparams;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)     //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);     //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        //-----  Get the binary RSA PRIVATE key, decrypting if necessary ----
        private static byte[] DecodeOpenSSLPrivateKey(string instr)
        {
            const string pemprivheader = "-----BEGIN RSA PRIVATE KEY-----";
            const string pemprivfooter = "-----END RSA PRIVATE KEY-----";
            string pemstr = instr.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pemprivheader) || !pemstr.EndsWith(pemprivfooter))
                return null;

            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pemprivheader, "");  //remove headers/footers, if present
            sb.Replace(pemprivfooter, "");

            string pvkstr = sb.ToString().Trim();   //get string after removing leading/trailing whitespace

            try
            {        // if there are no PEM encryption info lines, this is an UNencrypted PEM private key
                binkey = Convert.FromBase64String(pvkstr);
                return binkey;
            }
            catch (System.FormatException)
            {       //if can't b64 decode, it must be an encrypted private key
                    //Console.WriteLine("Not an unencrypted OpenSSL PEM private key");
            }

            StringReader str = new StringReader(pvkstr);

            //-------- read PEM encryption info. lines and extract salt -----
            if (!str.ReadLine().StartsWith("Proc-Type: 4,ENCRYPTED"))
                return null;
            string saltline = str.ReadLine();
            if (!saltline.StartsWith("DEK-Info: DES-EDE3-CBC,"))
                return null;
            string saltstr = saltline.Substring(saltline.IndexOf(",") + 1).Trim();
            byte[] salt = new byte[saltstr.Length / 2];
            for (int i = 0; i < salt.Length; i++)
                salt[i] = Convert.ToByte(saltstr.Substring(i * 2, 2), 16);
            if (!(str.ReadLine() == ""))
                return null;

            //------ remaining b64 data is encrypted RSA key ----
            string encryptedstr = str.ReadToEnd();

            try
            {   //should have b64 encrypted RSA key now
                binkey = Convert.FromBase64String(encryptedstr);
            }
            catch (System.FormatException)
            {  // bad b64 data.
                return null;
            }

            //------ Get the 3DES 24 byte key using PDK used by OpenSSL ----

            SecureString despswd = GetSecPswd("Enter password to derive 3DES key==>");
            //Console.Write("\nEnter password to derive 3DES key: ");
            //String pswd = Console.ReadLine();
            byte[] deskey = GetOpenSSL3deskey(salt, despswd, 1, 2);    // count=1 (for OpenSSL implementation); 2 iterations to get at least 24 bytes
            if (deskey == null)
                return null;
            //showBytes("3DES key", deskey) ;

            //------ Decrypt the encrypted 3des-encrypted RSA private key ------
            byte[] rsakey = DecryptKey(binkey, deskey, salt); //OpenSSL uses salt value in PEM header also as 3DES IV
            if (rsakey != null)
                return rsakey;  //we have a decrypted RSA private key
            else
            {
                Console.WriteLine("Failed to decrypt RSA private key; probably wrong password.");
                return null;
            }
        }

        // ----- Decrypt the 3DES encrypted RSA private key ----------
        private static byte[] DecryptKey(byte[] cipherData, byte[] desKey, byte[] IV)
        {
            MemoryStream memst = new MemoryStream();
            TripleDES alg = TripleDES.Create();
            alg.Key = desKey;
            alg.IV = IV;
            try
            {
                CryptoStream cs = new CryptoStream(memst, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
            byte[] decryptedData = memst.ToArray();
            return decryptedData;
        }

        //-----   OpenSSL PBKD uses only one hash cycle (count); miter is number of iterations required to build sufficient bytes ---
        private static byte[] GetOpenSSL3deskey(byte[] salt, SecureString secpswd, int count, int miter)
        {
            IntPtr unmanagedPswd = IntPtr.Zero;
            int HASHLENGTH = 16;    //MD5 bytes
            byte[] keymaterial = new byte[HASHLENGTH * miter];     //to store contatenated Mi hashed results

            byte[] psbytes = new byte[secpswd.Length];
            unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

            //UTF8Encoding utf8 = new UTF8Encoding();
            //byte[] psbytes = utf8.GetBytes(pswd);

            // --- contatenate salt and pswd bytes into fixed data array ---
            byte[] data00 = new byte[psbytes.Length + salt.Length];
            Array.Copy(psbytes, data00, psbytes.Length);      //copy the pswd bytes
            Array.Copy(salt, 0, data00, psbytes.Length, salt.Length); //concatenate the salt bytes

            // ---- do multi-hashing and contatenate results  D1, D2 ...  into keymaterial bytes ----
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = null;
            byte[] hashtarget = new byte[HASHLENGTH + data00.Length];   //fixed length initial hashtarget

            for (int j = 0; j < miter; j++)
            {
                // ----  Now hash consecutively for count times ------
                if (j == 0)
                    result = data00;    //initialize
                else
                {
                    Array.Copy(result, hashtarget, result.Length);
                    Array.Copy(data00, 0, hashtarget, result.Length, data00.Length);
                    result = hashtarget;
                    //Console.WriteLine("Updated new initial hash target:") ;
                    //showBytes(result) ;
                }

                for (int i = 0; i < count; i++)
                    result = md5.ComputeHash(result);
                Array.Copy(result, 0, keymaterial, j * HASHLENGTH, result.Length);  //contatenate to keymaterial
            }
            //showBytes("Final key material", keymaterial);
            byte[] deskey = new byte[24];
            Array.Copy(keymaterial, deskey, deskey.Length);

            Array.Clear(psbytes, 0, psbytes.Length);
            Array.Clear(data00, 0, data00.Length);
            Array.Clear(result, 0, result.Length);
            Array.Clear(hashtarget, 0, hashtarget.Length);
            Array.Clear(keymaterial, 0, keymaterial.Length);

            return deskey;
        }

        private static SecureString GetSecPswd(string prompt)
        {
            SecureString password = new SecureString();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.Magenta;

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    return password;
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    // remove the last asterisk from the screen...
                    if (password.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        password.RemoveAt(password.Length - 1);
                    }
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    return password;
                }
                else if (Char.IsLetterOrDigit(cki.KeyChar) || Char.IsSymbol(cki.KeyChar))
                {
                    if (password.Length < 20)
                    {
                        password.AppendChar(cki.KeyChar);
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else
                {
                    Console.Beep();
                }
            }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static void showBytes(string info, byte[] data)
        {
            Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
            for (int i = 1; i <= data.Length; i++)
            {
                Console.Write("{0:X2}  ", data[i - 1]);
                if (i % 16 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Export public key from MS RSACryptoServiceProvider into OpenSSH PEM string
        /// slightly modified from https://stackoverflow.com/a/28407693
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public static string ExportPublicKey(RSACryptoServiceProvider csp)
        {
            StringWriter outputStream = new StringWriter();
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write("-----BEGIN PUBLIC KEY-----\n");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write("-----END PUBLIC KEY-----");
            }

            return outputStream.ToString();
        }

        // https://stackoverflow.com/a/23739932/2860309
        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        //https://stackoverflow.com/a/23739932/2860309
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
    }

    [Obsolete]
    public class Rsa : CryptographyBase
    {
        public static Rsa Create(int keySize = 1024)
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
        public string PublicKey => GetXmlStringInNotSupportPlatform(rsaProvider, false);// rsaProvider.ToXmlString(false);
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
            catch (CryptographicException ex)
            {
                throw new Exception("私钥不存在", ex);
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

        #endregion 过去的

        public override void Dispose()
        {
            rsaProvider.Dispose();
        }

        // We can provide a default implementation of ToXmlString because we require
        // every RSA implementation to implement ImportParameters
        // If includePrivateParameters is false, this is just an XMLDSIG RSAKeyValue
        // clause.  If includePrivateParameters is true, then we extend RSAKeyValue with
        // the other (private) elements.
        public string GetXmlStringInNotSupportPlatform(RSACryptoServiceProvider provider, bool includePrivateParameters)
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