using System.Collections.Generic;

namespace PetaPocoApp.Database
{
    internal interface ISpeciesManager
    {
        string Copyright { get; set; }
        string ExportFolder { get; set; }
        bool OverwriteImages { get; set; }

        bool ValidateImagePath(DBObject.Image image);
        void LoadImages(DBObject.Species species);
        void Update(DBObject.Species species, DBObject.Species editedSpecies);
        void Insert(DBObject.Species species);
        void Delete(DBObject.Species species);
        IEnumerable<DBObject.Species> SpeciesEnumerator { get; }

        void Update(DBObject.ImagePath imagePath);
        void Insert(DBObject.ImagePath imagePath);
        bool Delete(DBObject.ImagePath imagePath);
        IEnumerable<DBObject.ImagePath> ImagePathEnumerator { get; }
    }
}
