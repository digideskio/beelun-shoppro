using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Windows.Controls.DataVisualization.Charting;

namespace HappyDog.SL.Views
{
    public partial class DashBoardPage1 : Page
    {
        public DashBoardPage1()
        {
            InitializeComponent();

            DateTime startTime = DateTime.Now.AddDays(-7);

            WSEntryManagerClient wsClient = Globals.NewWSClient;

            wsClient.getSalesByBrandCompleted += (s, e) =>
            {
                PieSeries ps = BrandSalesPieChart.Series[0] as PieSeries;
                ps.ItemsSource = e.Result;
            };
            wsClient.getSalesByBrandAsync(startTime, HappyDog.SL.Beelun.Shoppro.WSEntryManager.timeAccuracyEnum.DAY, 7);
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }

}
