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

namespace MediaCycler2.Controls
{
    /// <summary>
    /// Interaction logic for WebControl.xaml
    /// </summary>
    public partial class WebControl : AMediaContentControl
    {
        public WebControl()
        {
            InitializeComponent();
        }

        public override void Load(string path)
        {
            W.Navigate(new Uri(path));
        }
    }
}
