using SoftwareAccountingService.Wpf.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoftwareAccountingService.Wpf.Presentation.Views
{
    /// <summary>
    /// Логика взаимодействия для InspectionObjectFormPage.xaml
    /// </summary>
    public partial class InspectionObjectFormPage : Page
    {
        public InspectionObjectFormPage(InspectionObjectFormViewModel VM)
        {
            InitializeComponent();

            DataContext = VM;
        }
    }
}
