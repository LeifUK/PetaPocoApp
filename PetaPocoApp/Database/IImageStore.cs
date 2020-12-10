using System.Collections.Generic;

namespace PetaPocoApp.Database
{
    internal interface IImageStore
    {
        bool Exists(DBObject.ImagePath imagePath);
        void Insert(DBObject.Image image);
        void Update(DBObject.Image image);
        void Delete(DBObject.Image image);
        bool UseTableNameFix { get; }
        IEnumerable<DBObject.Image> Enumerator { get; }
        List<DBObject.Image> LoadImages(long speciesId, Dictionary<long, string> paths);
    }
}
