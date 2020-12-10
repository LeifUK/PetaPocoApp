using System.Windows;

namespace PetaPocoApp.View
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void _buttonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog == null)
            {
                return;
            }

            //dialog.SelectedPath = viewModel.BinaryLogFolder;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            ViewModel.ConfigurationViewModel configurationViewModel = DataContext as ViewModel.ConfigurationViewModel;
            configurationViewModel.AddImagePath(dialog.SelectedPath);
        }

        private void _buttonEditFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_listBoxFolders.SelectedItem == null)
            {
                return;
            }

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog == null)
            {
                return;
            }

            DBObject.ImagePath imagePath = _listBoxFolders.SelectedItem as DBObject.ImagePath;
            ViewModel.ConfigurationViewModel configurationViewModel = DataContext as ViewModel.ConfigurationViewModel;
            int index = configurationViewModel.ImagePaths.IndexOf(imagePath);

            dialog.SelectedPath = imagePath.path;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            imagePath.path = dialog.SelectedPath;
            configurationViewModel.UpdateImagePath(imagePath);

            configurationViewModel.ImagePaths.RemoveAt(index);
            configurationViewModel.ImagePaths.Insert(index, imagePath);
        }

        private void _buttonDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_listBoxFolders.SelectedItem != null)
            {
                ViewModel.ConfigurationViewModel configurationViewModel = DataContext as ViewModel.ConfigurationViewModel;
                configurationViewModel.DeleteImagePath(_listBoxFolders.SelectedItem as DBObject.ImagePath);
            }
        }
    }
}
