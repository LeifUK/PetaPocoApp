using System.Linq;
using PetaPoco;

namespace PetaPocoApp.PetaPocoAdapter
{
    class ConfigurationStore : PetaPocoApp.Database.IConfigurationStore
    {
        public ConfigurationStore(IDatabase iDatabase)
        {
            _iDatabase = iDatabase;
            UseTableNameFix = false;
        }

        // Postgres converts the table name in queries to lower case unless enclosed in quotes
        public bool UseTableNameFix { get; protected set; }
        protected readonly IDatabase _iDatabase;

        private void CreateIfNotExists(DBObject.Configuration configuration)
        {
            string query = UseTableNameFix ? "SELECT * FROM \"tblConfiguration\" WHERE name='" : "SELECT * FROM tblConfiguration WHERE name='";
            if (_iDatabase.Query<DBObject.Configuration>(query + configuration.name + "'").Count() == 0)
            {
                _iDatabase.Insert("tblConfiguration", "name", configuration);
            }
        }

        public void Initialise()
        {
            DBObject.Configuration configuration = new DBObject.Configuration();

            _iDatabase.BeginTransaction();

            configuration.name = "copyright";
            configuration.value = "Joe Bloggs";
            CreateIfNotExists(configuration);

            configuration.name = "export folder";
            configuration.value = "c:\\";
            CreateIfNotExists(configuration);

            configuration.name = "overwrite";
            configuration.value = "false";
            CreateIfNotExists(configuration);

            _iDatabase.CompleteTransaction();
        }

        public string Copyright
        {
            get
            {
                DBObject.Configuration configuration = _iDatabase.Query<DBObject.Configuration>(
                    UseTableNameFix ? 
                    "SELECT * FROM \"tblConfiguration\" WHERE name='copyright'" :
                    "SELECT * FROM tblConfiguration WHERE name='copyright'"
                    ).First();
                return configuration != null ? configuration.value : "";
            }
            set
            {
                _iDatabase.Update("tblConfiguration", "name", new DBObject.Configuration() { name = "copyright", value = value });
            }
        }

        public string ExportFolder
        {
            get
            {
                DBObject.Configuration configuration = _iDatabase.Query<DBObject.Configuration>(
                    UseTableNameFix ?
                    "SELECT * FROM \"tblConfiguration\" WHERE name='export folder'" :
                    "SELECT * FROM tblConfiguration WHERE name='export folder'"
                    ).First();
                return configuration != null ? configuration.value : "";
            }
            set
            {
                _iDatabase.Update("tblConfiguration", "name", new DBObject.Configuration() { name = "export folder", value = value });
            }
        }

        public bool OverwriteImages
        {
            get
            {
                DBObject.Configuration configuration = _iDatabase.Query<DBObject.Configuration>(
                    UseTableNameFix ?
                    "SELECT * FROM \"tblConfiguration\" WHERE name='overwrite'" :
                    "SELECT * FROM tblConfiguration WHERE name='overwrite'"
                    ).First();
                return configuration != null ? configuration.value == "1" : false;
            }
            set
            {
                _iDatabase.Update("tblConfiguration", "name", new DBObject.Configuration() { name = "overwrite", value = (value ? "1" : "0") });
            }
        }
    }
}
