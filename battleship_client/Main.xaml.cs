using battleship_client.BattleshipServerRef;
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
using System.Windows.Shapes;

namespace battleship_client
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private BattleshipServiceClient client;


        public Main()
        {
            this.client = new BattleshipServiceClient();
            InitializeComponent();
            Navigate(new RoomsPage());
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
            this.MinHeight = nextPage.MinHeight + 40;
            this.MinWidth = nextPage.MinWidth + 20;
            this.Height = nextPage.MinHeight + 40;
            this.Width = nextPage.MinWidth + 20;
            LoginPage login = new LoginPage();
            ((RoomsPage)this.Content).grid.Children.Add(login);
            login.SetValue(Grid.RowSpanProperty, 2);
            //this.panel.Children.Add(nextPage);
            //cc.Content = nextPage;
            //this.Content = 
        }

        public BattleshipServiceClient Client
        {
            get
            {
                return client;
            }
        }
    }
}
