using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PetaPocoApp.ViewModel
{
    internal class SpeciesViewModel : OpenControls.Wpf.DatabaseDialogs.ViewModel.BaseViewModel
    {
        public SpeciesViewModel(Database.ISpeciesManager iSpeciesManager, DBObject.Species species)
        {
            ISpeciesManager = iSpeciesManager;
            Species = species;
            Images = new ObservableCollection<DBObject.Image>(species.Images);
        }

        public readonly Database.ISpeciesManager ISpeciesManager;

        private ObservableCollection<KeyValuePair<string,string>> _speciesFields;
        public ObservableCollection<KeyValuePair<string, string>> SpeciesFields
        {
            get
            {
                return _speciesFields;
            }
            set
            {
                _speciesFields = value;
                NotifyPropertyChanged("SpeciesFields");
            }
        }

        private ObservableCollection<DBObject.Image> _images;
        public ObservableCollection<DBObject.Image> Images
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
                NotifyPropertyChanged("Images");
            }
        }

        public readonly DBObject.Species Species;

        public void AddImage(DBObject.Image image)
        {
            image.copyright = ISpeciesManager.Copyright;
            Images.Add(image);
            Species.Images.Add(image);
        }
         
        public void RemoveImage(DBObject.Image image)
        {
            Images.Remove(image);
            Species.Images.Remove(image);
        }

        public void MoveImage(int fromIndex, int toIndex)
        {
            DBObject.Image image = Images[fromIndex];
            Images.RemoveAt(fromIndex);
            Species.Images.RemoveAt(fromIndex);
            Images.Insert(toIndex, image);
            Species.Images.Insert(toIndex, image);
        }

        public string SpeciesName
        { 
            get
            {
                return Species.species;
            }
            set
            {
                Species.species = value;
                NotifyPropertyChanged("SpeciesName");
            }
        }

        public string Synonyms
        {
            get
            {
                return Species.synonyms;
            }
            set
            {
                Species.synonyms = value;
                NotifyPropertyChanged("Synonyms");
            }
        }

        public string Common_name
        {
            get
            {
                return Species.common_name;
            }
            set
            {
                Species.common_name = value;
                NotifyPropertyChanged("Common_name");
            }
        }

        public string Fruiting_body
        {
            get
            {
                return Species.fruiting_body;
            }
            set
            {
                Species.fruiting_body = value;
                NotifyPropertyChanged("Fruiting_body");
            }
        }

        public string Cap
        {
            get
            {
                return Species.cap;
            }
            set
            {
                Species.cap = value;
                NotifyPropertyChanged("Cap");
            }
        }

        public string Hymenium
        {
            get
            {
                return Species.hymenium;
            }
            set
            {
                Species.hymenium = value;
                NotifyPropertyChanged("Hymenium");
            }
        }

        public string Gills
        {
            get
            {
                return Species.gills;
            }
            set
            {
                Species.gills = value;
                NotifyPropertyChanged("Gills");
            }
        }

        public string Pores
        {
            get
            {
                return Species.pores;
            }
            set
            {
                Species.pores = value;
                NotifyPropertyChanged("Pores");
            }
        }

        public string Spines
        {
            get
            {
                return Species.spines;
            }
            set
            {
                Species.spines = value;
                NotifyPropertyChanged("Spines");
            }
        }

        public string Stem
        {
            get
            {
                return Species.stem;
            }
            set
            {
                Species.stem = value;
                NotifyPropertyChanged("Stem");
            }
        }

        public string Flesh
        {
            get
            {
                return Species.flesh;
            }
            set
            {
                Species.flesh = value;
                NotifyPropertyChanged("Flesh");
            }
        }

        public string Smell
        {
            get
            {
                return Species.smell;
            }
            set
            {
                Species.smell = value;
                NotifyPropertyChanged("Smell");
            }
        }

        public string Taste
        {
            get
            {
                return Species.taste;
            }
            set
            {
                Species.taste = value;
                NotifyPropertyChanged("Taste");
            }
        }

        public string Season
        {
            get
            {
                return Species.season;
            }
            set
            {
                Species.season = value;
                NotifyPropertyChanged("Season");
            }
        }

        public string Distribution
        {
            get
            {
                return Species.distribution;
            }
            set
            {
                Species.distribution = value;
                NotifyPropertyChanged("Distribution");
            }
        }

        public string Habitat
        {
            get
            {
                return Species.habitat;
            }
            set
            {
                Species.habitat = value;
                NotifyPropertyChanged("Habitat");
            }
        }

        public string Spore_print
        {
            get
            {
                return Species.spore_print;
            }
            set
            {
                Species.spore_print = value;
                NotifyPropertyChanged("Spore_print");
            }
        }

        public string Microscopic_features
        {
            get
            {
                return Species.microscopic_features;
            }
            set
            {
                Species.microscopic_features = value;
                NotifyPropertyChanged("Microscopic_features");
            }
        }

        public string Edibility
        {
            get
            {
                return Species.edibility;
            }
            set
            {
                Species.edibility = value;
                NotifyPropertyChanged("Edibility");
            }
        }

        public string Notes
        {
            get
            {
                return Species.notes;
            }
            set
            {
                Species.notes = value;
                NotifyPropertyChanged("Notes");
            }
        }
    }
}
