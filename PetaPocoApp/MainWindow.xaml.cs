using System;
using System.Windows;
using PetaPocoApp.Database;

namespace PetaPocoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowSpeciesListView(PetaPoco.IDatabase iDatabase, Database.ISpeciesManager iSpeciesManager)
        {
            View.SpeciesListView speciesListView = new View.SpeciesListView();
            ViewModel.SpeciesListViewModel speciesListViewModel = new ViewModel.SpeciesListViewModel(iSpeciesManager);
            speciesListView.DataContext = speciesListViewModel;
            speciesListViewModel.Load();
            speciesListView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            speciesListView.Owner = this;
            speciesListView.ShowDialog();
            speciesListView._datagrid.Columns[0].Visibility = Visibility.Collapsed;
            iDatabase.CloseSharedConnection();
        }

        private string _keyPath = System.Environment.Is64BitOperatingSystem ? @"SOFTWARE\Wow6432Node\PetaPocoApp\DatabaseSettings" : @"SOFTWARE\PetaPocoApp\DatabaseSettings";

        private bool OpenDatabase(out PetaPoco.IDatabase iDatabase, out Database.SpeciesManager speciesManager)
        {
            iDatabase = null;
            speciesManager = null;

            OpenControls.Wpf.Serialisation.RegistryItemSerialiser registryItemSerialiser = new OpenControls.Wpf.Serialisation.RegistryItemSerialiser(_keyPath);
            OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration databaseConfiguration = new OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration(registryItemSerialiser);
            if (registryItemSerialiser.OpenKey())
            {
                databaseConfiguration.Load();
            }

            OpenControls.Wpf.DatabaseDialogs.ViewModel.OpenDatabaseViewModel openDatabaseViewModel = new OpenControls.Wpf.DatabaseDialogs.ViewModel.OpenDatabaseViewModel(databaseConfiguration);
            OpenControls.Wpf.DatabaseDialogs.View.OpenDatabaseView openDatabaseView =
                new OpenControls.Wpf.DatabaseDialogs.View.OpenDatabaseView(new OpenControls.Wpf.DatabaseDialogs.Model.Encryption());
            openDatabaseView.DataContext = openDatabaseViewModel;
            if (openDatabaseView.ShowDialog() != true)
            {
                return false;
            }
            if (!registryItemSerialiser.IsOpen)
            {
                registryItemSerialiser.CreateKey();
            }
            databaseConfiguration.Save();
            registryItemSerialiser.Close();

            try
            {
                if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.SQLite)
                {
                    PetaPocoAdapter.SQLiteDatabaseFactory.OpenDatabase(out iDatabase, openDatabaseViewModel.SQLite_Filename);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MicrosoftSQLServer)
                {
                    PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.SQLServer_UseLocalServer,
                        openDatabaseViewModel.SelectedSqlServerInstance,
                        openDatabaseViewModel.SQLServer_IPAddress,
                        openDatabaseViewModel.SQLServer_Port,
                        openDatabaseViewModel.SQLServer_UseWindowsAuthentication,
                        openDatabaseViewModel.SQLServer_UserName,
                        openDatabaseViewModel.SQLServer_Password,
                        openDatabaseViewModel.SQLServer_DatabaseName);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.PostGreSQL)
                {
                    PetaPocoAdapter.PostgreSQLDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.PostgreSQL_IPAddress, 
                        openDatabaseViewModel.PostgreSQL_Port, 
                        openDatabaseViewModel.PostgreSQL_UseWindowsAuthentication, 
                        openDatabaseViewModel.PostgreSQL_UserName, 
                        openDatabaseViewModel.PostgreSQL_Password, 
                        openDatabaseViewModel.PostgreSQL_DatabaseName);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MySQL)
                {
                    PetaPocoAdapter.MySQLDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.MySQL_IPAddress, 
                        openDatabaseViewModel.MySQL_Port, 
                        openDatabaseViewModel.MySQL_UseWindowsAuthentication, 
                        openDatabaseViewModel.MySQL_UserName, 
                        openDatabaseViewModel.MySQL_Password, 
                        openDatabaseViewModel.MySQL_DatabaseName);
                }
                else
                {
                    throw new Exception("Unsupported database type");
                }
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message);
                return false;
            }

            speciesManager = PetaPocoAdapter.SpeciesManagerFactory.GetSpeciesManager(iDatabase);

            return true;
        }

        private void _buttonOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (OpenDatabase(out PetaPoco.IDatabase iDatabase, out Database.SpeciesManager speciesManager))
            {
                ShowSpeciesListView(iDatabase, speciesManager);
            }
        }

        private bool NewDatabase(out PetaPoco.IDatabase iDatabase, out Database.SpeciesManager speciesManager)
        {
            iDatabase = null;
            speciesManager = null;

            OpenControls.Wpf.Serialisation.RegistryItemSerialiser registryItemSerialiser = new OpenControls.Wpf.Serialisation.RegistryItemSerialiser(_keyPath);
            OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration databaseConfiguration = new OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration(registryItemSerialiser);
            if (registryItemSerialiser.OpenKey())
            {
                databaseConfiguration.Load();
            }

            OpenControls.Wpf.DatabaseDialogs.ViewModel.NewDatabaseViewModel newDatabaseViewModel = new OpenControls.Wpf.DatabaseDialogs.ViewModel.NewDatabaseViewModel(databaseConfiguration);
            OpenControls.Wpf.DatabaseDialogs.View.NewDatabaseView newDatabaseView =
                new OpenControls.Wpf.DatabaseDialogs.View.NewDatabaseView(new OpenControls.Wpf.DatabaseDialogs.Model.Encryption());
            newDatabaseView.DataContext = newDatabaseViewModel;
            if (newDatabaseView.ShowDialog() != true)
            {
                return false;
            }
            if (!registryItemSerialiser.IsOpen)
            {
                registryItemSerialiser.CreateKey();
            }
            databaseConfiguration.Save();
            registryItemSerialiser.Close();

            try
            {
                OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider databaseProvider = (OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider)newDatabaseViewModel.SelectedDatabaseProvider;
                if (databaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.SQLite)
                {
                    PetaPocoAdapter.SQLiteDatabaseFactory.CreateDatabase(
                        out iDatabase, newDatabaseViewModel.SQLite_Folder, 
                        newDatabaseViewModel.SQLite_DatabaseName);
                }
                else if (databaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MicrosoftSQLServer)
                {
                    PetaPocoAdapter.SQLServerDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.SelectedSqlServerInstance,
                        newDatabaseViewModel.SQLServer_UseWindowsAuthentication,
                        newDatabaseViewModel.SQLServer_UserName,
                        newDatabaseViewModel.SQLServer_Password,
                        newDatabaseViewModel.SQLServer_Folder,
                        newDatabaseViewModel.SQLServer_Filename);
                }
                else if (databaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.PostGreSQL)
                {
                    PetaPocoAdapter.PostgreSQLDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.PostgreSQL_IPAddress, 
                        newDatabaseViewModel.PostgreSQL_Port, 
                        newDatabaseViewModel.PostgreSQL_UseWindowsAuthentication, 
                        newDatabaseViewModel.PostgreSQL_UserName, 
                        newDatabaseViewModel.PostgreSQL_Password, 
                        newDatabaseViewModel.PostgreSQL_DatabaseName);
                }
                else if (databaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MySQL)
                {
                    PetaPocoAdapter.MySQLDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.MySQL_IPAddress, 
                        newDatabaseViewModel.MySQL_Port, 
                        newDatabaseViewModel.MySQL_UseWindowsAuthentication, 
                        newDatabaseViewModel.MySQL_UserName, 
                        newDatabaseViewModel.MySQL_Password, 
                        newDatabaseViewModel.MySQL_DatabaseName);
                }
                else
                {
                    throw new Exception("Unsupported database type");
                }

                speciesManager = PetaPocoAdapter.SpeciesManagerFactory.GetSpeciesManager(iDatabase);
                speciesManager.Initialise();
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message);
                return false;
            }

            return true;
        }

        private void _buttonNewDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (NewDatabase(out PetaPoco.IDatabase iDatabase, out PetaPocoApp.Database.SpeciesManager speciesManager))
            {
                ShowSpeciesListView(iDatabase, speciesManager);
            }
        }

        private void _buttonExportDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (!OpenDatabase(out PetaPoco.IDatabase iSourceDatabase, out PetaPocoApp.Database.SpeciesManager sourceSpeciesManager))
            {
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseSharedConnection();
                }
                return;
            }

            if (!NewDatabase(out PetaPoco.IDatabase iTargetDatabase, out PetaPocoApp.Database.SpeciesManager targetSpeciesManager))
            {
                if (iTargetDatabase != null)
                {
                    iTargetDatabase.CloseSharedConnection();
                }
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseSharedConnection();
                }
                return;
            }

            SpeciesManager.Export(iSourceDatabase, sourceSpeciesManager, iTargetDatabase, targetSpeciesManager);
        }
    }
}
