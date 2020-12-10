using System.Collections.Generic;

namespace PetaPocoApp.PetaPocoAdapter
{
    class SpeciesStore : PetaPocoApp.Database.ISpeciesStore
    {
        public SpeciesStore(PetaPoco.IDatabase iDatabase)
        {
            _iDatabase = iDatabase;
            UseTableNameFix = false;
        }

        // Postgres converts the table name in queries to lower case unless enclosed in quotes
        public bool UseTableNameFix { get; protected set; }
        protected readonly PetaPoco.IDatabase _iDatabase;

        public void Insert(DBObject.Species species)
        {
            species.id = System.Convert.ToInt64(_iDatabase.Insert("tblFungi", "id", species));
        }

        public void Update(DBObject.Species species)
        {
            _iDatabase.Update("tblFungi", "id", species);
        }

        public void Delete(DBObject.Species species)
        {
            _iDatabase.Delete("tblFungi", "id", species);
        }

        public IEnumerable<DBObject.Species> Enumerator
        {
            get
            {
                return _iDatabase.Query<DBObject.Species>(
                    UseTableNameFix ?
                    "SELECT * FROM \"tblFungi\" ORDER BY species" :
                    "SELECT * FROM tblFungi ORDER BY species"); 

            }
        }
    }
}
