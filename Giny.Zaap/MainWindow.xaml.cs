using Giny.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Giny.Zaap
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ClientPath
        {
            get;
            set;
        }
        public MainWindow()
        {
            InitializeComponent();
            ClientPath = ConfigurationManager.AppSettings["clientPath"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dofusPath = Path.Combine(ClientPath, ClientConstants.ExePath);

            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = dofusPath;
            ps.Arguments = "--port=3000 --gameName=dofus --gameRelease=main --instanceId=1 --hash=89e700ea-7d27-40c9-b51e-16ef248507d --canLogin=true";
            Process process = new Process();
            process.StartInfo = ps;
            process.Start();

        }
    }
}
