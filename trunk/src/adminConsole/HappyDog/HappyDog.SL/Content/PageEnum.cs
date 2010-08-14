using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HappyDog.SL.Content
{
    /// <summary>
    /// IMPORTANT: To ensure navigation system to work, name of this enum must be the same with individual page.
    /// For example, page "BeelunPage.xaml & BeelunPage.xaml.cs", its enum MUST be BeelunPage(case sensitive)
    /// </summary>
    public enum PageEnum
    {
        NONE,

        MainView,       // MainView.xaml
        LoginPage,      // LoginPage.xaml
        ProgressPage,   // ProgressForInitPage.xaml
        ListPage,       
        EmptyPage,      // EmptyPage.xaml

        DashBoardPage1, // DashBoardPage1.xaml
        DashBoardPage2, // DashBoardPage2.xaml

        ItemListPage,   // ItemListPage.xmal
        ItemDetailsPage, // ItemDetailsPage.xmal

        CategoryListPage,       //CategoryListPage.xmal
        CategoryDetailsPage,    //CategoryDetailPage.xmal

        TabListPage,       //TabListPage.xmal
        TabDetailsPage,    //TabDetailPage.xmal

        UserListPage,       //UserListPage.xmal
        UserDetailsPage,     //UserDetailPage.xmal

        OrderListPage,       //OrderListPage.xmal
        OrderDetailsPage,     //OrderDetailPage.xmal

        MediaListPage,
        MediaDetailsPage,

        GlobalPage,
        PaypalPage,

        ArticleListPage,
        ArticleDetailsPage,

        BrandListPage,
        BrandDetailsPage,

        TemplateListPage,
        TemplateDetailsPage

    }
}
