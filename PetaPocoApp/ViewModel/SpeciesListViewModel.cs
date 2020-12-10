using System.Linq;
using PetaPocoApp.Database;

namespace PetaPocoApp.ViewModel
{
    internal class SpeciesListViewModel : OpenControls.Wpf.DatabaseDialogs.ViewModel.BaseViewModel
    {
        public SpeciesListViewModel(ISpeciesManager iSpeciesManager)
        {
            ISpeciesManager = iSpeciesManager;

            SpeciesCollection = new System.Collections.ObjectModel.ObservableCollection<DBObject.Species>();
            Load();
        }

        public readonly ISpeciesManager ISpeciesManager;

        private System.Collections.ObjectModel.ObservableCollection<DBObject.Species> _speciesCollection;
        public System.Collections.ObjectModel.ObservableCollection<DBObject.Species> SpeciesCollection
        {
            get
            {
                return _speciesCollection;
            }
            set
            {
                _speciesCollection = value;
                NotifyPropertyChanged("SpeciesCollection");
            }
        }

        private DBObject.Species _selectedSpecies;
        public DBObject.Species SelectedSpecies
        {
            get
            {
                return _selectedSpecies;
            }
            set
            {
                _selectedSpecies = value;
                NotifyPropertyChanged("SelectedSpecies");
            }
        }

        public bool Load()
        {
            SpeciesCollection = new System.Collections.ObjectModel.ObservableCollection<DBObject.Species>(ISpeciesManager.SpeciesEnumerator);
            return true;
        }

        public void LoadImages(DBObject.Species species)
        {
            ISpeciesManager.LoadImages(species);
        }

        public void UpdateSpecies(DBObject.Species species, DBObject.Species editedSpecies)
        {
            ISpeciesManager.Update(species, editedSpecies);
            Load();
        }

        public void InsertSpecies(DBObject.Species species)
        {
            ISpeciesManager.Insert(species);
            Load();
            var enumerator = SpeciesCollection.Where(n => n.id == species.id);
            if ((enumerator != null) && (enumerator.Count() > 0))
            {
                SelectedSpecies = enumerator.First();
            }
        }

        public void DeleteSpecies(int index)
        {
            ISpeciesManager.Delete(SelectedSpecies);
            Load();
        }
    }
}
