using System;
using System.Collections.Generic;
using System.Linq;

namespace PetaPocoApp.Database
{
    internal class SpeciesManager : ISpeciesManager
    {
        public SpeciesManager(
            IDatabase iDatabase, 
            IConfigurationStore iConfigurationStore,
            ISpeciesStore iSpeciesStore,
            IImagePathsStore iImagePathsStore,
            IImageStore iImageStore)
        {
            IDatabase = iDatabase;
            IConfigurationStore = iConfigurationStore;
            ISpeciesStore = iSpeciesStore;
            IImagePathsStore = iImagePathsStore;
            IImageStore = iImageStore;
        }

        private readonly IDatabase IDatabase;
        private readonly IConfigurationStore IConfigurationStore;
        private readonly ISpeciesStore ISpeciesStore;
        private readonly IImagePathsStore IImagePathsStore;
        private readonly IImageStore IImageStore;

        public void Initialise()
        {
            IConfigurationStore.Initialise();
        }

        private static bool ParseImagePath(IImagePathsStore iImagePathsStore, List<DBObject.Image> images)
        {
            Dictionary<long, string> paths = iImagePathsStore.LoadImagePaths();

            foreach (var image in images)
            {
                foreach (KeyValuePair<long, string> keyValuePair in paths)
                {
                    if (image.Path.Contains(keyValuePair.Value))
                    {
                        image.image_database_id = keyValuePair.Key;
                        image.filename = image.Path.Substring(keyValuePair.Value.Length);
                        break;
                    }
                }
            }

            return false;
        }

        public bool ValidateImagePath(DBObject.Image image)
        {
            System.Diagnostics.Trace.Assert(image != null);
            
            Dictionary<long, string> paths = IImagePathsStore.LoadImagePaths();
            foreach (KeyValuePair<long, string> keyValuePair in paths)
            {
                if (image.Path.Contains(keyValuePair.Value))
                {
                    return true;
                }
            }

            return false;
        }

        private void WriteImages(DBObject.Species species)
        {
            byte displayOrder = 0;
            ParseImagePath(IImagePathsStore, species.Images);

            foreach (DBObject.Image image in species.Images)
            {
                image.display_order = displayOrder;
                ++displayOrder;
                image.fungus_id = species.id;
                if (image.id == 0)
                {
                    IImageStore.Insert(image);
                }
                else
                {
                    IImageStore.Update(image);
                }
            }
        }

        public string Copyright
        {
            get
            {
                return IConfigurationStore.Copyright;
            }
            set
            {
                IConfigurationStore.Copyright = value;
            }
        }

        public string ExportFolder
        {
            get
            {
                return IConfigurationStore.ExportFolder;
            }
            set
            {
                IConfigurationStore.ExportFolder = value;
            }
        }

        public bool OverwriteImages
        {
            get
            {
                return IConfigurationStore.OverwriteImages;
            }
            set
            {
                IConfigurationStore.OverwriteImages = value;
            }
        }

        public void LoadImages(DBObject.Species species)
        {
            Dictionary<long, string> paths = IImagePathsStore.LoadImagePaths();

            species.Images = IImageStore.LoadImages(species.id, paths);
        }

        public void Update(DBObject.Species species, DBObject.Species editedSpecies)
        {
            IDatabase.BeginTransaction();
            try
            {
                ISpeciesStore.Update(editedSpecies);

                List<Int64> editedImageIds = editedSpecies.Images.Select(n => n.id).ToList();
                foreach (DBObject.Image image in species.Images)
                {
                    if (!editedImageIds.Contains(image.id))
                    {
                        IImageStore.Delete(image);
                    }
                }

                WriteImages(editedSpecies);

                IDatabase.CommitTransaction();
            }
            catch (Exception exception)
            {
                IDatabase.RollbackTransaction();
            }
        }

        public void Insert(DBObject.Species species)
        {
            IDatabase.BeginTransaction();
            try
            {
                ISpeciesStore.Insert(species);
                WriteImages(species);

                IDatabase.CommitTransaction();
            }
            catch (Exception exception)
            {
                IDatabase.RollbackTransaction();
            }
        }

        public void Delete(DBObject.Species species)
        {
            LoadImages(species);

            IDatabase.BeginTransaction();
            try
            {
                foreach (var image in species.Images)
                {
                    IImageStore.Delete(image);
                }
                ISpeciesStore.Delete(species);

                IDatabase.CommitTransaction();
            }
            catch
            {
                IDatabase.RollbackTransaction();
            }
        }

        public IEnumerable<DBObject.Species> SpeciesEnumerator
        {
            get
            {
                return ISpeciesStore.Enumerator;
            }
        }

        public void Update(DBObject.ImagePath imagePath)
        {
            IImagePathsStore.Update(imagePath);
        }

        public void Insert(DBObject.ImagePath imagePath)
        {
            IImagePathsStore.Insert(imagePath);
        }

        public bool Delete(DBObject.ImagePath imagePath)
        {
            if (!IImageStore.Exists(imagePath))
            {
                return false;
            }

            IImagePathsStore.Delete(imagePath);
            return true;
        }

        public IEnumerable<DBObject.ImagePath> ImagePathEnumerator
        {
            get
            {
                return IImagePathsStore.Enumerator;
            }
        }

        public static void Export(
            PetaPoco.IDatabase iSourceDatabase, 
            SpeciesManager sourceSpeciesManager,
            PetaPoco.IDatabase iTargetDatabase, 
            SpeciesManager targetSpeciesManager)
        {
            targetSpeciesManager.Copyright = sourceSpeciesManager.Copyright;
            targetSpeciesManager.ExportFolder = sourceSpeciesManager.ExportFolder;
            targetSpeciesManager.OverwriteImages = sourceSpeciesManager.OverwriteImages;

            Dictionary<long, long> speciesIdMap = new Dictionary<long, long>();
            foreach (DBObject.Species species in sourceSpeciesManager.ISpeciesStore.Enumerator)
            {
                long key = species.id;
                targetSpeciesManager.Insert(species);
                speciesIdMap.Add(key, species.id);
            }

            Dictionary<long, long> imagePathIdMap = new Dictionary<long, long>();
            foreach (DBObject.ImagePath imagePath in sourceSpeciesManager.IImagePathsStore.Enumerator)
            {
                long key = imagePath.id;
                targetSpeciesManager.Insert(imagePath);
                imagePathIdMap.Add(key, imagePath.id);
            }

            foreach (DBObject.Image image in sourceSpeciesManager.IImageStore.Enumerator)
            {
                image.fungus_id = speciesIdMap[image.fungus_id];
                image.image_database_id = imagePathIdMap[image.image_database_id];
                targetSpeciesManager.IImageStore.Insert(image);
            }

            iSourceDatabase.CloseSharedConnection();
            iTargetDatabase.CloseSharedConnection();
        }
    }
}
