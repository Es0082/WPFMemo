using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memo.ViewModels
{
    public class FileListViewModel
    {
        public string Path
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }

        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    return Directory
                            .EnumerateFiles(this.Path)
                            .Select(s => new FileViewModel(s));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #region inner classes

        public class FileViewModel
        {
            private readonly FileInfo fileInfo;

            public FileViewModel(string path)
            {
                this.fileInfo = new FileInfo(path);
            }

            public string Name
            {
                get
                {
                    return this.fileInfo.Name;
                }
            }

            public string LastUpdateDate
            {
                get
                {
                    return this.fileInfo.LastWriteTime.ToShortDateString();
                }
            }

            public string Type
            {
                get
                {
                    return this.fileInfo.Extension;
                }
            }

            public string Size
            {
                get
                {
                    return (this.fileInfo.Length / 1000).ToString("#,0K");
                }
            }
        }
    }

    #endregion
}
