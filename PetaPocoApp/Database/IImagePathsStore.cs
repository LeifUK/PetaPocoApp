using System.Collections.Generic;

namespace PetaPocoApp.Database
{ 
    internal interface IImagePathsStore
    {
        void Update(DBObject.ImagePath imagePath);
        void Insert(DBObject.ImagePath imagePath);
        void Delete(DBObject.ImagePath imagePath);
        IEnumerable<DBObject.ImagePath> Enumerator { get; }
        Dictionary<long, string> LoadImagePaths();
    }
}
