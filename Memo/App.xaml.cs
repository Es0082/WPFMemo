using System.Windows;
using Prism.Unity;
using Prism.Ioc;
using Prism.Mvvm;
using Memo.ViewModels;
using Memo.Views;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
using Memo.Models;

namespace Memo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            //! ビュー情報登録
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //! モデルをコンテナに格納
            containerRegistry.RegisterSingleton<FileModel>();
            base.ConfigureViewModelLocator();
        }
    }
}
