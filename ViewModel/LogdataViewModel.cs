using LogParser.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogParser.ViewModel
{
    class LogdataViewModel : DependencyObject
    {


        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(LogdataViewModel), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as LogdataViewModel;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterLogdata;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(LogdataViewModel), new PropertyMetadata(null));

        //public LogdataViewModel(string FileName)
        //{
        //    //Items = CollectionViewSource.GetDefaultView(Logdata.GetDatafromFile(FileName));
        //    Items = CollectionViewSource.GetDefaultView(Logdata.GetDatafromDB());
        //    Items.Filter = FilterLogdata;
        //}

        public LogdataViewModel()
        {
            //Items = CollectionViewSource.GetDefaultView(Logdata.GetDatafromFile(FileName));
            Items = CollectionViewSource.GetDefaultView(Logdata.GetDatafromDB());
            Items.Filter = FilterLogdata;
        }

        private bool FilterLogdata(object obj)
        {
            bool result = true;
            Logdata current = obj as Logdata;
            if (!string.IsNullOrWhiteSpace(FilterText) &&
                current != null &&
                !current.Host.Contains(FilterText) &&
                !current.Time.Contains(FilterText) &&
                !current.Request.Contains(FilterText) &&
                !current.Status.Contains(FilterText) &&
                !current.Agent.Contains(FilterText))
            {
                result = false;
            }
            return result;
        }
    }
}
