using PetaPocoApp.Database;

namespace PetaPocoApp.PetaPocoAdapter
{
    class SpeciesManagerFactory
    {
        public static SpeciesManager GetSpeciesManager(PetaPoco.IDatabase iDatabase)
        {
            SpeciesManager speciesManager = new SpeciesManager(
                new PetaPocoAdapter.Database(iDatabase),
                new PetaPocoAdapter.ConfigurationStore(iDatabase),
                new PetaPocoAdapter.SpeciesStore(iDatabase),
                new PetaPocoAdapter.ImagePathsStore(iDatabase),
                new PetaPocoAdapter.ImageStore(iDatabase));
            speciesManager.Initialise();
            return speciesManager;
        }
    }
}
