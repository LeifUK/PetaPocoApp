using PetaPoco;
using System;
using System.Collections.Generic;

namespace PetaPocoApp.DBObject
{
    internal class Species
    {
        public Species()
        {
            // Indicates a new unsaved species
            id = 0;
            Images = new List<Image>();
        }

        public Species Clone()
        {
            Species clonedSpecies = new Species();
            clonedSpecies.id = id;
            clonedSpecies.species = species;
            clonedSpecies.synonyms = synonyms;
            clonedSpecies.common_name = common_name;
            clonedSpecies.fruiting_body = fruiting_body;
            clonedSpecies.cap = cap;
            clonedSpecies.hymenium = hymenium;
            clonedSpecies.gills = gills;
            clonedSpecies.pores = pores;
            clonedSpecies.spines = spines;
            clonedSpecies.stem = stem;
            clonedSpecies.flesh = flesh;
            clonedSpecies.smell = smell;
            clonedSpecies.taste = taste;
            clonedSpecies.season = season;
            clonedSpecies.distribution = distribution;
            clonedSpecies.habitat = habitat;
            clonedSpecies.spore_print = spore_print;
            clonedSpecies.microscopic_features = microscopic_features;
            clonedSpecies.edibility = edibility;
            clonedSpecies.notes = notes;
            return clonedSpecies;
        }

        #region ISpecies

        public Int64 id { get; set; }
        public string species { get; set; }
        public string synonyms { get; set; }
        public string common_name { get; set; }
        public string fruiting_body { get; set; }
        public string cap { get; set; }
        public string hymenium { get; set; }
        public string gills { get; set; }
        public string pores { get; set; }
        public string spines { get; set; }
        public string stem { get; set; }
        public string flesh { get; set; }
        public string smell { get; set; }
        public string taste { get; set; }
        public string season { get; set; }
        public string distribution { get; set; }
        public string habitat { get; set; }
        public string spore_print { get; set; }
        public string microscopic_features { get; set; }
        public string edibility { get; set; }
        public string notes { get; set; }

        #endregion ISpecies
        
        [Ignore]
        public List<Image> Images { get; set; }
    }
}
