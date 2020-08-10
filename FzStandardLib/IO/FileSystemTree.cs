using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FzLib.IO
{
    public class FileSystemTree : IReadOnlyList<FileSystemTree>
    {
        private FileSystemTree(string path, FileSystemTree parent)
        {
            Path = path;
            Parent = parent;
        }
        public string Path { get; private set; }


        public string[] Files { get; private set; }
        public FileSystemTree[] Directories { get; private set; }

        public DirectoryInfo[] GetDirectoriyInfoDictionary()
        {
            if (Directories == null || Directories.Length == 0)
            {
                return Array.Empty<DirectoryInfo>();
            }

            return Directories.Select(p => new DirectoryInfo(p.Path)).ToArray();
        }
        public async Task<DirectoryInfo[]> GetDirectoryInfoDictionaryAsync()
        {
            DirectoryInfo[] infos = null;
            await Task.Run(() => infos = GetDirectoriyInfoDictionary());
            return infos;
        }

        public FileInfo[] GetFileInfoDictionary()
        {
            if (Files == null || Files.Length == 0)
            {
                return new FileInfo[0];
            }

            return Files.Select(p => new FileInfo(p)).ToArray();
        }
        public async Task<FileInfo[]> GetFileInfoDictionaryAsync()
        {
            FileInfo[] infos = null;
            await Task.Run(() => infos = GetFileInfoDictionary());
            return infos;
        }


        public bool CanAccessChildren { get; private set; } = true;

        public bool? IsEmpty
        {
            get
            {
                if (!CanAccessChildren)
                {
                    return null;
                }

                if ((Files == null || Files.Length == 0) && (Directories == null || Directories.Length == 0))
                {
                    return true;
                }
                return false;
            }
        }

        public int Count { get; }

        public FileSystemTree this[int index] => Directories[index];

        public async static Task<FileSystemTree> GetFileSystemTreeAsync(string path)
        {
            FileSystemTree tree = null;
            await Task.Run(() => tree = GetFileSystemTree(path));
            return tree;
        }

        public static FileSystemTree GetFileSystemTree(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("目录" + path + "不存在");
            }
            var tree = new FileSystemTree(path, null);
            EnumerateRecursively(tree);
            return tree;
        }

        private static void EnumerateRecursively(FileSystemTree parent)
        {
            try
            {
                parent.Files = Directory.EnumerateFiles(parent.Path).ToArray();
                parent.Directories = Directory.EnumerateDirectories(parent.Path).Select(p => new FileSystemTree(p, parent)).ToArray();
                foreach (var child in parent.Directories)
                {
                    EnumerateRecursively(child);
                }
            }
            catch
            {
                parent.CanAccessChildren = false;
            }
        }

        public string[] GetAllFiles()
        {
            List<string> files = new List<string>();
            Get(this);
            return files.ToArray();

            void Get(FileSystemTree tree)
            {
                if (tree.Files != null && tree.Files.Length > 0)
                {
                    files.AddRange(tree.Files);
                }
                if (tree.Directories != null && tree.Directories.Length > 0)
                {
                    foreach (var subTree in tree)
                    {
                        Get(subTree);
                    }
                }
            }
        }

        public FileSystemTree Parent { get; private set; }
        public IEnumerator<FileSystemTree> GetEnumerator() => new FileSystemTreeEnumerator(Directories);
        IEnumerator IEnumerable.GetEnumerator() => new FileSystemTreeEnumerator(Directories);


        public class FileSystemTreeEnumerator : IEnumerator<FileSystemTree>
        {
            public FileSystemTreeEnumerator(FileSystemTree[] directories)
            {
                Directories = directories;
            }
            int currentIndex = -1;
            public FileSystemTree[] Directories { get; private set; }
            public FileSystemTree Current => Directories[currentIndex];

            object IEnumerator.Current => Current as object;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < Directories.Length;
            }
            public void Reset() => currentIndex = -1;
            public void Dispose() { }
        }


        public static FileComparisonResult CompareFiles(FileSystemTree left, FileSystemTree right)
        {
            List<(string, string)> same = new List<(string, string)>();
            List<(string, string)> leftNew = new List<(string, string)>();
            List<(string, string)> rightNew = new List<(string, string)>();
            List<string> leftIsolated = new List<string>();
            List<string> rightIsolated = new List<string>();

            CompareDirectories(left, right);

            return new FileComparisonResult(same.ToArray(), leftNew.ToArray(), rightNew.ToArray(), leftIsolated.ToArray(), rightIsolated.ToArray());


            void CompareFiles(FileSystemTree leftSubTree, FileSystemTree rightSubTree)
            {
                Dictionary<string, string> leftFileNameAndFullNames = new Dictionary<string, string>();
                foreach (string leftFile in leftSubTree.Files)
                {
                    leftFileNameAndFullNames.Add(System.IO.Path.GetFileName(leftFile), leftFile);
                }

                Dictionary<string, string> rightFileNameAndFullNames = new Dictionary<string, string>();
                foreach (string rightFile in rightSubTree.Files)
                {
                    rightFileNameAndFullNames.Add(System.IO.Path.GetFileName(rightFile), rightFile);
                }

                foreach (string fileName in leftFileNameAndFullNames.Keys.ToArray())
                {
                    if (rightFileNameAndFullNames.ContainsKey(fileName))//存在同一个文件名的文件
                    {
                        DateTime leftTime = File.GetLastWriteTime(leftFileNameAndFullNames[fileName]);
                        DateTime rightTime = File.GetLastWriteTime(rightFileNameAndFullNames[fileName]);
                        if (leftTime == rightTime)//修改时间相同
                        {
                            same.Add((leftFileNameAndFullNames[fileName], rightFileNameAndFullNames[fileName]));
                        }
                        else if (leftTime > rightTime)//修改时间左侧更晚
                        {
                            leftNew.Add((leftFileNameAndFullNames[fileName], rightFileNameAndFullNames[fileName]));
                        }
                        else//修改时间右侧更晚
                        {
                            rightNew.Add((leftFileNameAndFullNames[fileName], rightFileNameAndFullNames[fileName]));
                        }
                        leftFileNameAndFullNames.Remove(fileName);
                        rightFileNameAndFullNames.Remove(fileName);
                    }
                }

                leftIsolated.AddRange(leftFileNameAndFullNames.Values);
                rightIsolated.AddRange(rightFileNameAndFullNames.Values);
            }

            void CompareDirectories(FileSystemTree leftSubTree, FileSystemTree rightSubTree)
            {
                CompareFiles(leftSubTree, rightSubTree);

                Dictionary<string, FileSystemTree> leftFolderNameAndFullNames = new Dictionary<string, FileSystemTree>();
                foreach (var leftFolder in leftSubTree)
                {
                    leftFolderNameAndFullNames.Add(System.IO.Path.GetFileName(leftFolder.Path), leftFolder);
                }
                Dictionary<string, FileSystemTree> rightFolderNameAndFullNames = new Dictionary<string, FileSystemTree>();
                foreach (var rightFolder in rightSubTree)
                {
                    rightFolderNameAndFullNames.Add(System.IO.Path.GetFileName(rightFolder.Path), rightFolder);
                }



                foreach (string folderName in leftFolderNameAndFullNames.Keys.ToArray())
                {
                    if (rightFolderNameAndFullNames.ContainsKey(folderName))//存在同一个文件名的文件
                    {
                        CompareDirectories(leftFolderNameAndFullNames[folderName], rightFolderNameAndFullNames[folderName]);
                        leftFolderNameAndFullNames.Remove(folderName);
                        rightFolderNameAndFullNames.Remove(folderName);
                    }
                }
                foreach (var tree in leftFolderNameAndFullNames.Values)
                {
                    leftIsolated.AddRange(tree.GetAllFiles());
                }
                foreach (var tree in rightFolderNameAndFullNames.Values)
                {
                    rightIsolated.AddRange(tree.GetAllFiles());
                }
            }


        }
        public class FileComparisonResult
        {
            public FileComparisonResult((string left, string right)[] sameFiles, (string left, string right)[] leftNewFiles, (string left, string right)[] rightNewFiles, string[] leftIsolatedFiles, string[] rightIsolatedFiles)
            {
                SameFiles = sameFiles ?? throw new ArgumentNullException(nameof(sameFiles));
                LeftNewFiles = leftNewFiles ?? throw new ArgumentNullException(nameof(leftNewFiles));
                RightNewFiles = rightNewFiles ?? throw new ArgumentNullException(nameof(rightNewFiles));
                LeftIsolatedFiles = leftIsolatedFiles ?? throw new ArgumentNullException(nameof(leftIsolatedFiles));
                RightIsolatedFiles = rightIsolatedFiles ?? throw new ArgumentNullException(nameof(rightIsolatedFiles));
            }

            public (string left, string right)[] SameFiles { get; internal set; }
            public (string left, string right)[] LeftNewFiles { get; internal set; }
            public (string left, string right)[] RightNewFiles { get; internal set; }
            public string[] LeftIsolatedFiles { get; internal set; }
            public string[] RightIsolatedFiles { get; internal set; }
        }
    }
}
