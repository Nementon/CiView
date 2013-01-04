﻿using ActivityLogger.SewerModel;
using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
using ModelView.ViewModel;


namespace ModelView
{//equaero
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BagItems _bag;
        public MainWindow()
        {
            _bag = new BagItems();
            DataContext = new ActivityLoggerViewModel(_bag);
            
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void LogLevel_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LogLevel1_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        } 
    }
}
