using System.Data.SqlClient;

namespace PetaPocoApp.PetaPocoAdapter
{
    internal class SQLServerDatabaseFactory
    {
        private static string MakeConnectionString(bool useDataSource, string dataSource, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            if (useDataSource)
            {
                sqlConnectionStringBuilder.DataSource = dataSource;
            }
            else
            {
                sqlConnectionStringBuilder.DataSource = host + "," + port;
            }

            if (useWindowsAuthentication)
            {
                sqlConnectionStringBuilder.IntegratedSecurity = true;
            }
            else
            {
                sqlConnectionStringBuilder.UserID = userName;
                sqlConnectionStringBuilder.Password = password;
            }

            sqlConnectionStringBuilder.InitialCatalog = dbName;
            return sqlConnectionStringBuilder.ToString();
        }

        public static PetaPoco.IDatabase CreateDatabase(out PetaPoco.IDatabase iDatabase, string dataSource, bool useWindowsAuthentication, string userName, string password, string folder, string dbName)
        {
            // Connect to the master DB to create the requested database

            OpenDatabase(out iDatabase, true, dataSource, null, -1, useWindowsAuthentication, userName, password, "master");

            string filename = System.IO.Path.Combine(folder, dbName);
            iDatabase.Execute("CREATE DATABASE " + dbName + " ON PRIMARY (Name=" + dbName + ", filename = \"" + filename + ".mdf\") LOG ON (name=" + dbName + "_log, filename=\"" + filename + ".ldf\")");
            iDatabase.CloseSharedConnection();

            // Connect to the new database

            OpenDatabase(out iDatabase, true, dataSource, null, -1, useWindowsAuthentication, userName, password, dbName);
            iDatabase.Execute("CREATE TABLE tblConfiguration (name VARCHAR(100) NOT NULL PRIMARY KEY, value VARCHAR(767) NOT NULL);");
            iDatabase.Execute("CREATE TABLE tblFungi ( " +
                "id INTEGER NOT NULL IDENTITY PRIMARY KEY, " +
                "species VARCHAR(1000) NOT NULL, " +
                "synonyms VARCHAR(1000) NULL, " +
                "common_name VARCHAR(1000) NULL, " +
                "fruiting_body VARCHAR(1000) NULL, " +
                "cap VARCHAR(1000) NULL, " +
                "hymenium VARCHAR(1000) NULL, " +
                "gills VARCHAR(1000) NULL, " +
                "pores VARCHAR(1000) NULL, " +
                "spines VARCHAR(1000) NULL, " +
                "stem VARCHAR(1000) NULL, " +
                "flesh VARCHAR(1000) NULL, " +
                "smell VARCHAR(1000) NULL, " +
                "taste VARCHAR(1000) NULL, " +
                "season VARCHAR(1000) NULL, " +
                "distribution VARCHAR(1000) NULL, " +
                "habitat VARCHAR(1000) NULL, " +
                "spore_print VARCHAR(1000) NULL, " +
                "microscopic_features VARCHAR(1000) NULL, " +
                "edibility VARCHAR(1000) NULL, " +
                "notes VARCHAR(1000) NULL);");
            iDatabase.Execute("CREATE TABLE tblImagesDatabase (id INTEGER IDENTITY PRIMARY KEY, path VARCHAR(255) UNIQUE);");
            iDatabase.Execute("CREATE TABLE tblImages ( " +
                    "id INTEGER NOT NULL IDENTITY PRIMARY KEY, " +
                    "fungus_id INTEGER NOT NULL, " +
                    "image_database_id INTEGER NULL, " +
                    "filename VARCHAR(1000) NOT NULL, " +
                    "description VARCHAR(1000) NULL, " +
                    "copyright VARCHAR(1000) NULL, " +
                    "display_order INTEGER NULL, " +
                    "FOREIGN KEY(fungus_id) REFERENCES tblFungi(id)," +
                    "FOREIGN KEY(image_database_id) REFERENCES tblImagesDatabase(id));");

            return iDatabase;
        }

        public static void OpenDatabase(out PetaPoco.IDatabase iDatabase, bool useDataSource, string dataSource, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            string connectionString = MakeConnectionString(useDataSource, dataSource, host, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase = new PetaPoco.Database(connectionString, "System.Data.SqlClient");
            iDatabase.OpenSharedConnection();
        }
    }
}
