﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Modularity;
using Prism.Unity;

namespace TranslationSpeech
{
    public class Bootstrapper:UnityBootstrapper
    {
        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window) this.Shell;
            App.Current.MainWindow.Show();
        }
        protected override DependencyObject CreateShell()
        {
            return this.Container.TryResolve<Shell>();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
        }
    }
}
