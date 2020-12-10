namespace PetaPocoApp.PetaPocoAdapter
{
    internal class SQLiteDatabaseFactory
    {
        public static void CreateDatabase(out PetaPoco.IDatabase iDatabase, string folder, string dbName)
        {
            string path = System.IO.Path.Combine(folder, dbName + ".sqlite");
            iDatabase = new PetaPoco.Database("Data Source=" + path + ";Version=3;", "System.Data.SQLite");
            iDatabase.OpenSharedConnection();
            
            iDatabase.Execute("CREATE TABLE tblConfiguration (name TEXT NOT NULL, value TEXT NOT NULL, PRIMARY KEY(name));");
            iDatabase.Execute("CREATE TABLE tblFungi ( " +
                "id INTEGER NOT NULL PRIMARY KEY, " +
                "species TEXT NOT NULL, " +
                "synonyms TEXT NULL, " +
                "common_name TEXT NULL, " +
                "fruiting_body TEXT NULL, " +
                "cap TEXT NULL, " +
                "hymenium TEXT NULL, " +
                "gills TEXT NULL, " +
                "pores TEXT NULL, " +
                "spines TEXT NULL, " +
                "stem TEXT NULL, " +
                "flesh TEXT NULL, " +
                "smell TEXT NULL, " +
                "taste TEXT NULL, " +
                "season TEXT NULL, " +
                "distribution TEXT NULL, " +
                "habitat TEXT NULL, " +
                "spore_print TEXT NULL, " +
                "microscopic_features TEXT NULL, " +
                "edibility TEXT NULL, " +
                "notes TEXT NULL);");
            iDatabase.Execute("CREATE TABLE tblImagesDatabase (id INTEGER PRIMARY KEY, path TEXT UNIQUE);");
            iDatabase.Execute("CREATE TABLE tblImages ( " +
                    "id INTEGER NOT NULL PRIMARY KEY, " +
                    "fungus_id INTEGER NOT NULL, " +
                    "image_database_id INTEGER NULL, " +
                    "filename TEXT NOT NULL, " +
                    "description TEXT NULL, " +
                    "copyright TEXT NULL, " +
                    "display_order INTEGER NULL, " +
                    "FOREIGN KEY(fungus_id) REFERENCES tblFungi(id)," +
                    "FOREIGN KEY(image_database_id) REFERENCES tblImagesDatabase(id));");
        }

        public static void OpenDatabase(out PetaPoco.IDatabase iDatabase, string filepath)
        {
            iDatabase = new PetaPoco.Database("Data Source=" + filepath + ";Version=3;", "System.Data.SQLite");
            iDatabase.OpenSharedConnection();
        }
    }
}
