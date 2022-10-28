using Memo.Models;
using Reactive.Bindings;
using System;


namespace Memo.ViewModels
{
    //! ステータスバー表示クラス
    public class StatusBarViewModel
    {
        /* ファイルパス */    public ReactiveProperty<string> FilePath_s { get; set; } = new ReactiveProperty<string>();
        /* 文字数 */          public ReactiveProperty<int> TexLength { get; set; } = new ReactiveProperty<int>();

        //! コンストラクタ
        public StatusBarViewModel(FileModel fileModel)
        {
            //! モデル情報受け取り
            FilePath_s.Value = fileModel.Path.Value;
            TexLength.Value = fileModel.Text.Value.Length;

            //! 変更通知
            fileModel.Path.Subscribe(t =>
            {
                FilePath_s.Value = fileModel.Path.Value;
            });
            fileModel.Text.Subscribe(t =>
            {
                TexLength.Value = t.Length;
            });
        }
    }
}
