using System.Collections.Generic;

namespace PetaPocoApp.Database
{
    interface ISpeciesStore
    {
        void Insert(DBObject.Species species);
        void Update(DBObject.Species species);
        void Delete(DBObject.Species species);
        IEnumerable<DBObject.Species> Enumerator { get; }
    }
}
