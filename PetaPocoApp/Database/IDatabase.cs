namespace PetaPocoApp.Database
{
    interface IDatabase
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
