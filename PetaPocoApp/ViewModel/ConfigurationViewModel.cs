using PetaPocoApp.DBObject;

namespace PetaPocoApp.ViewModel
{
    class ConfigurationViewModel : OpenControls.Wpf.DatabaseDialogs.ViewModel.BaseViewModel
    {
        public ConfigurationViewModel(PetaPocoApp.Database.ISpeciesManager iSpeciesManager)
        {
            ISpeciesManager = iSpeciesManager;

            ImagePaths = new System.Collections.ObjectModel.ObservableCollection<ImagePath>(ISpeciesManager.ImagePathEnumerator);
        }

        private readonly PetaPocoApp.Database.ISpeciesManager ISpeciesManager;

        private System.Collections.ObjectModel.ObservableCollection<DBObject.ImagePath> _imagePaths;
        public System.Collections.ObjectModel.ObservableCollection<DBObject.ImagePath> ImagePaths
        {
            get
            {
                return _imagePaths;
            }
            set
            {
                _imagePaths = value;
                NotifyPropertyChanged("ImagePaths");
            }
        }

        public void UpdateImagePath(ImagePath imagePath)
        {
            ISpeciesManager.Update(imagePath);
        }

        public void AddImagePath(string path)
        {
            DBObject.ImagePath imagePath = new ImagePath();
            imagePath.path = path;
            ISpeciesManager.Insert(imagePath);
            ImagePaths.Add(imagePath);
        }

        public void DeleteImagePath(ImagePath imagePath)
        {
            if (!ISpeciesManager.Delete(imagePath))
            {
                System.Windows.Forms.MessageBox.Show("Unable to remove the folder: it is referenced by one or more images");
                return;
            }

            ImagePaths.Remove(imagePath);
        }
    }
}
