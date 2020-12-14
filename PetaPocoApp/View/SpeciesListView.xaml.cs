using System.Windows;

namespace PetaPocoApp.View
{
    /// <summary>
    /// Interaction logic for SpeciesListView.xaml
    /// </summary>
    public partial class SpeciesListView : Window
    {
        public SpeciesListView()
        {
            InitializeComponent();
            _datagrid.IsReadOnly = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _datagrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void EditSelectedItem()
        {
            if (_datagrid.SelectedIndex < 0)
            {
                return;
            }

            int selectedIndex = _datagrid.SelectedIndex;
            ViewModel.SpeciesListViewModel speciesListViewModel = DataContext as ViewModel.SpeciesListViewModel;
            DBObject.Species species = speciesListViewModel.SpeciesCollection[_datagrid.SelectedIndex] as DBObject.Species;
            // Edit a clone of the species
            DBObject.Species editedSpecies = species.Clone();
            speciesListViewModel.LoadImages(species);
            speciesListViewModel.LoadImages(editedSpecies);

            View.SpeciesView speciesView = new SpeciesView();
            ViewModel.SpeciesViewModel speciesViewModel = new ViewModel.SpeciesViewModel(speciesListViewModel.ISpeciesManager, editedSpecies);
            speciesView.DataContext = speciesViewModel;
            speciesView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            speciesView.Owner = this;
            if (speciesView.ShowDialog() == false)
            {
                return;
            }

            speciesListViewModel.UpdateSpecies(species, editedSpecies);
            _datagrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void _buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedItem();
        }

        private void _buttonNew_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SpeciesListViewModel speciesListViewModel = DataContext as ViewModel.SpeciesListViewModel;

            var enumerator = speciesListViewModel.ISpeciesManager.ImagePathEnumerator.GetEnumerator();
            if (enumerator.MoveNext() == false)
            {
                System.Windows.MessageBox.Show("Cannot create species. Please edit the configuration and add one or more image folders.");
                return;
            }
            enumerator.Dispose();

            DBObject.Species species = new DBObject.Species();

            View.SpeciesView speciesView = new SpeciesView();
            ViewModel.SpeciesViewModel speciesViewModel = new ViewModel.SpeciesViewModel(speciesListViewModel.ISpeciesManager, species);
            speciesView.DataContext = speciesViewModel;
            speciesView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            speciesView.Owner = this;
            if (speciesView.ShowDialog() == false)
            {
                return;
            }

            // Save everything

            speciesListViewModel.InsertSpecies(species);
            _datagrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_datagrid.SelectedIndex < 0)
            {
                return;
            }

            int selectedIndex = _datagrid.SelectedIndex;

            ViewModel.SpeciesListViewModel speciesListViewModel = DataContext as ViewModel.SpeciesListViewModel;
            speciesListViewModel.DeleteSpecies(selectedIndex);

            _datagrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void _buttonConfigure_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SpeciesListViewModel speciesListViewModel = DataContext as ViewModel.SpeciesListViewModel;
            View.ConfigurationView configurationView = new ConfigurationView();
            ViewModel.ConfigurationViewModel configurationViewModel = new ViewModel.ConfigurationViewModel(speciesListViewModel.ISpeciesManager);
            configurationView.DataContext = configurationViewModel;
            configurationView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            configurationView.Owner = this;
            configurationView.ShowDialog();
        }

        private void _buttonCloseDB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void _datagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedItem();
        }

        private void _datagrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key < System.Windows.Input.Key.A) || (e.Key > System.Windows.Input.Key.Z))
            {
                return;
            }

            char matchChar = (char)(e.Key + 21);
            ViewModel.SpeciesListViewModel speciesListViewModel = DataContext as ViewModel.SpeciesListViewModel;
            foreach (var species in speciesListViewModel.SpeciesCollection)
            {
                if (species.species[0] == matchChar)
                {
                    speciesListViewModel.SelectedSpecies = species;
                    _datagrid.ScrollIntoView(species);
                    return;
                }
            }
        }
    }
}
