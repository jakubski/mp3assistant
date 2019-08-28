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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MP3Assistant
{
    /// <summary>
    /// Interaction logic for FileExplorerPage.xaml
    /// </summary>
    public partial class FileExplorerPage : Page
    {
        public FileExplorerPage()
        {
            InitializeComponent();

            Loaded += (sender, e) => { SetUpColumns(); };
            //SetUpColumns();
        }

        public FileExplorerPage(ViewModel dataContext)
            : this()
        {
            DataContext = dataContext;
        }

        private void SetUpColumns()
        {
            var viewModel = DataContext as MainPageViewModel;

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

        private void FileExplorerDirectoryItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((MainPageViewModel)DataContext).ItemDoubleClickCommand.Execute(((ListViewItem)sender).DataContext);
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
