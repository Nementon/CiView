using CiView.SewerModel;
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

namespace ModelView
{//equaero
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.InsterRootItem(root);
            root.InsertChild(rootChild);
            root.InsertChild(rootChildChild);

            tree.Items.Add(root.ToString());
            TreeViewItem t = new TreeViewItem();
            t.Items.Add(rootChild.ToString());
            t.Items.Add(rootChildChild.ToString());
            tree.Items.Add(t);
           
            
        }
     
    }
}
