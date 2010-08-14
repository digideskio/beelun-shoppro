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
    public partial class DashBoardPage2 : Page
    {
        public DashBoardPage2()
        {
            InitializeComponent();

            DateTime startTime = DateTime.Now.AddDays(-7);

            WSEntryManagerClient wsClient = Globals.NewWSClient;

            wsClient.getNewUserTrendCompleted += (s, e) =>
            {
                ColumnSeries cs = TestColumnChart.Series[0] as ColumnSeries;
                cs.ItemsSource = e.Result;
            };
            wsClient.getNewUserTrendAsync(startTime, HappyDog.SL.Beelun.Shoppro.WSEntryManager.timeAccuracyEnum.DAY, 7);

        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }

}
