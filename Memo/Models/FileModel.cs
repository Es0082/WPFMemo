using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Memo.Models
{
    //! ファイル情報クラス
    public class FileModel
    {
        public ReactiveProperty<string> Path = new ReactiveProperty<string>();
        public ReactiveProperty<string> Name = new ReactiveProperty<string>();
        public ReactiveProperty<string> Text = new ReactiveProperty<string>();

        public FileModel()
        {
            Path.Value = "";
            Name.Value = "";
            Text.Value = "";
        }
    }
}
