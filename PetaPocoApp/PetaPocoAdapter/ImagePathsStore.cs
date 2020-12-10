using System.Collections.Generic;
using PetaPoco;

namespace PetaPocoApp.PetaPocoAdapter
{
    internal class ImagePathsStore : PetaPocoApp.Database.IImagePathsStore
    {
        public ImagePathsStore(IDatabase iDatabase)
        {
            _iDatabase = iDatabase;
            UseTableNameFix = false;
        }

        protected readonly IDatabase _iDatabase;
        // Postgres converts the table name in queries to lower case unless enclosed in quotes
        public bool UseTableNameFix { get; protected set; }

        public void Update(DBObject.ImagePath imagePath)
        {
            _iDatabase.Update("tblImagesDatabase", "id", imagePath);
        }

        public void Insert(DBObject.ImagePath imagePath)
        {
            imagePath.id = System.Convert.ToInt64(_iDatabase.Insert("tblImagesDatabase", "id", imagePath));
        }

        public void Delete(DBObject.ImagePath imagePath)
        {
            _iDatabase.Delete("tblImagesDatabase", "id", imagePath);
        }

        public IEnumerable<DBObject.ImagePath> Enumerator
        {
            get
            {
                return _iDatabase.Query<DBObject.ImagePath>(
                    UseTableNameFix ? "SELECT * FROM \"tblImagesDatabase\"" : "SELECT * FROM tblImagesDatabase");
            }
        }

        public Dictionary<long, string> LoadImagePaths()
        {
            Dictionary<long, string> paths = new Dictionary<long, string>();

            foreach (DBObject.ImagePath imagePath in Enumerator)
            {
                paths.Add(imagePath.id, imagePath.path);
            }

            return paths;
        }
    }
}
