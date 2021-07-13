using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Cryptography
{
    public class Hash
    {
        public byte[] GetArray(Hashs hashName, byte[] bytes)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array = hashName.Compute(bytes);
            return array;
        }

        public async Task<byte[]> GetArrayAsync(Hashs hashName, byte[] bytes)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array = null;
            await Task.Run(() =>
             {
                 array = hashName.Compute(bytes);
             });
            return array;
        }

        public byte[] GetArray(Hashs hashName, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array = hashName.Compute(stream);

            return array;
        }

        public async Task<byte[]> GetArrayAsync(Hashs hashName, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array = null;
            await Task.Run(() =>
              {
                  array = hashName.Compute(stream);
              });
            return array;
        }

        public Dictionary<Hashs, byte[]> GetArray(IEnumerable<Hashs> hashNames, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            if (hashNames.Count() != hashNames.Distinct().Count())
            {
                throw new ArgumentException("存在重复的哈希名");
            }
            var hashNameArray = hashNames.ToArray();
            HashAlgorithm[] hashes = hashNames.Select(p => p.Create()).ToArray();

            int hashCount = hashes.Length;

            byte[] buffer = new byte[BufferLength];
            long[] offsets = new long[hashCount];

            long totalLength = stream.Length;
            int length = 0;
            while ((length = stream.Read(buffer, 0, BufferLength)) != 0)
            {
                if (stoping)
                {
                    foreach (var hash in hashes)
                    {
                        hash.Dispose();
                    }
                    HashAborted?.Invoke(this, new EventArgs());
                    return null;
                }
                if (stream.Position < totalLength)
                {
                    Parallel.For(0, hashCount, i =>
                    {
                        offsets[i] += hashes[i].TransformBlock(buffer, 0, length, buffer, 0);
                    });
                }
                else
                {
                    Parallel.For(0, hashCount, i =>
                    {
                        hashes[i].TransformFinalBlock(buffer, 0, length);
                    });
                }
            }
            Dictionary<Hashs, byte[]> results = new Dictionary<Hashs, byte[]>();
            for (int i = 0; i < hashNameArray.Length; i++)
            {
                results.Add(hashNameArray[i], hashes[i].Hash);
            }
            foreach (var hash in hashes)
            {
                hash.Dispose();
            }
            return results;
        }

        public async Task<byte[][]> GetArrayAsync(IEnumerable<Hashs> hashNames, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            HashAlgorithm[] hashes = hashNames.Select(p => p.Create()).ToArray();
            int hashCount = hashes.Length;

            byte[] buffer = new byte[BufferLength];
            long[] offsets = new long[hashCount];

            long totalLength = stream.Length;
            int length = 0;
            byte[][] results = null;
            await Task.Run(() =>
            {
                while ((length = stream.Read(buffer, 0, BufferLength)) != 0)
                {
                    if (stoping)
                    {
                        foreach (var hash in hashes)
                        {
                            hash.Dispose();
                        }
                        HashAborted?.Invoke(this, new EventArgs());
                        return;
                    }
                    if (stream.Position < totalLength)
                    {
                        Parallel.For(0, hashCount, i =>
                        {
                            offsets[i] += hashes[i].TransformBlock(buffer, 0, length, buffer, 0);
                        });
                    }
                    else
                    {
                        Parallel.For(0, hashCount, i =>
                        {
                            hashes[i].TransformFinalBlock(buffer, 0, length);
                        });
                    }
                }
                results = hashes.Select(p => p.Hash).ToArray();
                foreach (var hash in hashes)
                {
                    hash.Dispose();
                }
            });
            return results;
        }

        public Dictionary<Hashs, string> GetString(IEnumerable<Hashs> hashNames, Stream stream)
        {
            var array = GetArray(hashNames, stream);

            return array.ToDictionary(p => p.Key, p => Bytes2String(p.Value));
        }

        public async Task<Dictionary<Hashs, string>> GetStringAsync(IEnumerable<Hashs> hashNames, Stream stream)
        {
            Dictionary<Hashs, byte[]> array = null;
            await Task.Run(() => array = GetArray(hashNames, stream));

            return array?.ToDictionary(p => p.Key, p => Bytes2String(p.Value));
        }

        public string GetString(Hashs hashName, Stream stream)
        {
            byte[] array = GetArray(hashName, stream);

            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public async Task<string> GetStringAsync(Hashs hashName, Stream stream)
        {
            byte[] array = await GetArrayAsync(hashName, stream);

            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public string GetStringFromFile(Hashs hashName, string filePath)
        {
            byte[] array;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                array = GetArray(hashName, stream);
            }

            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public async Task<string> GetStringFromFileAsync(Hashs hashName, string filePath)
        {
            byte[] array = null;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                array = await GetArrayAsync(hashName, stream);
            }

            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public string GetString(Hashs hashName, string content)
        {
            byte[] array = GetArray(hashName, StringEncoding.GetBytes(content));
            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public async Task<string> GetStringAsync(Hashs hashName, string content)
        {
            byte[] array = await GetArrayAsync(hashName, StringEncoding.GetBytes(content));
            return string.Join(HexStringSeparator, array.Select(p => p.ToString(HexStringFormat)));
        }

        public Dictionary<Hashs, string> GetStringFromFile(IEnumerable<Hashs> hashNames, string filePath)
        {
            Dictionary<Hashs, string> result = null;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                result = GetString(hashNames, stream);
            }
            return result;
        }

        public async Task<Dictionary<Hashs, string>> GetStringFromFileAsync(IEnumerable<Hashs> hashNames, string filePath)
        {
            Dictionary<Hashs, string> result = null;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                result = await GetStringAsync(hashNames, stream);
            }
            return result;
        }

        private bool stoping = false;

        public void TryStop()
        {
            stoping = true;
        }

        public static bool IsNameExist(string hashName)
        {
            var hash = HashAlgorithm.Create(hashName);
            bool result = hash != null;
            hash?.Dispose();
            return result;
        }

        public event EventHandler HashAborted;

        public Encoding StringEncoding { get; set; } = Encoding.UTF8;

        public int BufferLength { get; set; } = 1024 * 1024;
        public string HexStringFormat { get; set; } = "X2";
        public string HexStringSeparator { get; set; } = "";

        private string Bytes2String(byte[] bytes)
        {
            return string.Join(HexStringSeparator, bytes.Select(q => q.ToString(HexStringFormat)));
        }
    }

    internal static class HashExtension
    {
        public static HashAlgorithm Create(this Hashs hash)
        {
            return HashAlgorithm.Create(hash.ToString());
        }

        public static byte[] Compute(this Hashs hash, byte[] array)
        {
            using (var h = hash.Create())
            {
                return h.ComputeHash(array);
            }
        }

        public static byte[] Compute(this Hashs hash, Stream array)
        {
            using (var h = hash.Create())
            {
                return h.ComputeHash(array);
            }
        }
    }

    public enum Hashs
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}