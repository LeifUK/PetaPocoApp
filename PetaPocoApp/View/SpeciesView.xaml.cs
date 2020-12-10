using System.Windows;

namespace PetaPocoApp.View
{
    /// <summary>
    /// Interaction logic for SpeciesView.xaml
    /// </summary>
    public partial class SpeciesView : Window
    {
        public SpeciesView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void _buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "JPG File (*.jpg)|*.jpg|GIF File (*.gif)|*.gif| Bitmap File(*.bmp)| *.bmp";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == true)
            {
                DBObject.Image image = new DBObject.Image();
                image.Path = dialog.FileName;
                ViewModel.SpeciesViewModel speciesViewModel = DataContext as ViewModel.SpeciesViewModel;

                if (!speciesViewModel.ISpeciesManager.ValidateImagePath(image))
                {
                    System.Windows.MessageBox.Show("Cannot add image as it is not in an image folder. Please edit the configuration.");
                    return;
                }

                speciesViewModel.AddImage(image);
            }
        }

        private void _buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (_tabControl.SelectedIndex < 0)
            {
                return;
            }

            ViewModel.SpeciesViewModel speciesViewModel = DataContext as ViewModel.SpeciesViewModel;
            speciesViewModel.RemoveImage(speciesViewModel.Images[_tabControl.SelectedIndex]);
        }

        private void MoveImage(int fromIndex, int toIndex)
        {
            ViewModel.SpeciesViewModel speciesViewModel = DataContext as ViewModel.SpeciesViewModel;
            speciesViewModel.MoveImage(fromIndex, toIndex);

            _tabControl.SelectedIndex = toIndex;
        }

        private void _buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (_tabControl.SelectedIndex < 0)
            {
                return;
            }

            MoveImage(_tabControl.SelectedIndex, 0);
        }

        private void _buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_tabControl.SelectedIndex < 1)
            {
                return;
            }

            MoveImage(_tabControl.SelectedIndex, _tabControl.SelectedIndex - 1);
        }

        private void _buttonRight_Click(object sender, RoutedEventArgs e)
        {
            if ((_tabControl.SelectedIndex < 0) || (_tabControl.SelectedIndex == (_tabControl.Items.Count - 1)))
            {
                return;
            }

            MoveImage(_tabControl.SelectedIndex, _tabControl.SelectedIndex + 1);
        }

        private void _buttonEnd_Click(object sender, RoutedEventArgs e)
        {
            if ((_tabControl.SelectedIndex < 0) || (_tabControl.SelectedIndex == (_tabControl.Items.Count - 1)))
            {
                return;
            }

            MoveImage(_tabControl.SelectedIndex, _tabControl.Items.Count - 1);
        }
    }
}
