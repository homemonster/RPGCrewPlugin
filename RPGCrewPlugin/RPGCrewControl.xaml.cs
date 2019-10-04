using System;
using System.Collections.Generic;
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

namespace RPGCrewPlugin
{
    /// <summary>
    /// Логика взаимодействия для RPGCrewControl.xaml
    /// </summary>
    public partial class RPGCrewControl : UserControl
    {
        private RPGCrewPlugin Plugin { get; }
        private RPGCrewControl() => InitializeComponent();
        public RPGCrewControl(RPGCrewPlugin plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Plugin.Save();
        }
    }
}
