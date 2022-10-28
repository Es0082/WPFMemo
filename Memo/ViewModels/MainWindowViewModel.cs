using System;
using Microsoft.Win32;
using Reactive.Bindings;
using System.IO;
using MSAPI = Microsoft.WindowsAPICodePack;
using Memo.Models;
using System.Windows.Threading;

namespace Memo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    //! コマンド
    /* 新規ファイル作成 */    public ReactiveCommand NewFileCommand { get; }
    /* ファイル読み込み */    public ReactiveCommand LoadCommand { get; }
    /* ファイル保存 */        public ReactiveCommand SaveCommand { get; }
    /* ファイル上書き */      public ReactiveCommand AddSaveCommand { get; }


    //! ファイル情報
    /* ファイル名 */          public ReactiveProperty<string> FileName { get; set; } = new ReactiveProperty<string>();
    /* ファイルパス */        public ReactiveProperty<string> FilePath { get; set; } = new ReactiveProperty<string>();
    /* ファイルテキスト */    public ReactiveProperty<string> FileText { get; set; } = new ReactiveProperty<string>();


    public MainWindowViewModel(FileModel fileModel) : base()
    {
        //! ファイルモデルの情報受け取り
        FileName = fileModel.Name.ToReactiveProperty<string>();
        FilePath = fileModel.Path.ToReactiveProperty<string>();
        FileText = fileModel.Text.ToReactiveProperty<string>();

        //! 変更通知
        fileModel.Path.Subscribe(t =>
        {
            FilePath.Value = t;
        });
        fileModel.Path.Subscribe(t =>
        {
            FilePath.Value = t;
        });
        fileModel.Path.Subscribe(t =>
        {
            FilePath.Value = t;
        });

        //! 新規ファイル作成コマンド
        this.NewFileCommand = new ReactiveCommand()
            .WithSubscribe(() =>
            {
                //! 情報初期化
                FilePath.Value = "";
                FileText.Value = "";

                //! モデル情報変更
                fileModel.Path.Value = FilePath.Value;
                fileModel.Text.Value = FileText.Value;
            });

        //! ファイル読み込みコマンド
        this.LoadCommand = new ReactiveCommand()
           .WithSubscribe(() =>
           {
               //! ファイルダイアログ表示
               OpenFileDialog ofd = new OpenFileDialog();
               ofd.FileName = "";
               ofd.DefaultExt = "*.*";

               //! ファイル選択したなら
               if (ofd.ShowDialog() == true)
               {
                   //! ファイル情報取得
                   FilePath.Value = ofd.FileName;
                   FileText.Value = File.ReadAllText(ofd.FileName);

                   //! モデル情報変更
                   fileModel.Path.Value = FilePath.Value;
                   fileModel.Text.Value = FileText.Value;
               }
           });

        //! ファイル保存コマンド
        this.SaveCommand = new ReactiveCommand()
           .WithSubscribe(() =>
           {
               //! ファイル保存ダイアログ表示
               var dlg = new MSAPI::Dialogs.CommonSaveFileDialog();

               //! 保存するかどうか
               if (dlg.ShowDialog() == MSAPI::Dialogs.CommonFileDialogResult.Ok)
               {
                   //! ファイル情報設定
                   FilePath.Value = dlg.FileName;
                   File.WriteAllText(FilePath.Value, FileText.Value);

                   //! モデル情報変更
                   fileModel.Path.Value = FilePath.Value;
                   fileModel.Text.Value = FileText.Value;

                   DoEvents();
               }
           });

        //! ファイル上書きコマンド
        this.AddSaveCommand = new ReactiveCommand()
           .WithSubscribe(() =>
           {
               //! ファイルパスがあるか（ファイル選択中か）
               if (FilePath.Value != "")
               {
                   //! ファイル書き込み
                   File.WriteAllText(FilePath.Value, FileText.Value);
               }
           });
    }
    private void DoEvents()
    {
        DispatcherFrame frame = new DispatcherFrame();
        var callback = new DispatcherOperationCallback(obj =>
        {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        });
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
        Dispatcher.PushFrame(frame);
    }
}