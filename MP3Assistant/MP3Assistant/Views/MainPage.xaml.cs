using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace MP3Assistant
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            FileExplorerViewModel viewModel = new FileExplorerViewModel();

            DataContext = viewModel;

            // Set up columns
            GridView gridView = FileExplorerListView.View as GridView;

            foreach (var columnViewModel in viewModel.Columns)
            {
                gridView.Columns.Add(new GridViewColumn()
                {
                    Header = columnViewModel.Header,
                    Width = columnViewModel.Width,
                    DisplayMemberBinding = new Binding()
                    {
                        Converter = columnViewModel.Converter,
                        Path = new PropertyPath(columnViewModel.BoundProperty)
                    }
                });
            }

            viewModel.ColumnAdded += AddColumn;
            viewModel.ColumnRemoved += RemoveColumn;
        }

        private void FilePresenterDirectoryItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((FileExplorerViewModel)DataContext).ItemDoubleClickCommand.Execute(((ListViewItem)sender).DataContext);
        }

        private void AddColumn(object sender, ColumnChangedEventArgs e)
        {
            GridView gridView = FileExplorerListView.View as GridView;

            gridView.Columns.Add(new GridViewColumn()
            {
                Header = e.Column.Header,
                Width = e.Column.Width,
                DisplayMemberBinding = new Binding()
                {
                    Converter = e.Column.Converter,
                    Path = new PropertyPath(e.Column.BoundProperty)
                }
            });
        }

        private void RemoveColumn(object sender, ColumnChangedEventArgs e)
        {
            GridView gridView = FileExplorerListView.View as GridView;
            GridViewColumn columnToBeDeleted = gridView.Columns.Single(col => ((Binding)col.DisplayMemberBinding).Path.Path == e.Column.BoundProperty);

            gridView.Columns.Remove(columnToBeDeleted);
        }
    }
}
