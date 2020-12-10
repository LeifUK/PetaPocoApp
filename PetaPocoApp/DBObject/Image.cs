using System;
using PetaPoco;

namespace PetaPocoApp.DBObject
{
    internal class Image
    {
        public Image()
        {
            // Indicates a new unsaved image
            id = 0;
        }

        //public Image Clone()
        //{
        //    Image image = new Image();
        //    image.Fields.Clear();
        //    foreach (var field in Fields)
        //    {
        //        image.Fields.Add(field.Clone());
        //    }
        //    image.Path = Path;
        //    return image;
        //}

        public Int64 id { get; set; }
        public Int64 fungus_id { get; set; }
        public Int64 image_database_id { get; set; }
        public string filename { get; set; }
        public string description { get; set; }
        public string copyright { get; set; }
        public Byte display_order { get; set; }
        [Ignore]
        public string Path { get; set; }
    }
}
