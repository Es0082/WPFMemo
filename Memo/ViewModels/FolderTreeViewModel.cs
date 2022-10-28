using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Reactive.Bindings;
using Memo.Models;

namespace Memo.ViewModels
{
    //! フォルダ一覧表示（TreeView）クラス
    /// <summary>
    /// 
    /// </summary>
    public class FolderTreeViewModel
    {
        //! ツリー開始位置
        static private string rootPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        //! モデル情報受け取り用
        private FileModel fileModel1 = null;

        //! コンストラクタ
        public FolderTreeViewModel(FileModel fileModel)
        {
            //! 情報受け取り
            fileModel1 = fileModel;

            //! 変更通知
            fileModel.Path.Subscribe(t =>
            {
                fileModel1.Path.Value = t;
               
            });
            fileModel.Text.Subscribe(t =>
            {
                fileModel1.Text.Value = t;
               
            });
            fileModel1.Path.Subscribe(t =>
            {
                fileModel.Path.Value = t;
               
            });
            fileModel1.Text.Subscribe(t =>
            {
                fileModel.Text.Value = t;
               
            });
        }

        //! ツリー情報作成
        public IEnumerable<TreeNode> TreeContent
        {
            get
            {
                //! ツリーの始まり指定
                yield return new TreeNode(@rootPath, fileModel1.Path,fileModel1.Text);
                //yield return new TreeNode(@"C:\\Users\\SatoKen\\Desktop\\WPF",fileModel1.Path,fileModel1.Text);
                //return DriveInfo.GetDrives().Select(d => new TreeNode(d.Name, fileModel1.Path, fileModel1.Text));
            }
        }

        #region inner classes

        //! ツリーノードクラス
        public class TreeNode
        {
            /* フォルダかどうか */    private bool isDirectory;
            /* 選択されたか */        private ReactiveProperty<bool> isSelected = new ReactiveProperty<bool>();
            /* フォルダ情報 */        private readonly DirectoryInfo dirInfo;
            /* ファイル情報 */        private readonly FileInfo fileInfo;

            /* モデルのファイルパス */        ReactiveProperty<string> FilePath = new ReactiveProperty<string>();
            /* モデルのファイルテキスト */    ReactiveProperty<string> FileText = new ReactiveProperty<string>();

            //! コンストラクタ
            public TreeNode(string rootpath,ReactiveProperty<string> path, ReactiveProperty<string> text)
            {
                //! ファイルかフォルダか
                if (File.GetAttributes(rootpath) == FileAttributes.Directory || rootpath == rootPath)
                {
                    isDirectory = true;
                    this.dirInfo = new DirectoryInfo(rootpath);
                }
                else
                {
                    isDirectory = false;
                    this.fileInfo = new FileInfo(rootpath);
                }
               
                //! モデル情報受け取り
                FilePath = path;
                FileText = text;
            }

            //! 選択されたとき
            public bool IsSelected
            {
                get
                {
                    //! 情報取得
                    return this.isSelected.Value;
                }

                set
                {
                    //! 選択された時
                    if (this.isSelected.Value != value)
                    {
                        this.isSelected.Value = value;

                        //! ファイルなら読み込み
                        if (!isDirectory)
                        {
                            //! ファイルが存在するかどうか
                            if (File.Exists(fileInfo.FullName))
                            {
                                //! 情報書き換え 
                                FilePath.Value = fileInfo.FullName;
                                FileText.Value = File.ReadAllText(fileInfo.FullName);
                            }
                        }
                    }
                }
            }

            //! 表示名
            public string Label
            {
                get
                {
                    if(isDirectory == true)
                        return this.dirInfo.Name;
                    else
                        return this.fileInfo.Name;
                }
            }

            //! フォルダの下の階層作成
            public IEnumerable<TreeNode> Children
            {
                get
                {
                    try
                    {
                        if (isDirectory)
                        {
                            //! フォルダの情報列挙
                            var directories =
                                Directory
                               .EnumerateDirectories(this.dirInfo.FullName)
                               .Select(s => new TreeNode(s, FilePath, FileText));

                            //! ファイルの情報列挙
                            var files =
                                Directory
                               .EnumerateFiles(this.dirInfo.FullName)
                               .Select(s => new TreeNode(s, FilePath, FileText));

                            //! リスト結合
                            var list = directories.Concat(files);
                            return list;
                        }
                        else
                        {
                           return null;
                        }
                    }
                    catch (Exception)
                    {
                        // folder permission is handled by catching exception
                        return null;
                    }
                }
            }
        }
        #endregion
    }
}
