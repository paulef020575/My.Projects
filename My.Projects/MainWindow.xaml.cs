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
using EPV.Data.Queries;
using My.Projects.Classes;
using My.Projects.Client;

namespace My.Projects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            ApiConnector apiConnector = new ApiConnector("https://localhost:44381/API");

            // 4ee7bd43-2905-4f6c-8223-65b781b98943

        }
    }
}
