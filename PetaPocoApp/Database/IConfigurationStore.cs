namespace PetaPocoApp.Database
{
    interface IConfigurationStore
    {
        void Initialise();

        string Copyright { get; set; }
        string ExportFolder { get; set; }
        bool OverwriteImages { get; set; }
    }
}
