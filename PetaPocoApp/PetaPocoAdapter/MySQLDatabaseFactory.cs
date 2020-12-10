using System.Text;
namespace PetaPocoApp.PetaPocoAdapter
{
    internal class MySQLDatabaseFactory
    {
        private static string MakeConnectionString(string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Host=");
            stringBuilder.Append(host);
            if (useWindowsAuthentication)
            {
                stringBuilder.Append("; IntegratedSecurity=true");
            }
            else
            { 
                stringBuilder.Append("; Username=");
                stringBuilder.Append(userName);
                stringBuilder.Append("; Password=");
                stringBuilder.Append(password);
            }
            stringBuilder.Append("; Database= ");
            stringBuilder.Append(dbName);
            stringBuilder.Append("; Port= ");
            stringBuilder.Append(port);

            return stringBuilder.ToString();
        }

        public static void CreateDatabase(out PetaPoco.IDatabase iDatabase, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            // Connect to the master DB to create the requested database

            OpenDatabase(out iDatabase, host, port, useWindowsAuthentication, userName, password, "MySql");

            iDatabase.Execute(@"CREATE DATABASE " + dbName);
            iDatabase.CloseSharedConnection();

            // Connect to the new database

            OpenDatabase(out iDatabase, host, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase.Execute("CREATE TABLE tblConfiguration (name VARCHAR(100) NOT NULL PRIMARY KEY, value VARCHAR(767) NOT NULL);");
            iDatabase.Execute("CREATE TABLE tblFungi ( " +
                "id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                "species VARCHAR(1000) NOT NULL, " +
                "synonyms VARCHAR(1000) NULL, " +
                "common_name VARCHAR(200) NULL, " +
                "fruiting_body VARCHAR(1000) NULL, " +
                "cap VARCHAR(500) NULL, " +
                "hymenium VARCHAR(500) NULL, " +
                "gills VARCHAR(500) NULL, " +
                "pores VARCHAR(500) NULL, " +
                "spines VARCHAR(500) NULL, " +
                "stem VARCHAR(500) NULL, " +
                "flesh VARCHAR(500) NULL, " +
                "smell VARCHAR(200) NULL, " +
                "taste VARCHAR(200) NULL, " +
                "season VARCHAR(200) NULL, " +
                "distribution VARCHAR(200) NULL, " +
                "habitat VARCHAR(200) NULL, " +
                "spore_print VARCHAR(200) NULL, " +
                "microscopic_features VARCHAR(1000) NULL, " +
                "edibility VARCHAR(1000) NULL, " +
                "notes VARCHAR(1000) NULL);");
            iDatabase.Execute("CREATE TABLE tblImagesDatabase (id INTEGER AUTO_INCREMENT PRIMARY KEY, path VARCHAR(255) UNIQUE);");
            iDatabase.Execute("CREATE TABLE tblImages ( " +
                    "id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                    "fungus_id INTEGER NOT NULL, " +
                    "image_database_id INTEGER NULL, " +
                    "filename VARCHAR(1000) NOT NULL, " +
                    "description VARCHAR(1000) NULL, " +
                    "copyright VARCHAR(1000) NULL, " +
                    "display_order INTEGER NULL, " +
                    "FOREIGN KEY(fungus_id) REFERENCES tblFungi(id)," +
                    "FOREIGN KEY(image_database_id) REFERENCES tblImagesDatabase(id));");
        }

        public static void OpenDatabase(out PetaPoco.IDatabase iDatabase, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            string connectionString = MakeConnectionString(host, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase = new PetaPoco.Database(connectionString, "MySql");
            iDatabase.OpenSharedConnection();
        }
    }
}
