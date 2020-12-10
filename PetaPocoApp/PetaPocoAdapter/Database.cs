namespace PetaPocoApp.PetaPocoAdapter
{
    internal class Database : PetaPocoApp.Database.IDatabase
    {
        public Database(PetaPoco.IDatabase iDatabase)
        {
            _iDatabase = iDatabase;
        }

        private readonly PetaPoco.IDatabase _iDatabase;

        public void BeginTransaction()
        {
            _iDatabase.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _iDatabase.CompleteTransaction();
        }

        public void RollbackTransaction()
        {
            _iDatabase.AbortTransaction();
        }
    }
}
