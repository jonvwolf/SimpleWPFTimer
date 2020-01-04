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

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl
    {
        public enum Mode
        {
            Timer, Stopwatch
        }
        public ClockUserCtrl(Mode mode)
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
