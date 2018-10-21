using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FzLib.Cryptography
{
    public class Hash: CryptographyBase
    {

        public byte[] GetArray(string hashName, byte[] bytes)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array;
            using (var hash = HashAlgorithm.Create(hashName))
            {
                array = hash.ComputeHash(bytes);
            }
            return array;
        }

        public async Task<byte[]> GetArrayAsync(string hashName, byte[] bytes)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array=null;
          await  Task.Run(() =>
            {
                using (var hash = HashAlgorithm.Create(hashName))
                {
                    array = hash.ComputeHash(bytes);
                }
            });
            return array;
        }
        public byte[] GetArray(string hashName, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array;
            using (var hash = HashAlgorithm.Create(hashName))
            {
                array = hash.ComputeHash(stream);
            }
            return array;
        }
        public async Task<byte[]> GetArrayAsync(string hashName, Stream stream)
        {
            if (stoping)
            {
                stoping = false;
            }
            byte[] array=null; await Task.Run(() =>
            {
                using (var hash = HashAlgorithm.Create(hashName))
                {
                    array = hash.ComputeHash(stream);
                }
            });
            return array;
        }
        public byte[][] GetArray(IEnumerable<string> hashNames, Stream stream, string separator = "", string format = "X2")
        {
            if(stoping)
            {
                stoping = false;
            }
            HashAlgorithm[] hashes = hashNames.Select(p => HashAlgorithm.Create(p)).ToArray();
            foreach (var hash in hashes)
            {
                if (hash == null)
                {
                    throw new Exception("不存在" + hash);
                }
            }
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
            byte[][] results = hashes.Select(p => p.Hash).ToArray();
            foreach (var hash in hashes)
            {
                hash.Dispose();
            }
            return results;
        }
        public async Task<byte[][]> GetArrayAsync(IEnumerable<string> hashNames, Stream stream, string separator = "", string format = "X2")
        {
            if (stoping)
            {
                stoping = false;
            }
            HashAlgorithm[] hashes = hashNames.Select(p => HashAlgorithm.Create(p)).ToArray();
            foreach (var hash in hashes)
            {
                if (hash == null)
                {
                    throw new Exception("不存在" + hash);
                }
            }
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
                        return ;
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

        public string[] GetString(IEnumerable<string> hashNames, Stream stream, string separator = "", string format = "X2")
        {
            byte[][] array = GetArray(hashNames, stream);

            return array.Select(p => string.Join(separator, p.Select(q => q.ToString(format)))).ToArray();
        }
        public async Task<string[]> GetStringAsync(IEnumerable<string> hashNames, Stream stream, string separator = "", string format = "X2")
        {
            byte[][] array = null;
            await Task.Run(() => array = GetArray(hashNames, stream));

            return array?.Select(p => string.Join(separator, p.Select(q => q.ToString(format)))).ToArray();
        }
        public string GetString(string hashName, Stream stream, string separator = "", string format = "X2")
        {
            byte[] array = GetArray(hashName, stream);

            return string.Join(separator, array.Select(p => p.ToString(format)));
        }
        public async Task<string> GetStringAsync(string hashName, Stream stream, string separator = "", string format = "X2")
        {
            byte[] array = await GetArrayAsync(hashName, stream);

            return string.Join(separator, array.Select(p => p.ToString(format)));
        }

        public string GetStringFromFile(string hashName, string filePath, string separator = "", string format = "X2")
        {
            byte[] array;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                array = GetArray(hashName, stream);
            }

            return string.Join(separator, array.Select(p => p.ToString(format)));
        }
        public async Task<string> GetStringFromFileAsync(string hashName, string filePath, string separator = "", string format = "X2")
        {
            byte[] array=null;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                array = await GetArrayAsync(hashName, stream);
            }

            return string.Join(separator, array.Select(p => p.ToString(format)));
        }

        public string GetString(string hashName, string content, string separator = "", string format = "X2")
        {
            byte[] array = GetArray(hashName, StringEncoding.GetBytes(content));
            return string.Join(separator, array.Select(p => p.ToString(format)));
        }

        public async Task<string> GetStringAsync(string hashName, string content, string separator = "", string format = "X2")
        {
            byte[] array =await GetArrayAsync(hashName, StringEncoding.GetBytes(content));
            return string.Join(separator, array.Select(p => p.ToString(format)));
        }

        public string[] GetStringFromFile(IEnumerable<string> hashNames, string filePath, string separator = "", string format = "X2")
        {
            FileStream stream = File.Open(filePath, FileMode.Open);
            var result = GetString(hashNames, stream, separator, format);
            stream.Dispose();
            return result;

        }

        public async Task<string[]> GetStringFromFileAsync(IEnumerable<string> hashNames, string filePath, string separator = "", string format = "X2")
        {
            FileStream stream = File.Open(filePath, FileMode.Open);
            var result =await GetStringAsync(hashNames, stream, separator, format);
            stream.Dispose();
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

        public override void Dispose()
        {
        }

    }
}
