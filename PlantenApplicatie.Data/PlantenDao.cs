using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.Data
{
    // De PlantenDao Klasse (Jim&Davy&Liam&Zakaria&Lily)
    public class PlantenDao
    {
        private readonly PlantenContext _context;
        private const string NoVariant = "N/A";

        static PlantenDao()
        {
            Instance = new PlantenDao();
        }

        private PlantenDao()
        {
            _context = new PlantenContext();
        }

        public static PlantenDao Instance { get; }

        // Geef alle planten terug met al hun properties van de andere tabellen (Lily & davy)
        public List<Plant> GetPlanten()
        {
            return _context.Plant
                .Include(p => p.Abiotiek)
                .Include(p => p.AbiotiekMulti)
                .Include(p => p.BeheerMaand)
                .Include(p => p.Commensalisme)
                .Include(p => p.CommensalismeMulti)
                .Include(p => p.ExtraEigenschap)
                .Include(p => p.Fenotype)
                .Include(p => p.Foto)
                .ToList();
        }

        // Haalt de ID's van TFGSV op en roept een methode op om te zoeken op ID's en Naam, 
        // houd rekening met null (Lily)
        public List<Plant> SearchPlants(string? type, string? family, string? genus,
            string? species, string? variant, string? name)
        {
            var typeIds = GetTypeIds(type is null ? string.Empty : type);
            var familyIds = GetFamilyIds(family is null ? string.Empty : family);
            var genusIds = GetGenusIds(genus is null ? string.Empty : genus);
            var speciesIds = GetSpeciesIds(species is null ? string.Empty : species);
            var variantIds = GetVariantIds(variant is null ? string.Empty : variant);

            return SearchPlantsWithTgsvAndName(
                typeIds, familyIds, genusIds, speciesIds, variantIds, name is null ? string.Empty : name);
        }

        // Zoekt op ID's en naam (Lily)
        private List<Plant> SearchPlantsWithTgsvAndName(List<long> typeIds, List<long> familyIds, 
            List<long> genusIds, List<long> speciesIds, List<long?> variantIds, string name)
        {
            return _context.Plant
                .Include(p => p.Abiotiek)
                .Include(p => p.AbiotiekMulti)
                .Include(p => p.BeheerMaand)
                .Include(p => p.Commensalisme)
                .Include(p => p.CommensalismeMulti)
                .Include(p => p.ExtraEigenschap)
                .Include(p => p.Fenotype)
                .Include(p => p.Foto)
                .ToList()
                .Where(p =>
                    variantIds.Contains(p.VariantId)
                    && speciesIds.Contains((long)p.SoortId!)
                    && genusIds.Contains((long)p.GeslachtId!)
                    && familyIds.Contains((long)p.FamilieId!)
                    && typeIds.Contains((long)p.TypeId!)
                    && PlantenParser.ParseSearchText(p.Fgsv)
                        .Contains(PlantenParser.ParseSearchText(name)))
                .OrderBy(p => p.Fgsv)
                .ToList();
        }

        // Geef de type ID's terug (Lily)
        private List<long> GetTypeIds(string type)
        {
            return _context.TfgsvType.ToList().Where(t => 
                PlantenParser.ParseSearchText(t.Planttypenaam)
                    .Contains(PlantenParser.ParseSearchText(type)))
                .Select(t => t.Planttypeid)
                .ToList();
        }

        // Geef de familie ID's terug (Lily)
        private List<long> GetFamilyIds(string family)
        {
            return _context.TfgsvFamilie.ToList().Where(f =>
                PlantenParser.ParseSearchText(f.Familienaam)
                    .Contains(PlantenParser.ParseSearchText(family)))
                .Select(f => f.FamileId)
                .ToList();
        }

        // Geef de geslacht ID's terug (Lily)
        private List<long> GetGenusIds(string genus)
        {
            return _context.TfgsvGeslacht.ToList().Where(g =>
                PlantenParser.ParseSearchText(g.Geslachtnaam)
                    .Contains(PlantenParser.ParseSearchText(genus)))
                .Select(g => g.GeslachtId)
                .ToList();
        }

        // Geef de soort Id's terug (Lily)
        private List<long> GetSpeciesIds(string species)
        {
            return _context.TfgsvSoort.ToList().Where(s =>
                PlantenParser.ParseSearchText(s.Soortnaam)
                    .Contains(PlantenParser.ParseSearchText(species)))
                .Select(s => s.Soortid)
                .ToList();
        }

        // Geef de variant ID's terug (Lily)
        private List<long?> GetVariantIds(string variant)
        {
            // Mogelijkheid om N/A variant te kiezen 
            if (variant == NoVariant)
            {
                return new List<long?> { null };
            }

            var variants = _context.TfgsvVariant.ToList().Where(v =>
                PlantenParser.ParseSearchText(v.Variantnaam)
                    .Contains(PlantenParser.ParseSearchText(variant)))
                .Select(v => v.VariantId)
                .Cast<long?>()
                .ToList();

           // Voeg null toe aan de lijst om planten met geen variant terug te kunnen krijgen
            if (variant == string.Empty)
            {
                variants.Add(null);
            }

            return variants;
        }


        //roep de ID's van aparte planten op ipv de lijst van planten. Jim
        private long GetTypeId(string type)
        {
            return _context.TfgsvType
                .SingleOrDefault(t => t.Planttypenaam == type)
                .Planttypeid;
        }
        private long GetFamilyId(string family)
        {
            return _context.TfgsvFamilie
                .SingleOrDefault(t => t.Familienaam == family)
                .FamileId;
        }
        private long GetGenusId(string genus)
        {
            return _context.TfgsvGeslacht
                .SingleOrDefault(t => t.Geslachtnaam == genus)
                .GeslachtId;
        }
        private long GetSpeciesId(string species)
        {
            return _context.TfgsvSoort
                .SingleOrDefault(t => t.Soortnaam == species)
                .Soortid;
        }
        private long? GetVariantId(string variant)
        {
            if (variant == NoVariant)
            {
                return null;
            }

            return _context.TfgsvVariant
                .SingleOrDefault(t => t.Variantnaam == variant)
                .VariantId;
        }


        // Haalt alle unieke typenamen op (Davy&Lily&Jim)
        public List<string> GetTypes()
        {
            return _context.TfgsvType
                .Select(t => t.Planttypenaam)
                .Distinct()
                .ToList();
        }

        // Haalt alle unieke familienamen op (Davy&Lily&Jim)
        public List<string> GetUniqueFamilyNames()
        {
            return _context.TfgsvFamilie
                .Select(f => f.Familienaam)
                .Distinct()
                .ToList();
        }

        // Haalt alle unieke geslachtnamen op (Davy&Lily&Jim)
        public List<string> GetUniqueGenusNames()
        {
            return _context.TfgsvGeslacht
                .Select(g => g.Geslachtnaam)
                .Distinct()
                .ToList();
        }

        // Haalt alle unieke soortnamen op (Davy&Lily&Jim)
        public List<string> GetUniqueSpeciesNames()
        {
            return _context.TfgsvSoort
                .ToList()
                .Select(s => PlantenParser.ParseSearchText(s.Soortnaam))
                .Distinct()
                .OrderBy(soortnaam => soortnaam)
                .ToList();
        }

        // Haalt alle unieke varianten op (Davy&Lily&Jim)
        public List<string> GetUniqueVariantNames()
        {
            var list = _context.TfgsvVariant
                .ToList()
                .Select(v => v.Variantnaam)
                .Distinct()
                .OrderBy(variantnaam => PlantenParser.ParseSearchText(variantnaam))
                .ToList();
            
            list.Insert(0, NoVariant);

            return list;
        }

        // Haal alle Habitats op via hun waarde (Lily)
        public List<AbioHabitat> GetHabitatsByValues(List<string> habitatKeys)
        {
            return _context.AbioHabitat
                .Where(ah => habitatKeys.Contains(ah.Afkorting))
                .ToList();
        }

        // Haal alle sociabiliteiten op via hun waarde (Lily)
        public List<CommSocialbiliteit> GetCommSociabiliteitByValues(List<string> commensalismeKeys)
        {
            return _context.CommSocialbiliteit
                .Where(cm => commensalismeKeys.Contains(cm.Sociabiliteit))
                .ToList();
        }

        //verander de gegevens van de Plant Onderwerp (jim)
        public void ChangePlant(Plant plant, string? type,string? family, string? genus, string? species, string? variant,short? plantMin, short? plantMax)
        {
            //haal de id's van de verschillende types op
            var typeId = (int?)GetTypeId(type);
            var familyId = (int?)GetFamilyId(family);
            var genusId = (int?)GetGenusId(genus);
            var speciesId = (int?)GetSpeciesId(species);
            var variantId = (int?)GetVariantId(variant);
            //verander de gegevens van de plant op basis van wat er geselecteerd word, word er niks geselecteerd
            //dan verandert de waarde niet

            plant.Type = type ?? plant.Type;
            plant.TypeId = typeId ?? plant.TypeId;
            plant.Familie = family ?? plant.Familie;
            plant.FamilieId = familyId ?? plant.FamilieId;
            plant.Geslacht = genus ?? plant.Geslacht;
            plant.GeslachtId = genusId ?? plant.GeslachtId;
            plant.Soort = species ?? plant.Soort;
            plant.SoortId = speciesId ?? plant.SoortId;
            plant.Variant = variant ?? plant.Variant; ;
            plant.VariantId = variantId ?? plant.VariantId;
            plant.PlantdichtheidMin = plantMin ?? plant.PlantdichtheidMin;
            plant.PlantdichtheidMax = plantMax ?? plant.PlantdichtheidMax;

            //de plantnaam verandert mee als er iets verandert in de FGSV volgorde.
            plant.Fgsv = plant.Familie + " " + plant.Geslacht + " " + plant.Soort + " " + plant.Variant;


            _context.SaveChanges();
        }

        //Voeg een fenotype toe aan de geselecteerde plant
        public void addFenotype(Plant plant,int bladgrootte,string bladvorm,string ratioBloeiBlad,string bloeiwijze,
        string habitus, string levensvorm)
        {
            var fenotypePlant = new Fenotype
            {
                PlantId = plant.PlantId,
                Bladgrootte = bladgrootte,
                Bladvorm = bladvorm,
                RatioBloeiBlad = ratioBloeiBlad,
                Bloeiwijze = bloeiwijze,
                Habitus = habitus,
                Levensvorm = levensvorm
            };

            if(_context.Fenotype.Contains(fenotypePlant))
            {
                fenotypePlant = null;
            }
            _context.Fenotype.Add(fenotypePlant);
            _context.SaveChanges();
        }

        //verander een fenotype van de geselecteerde plant
        public void ChangeFenotype(Fenotype fenotype, int? bladgrootte, string bladvorm, string ratioBloeiBlad, string bloeiwijze,
        string habitus, string levensvorm)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.Id == fenotype.Id);

            selectedfenotype.Bladgrootte = bladgrootte ?? selectedfenotype.Bladgrootte;
            selectedfenotype.Bladvorm = bladvorm ?? selectedfenotype.Bladvorm;
            selectedfenotype.RatioBloeiBlad = ratioBloeiBlad ?? selectedfenotype.RatioBloeiBlad;
            selectedfenotype.Bloeiwijze = bloeiwijze ?? selectedfenotype.Bloeiwijze;
            selectedfenotype.Habitus = habitus ?? selectedfenotype.Habitus;
            selectedfenotype.Levensvorm = levensvorm ?? selectedfenotype.Levensvorm;

            _context.SaveChanges();
        }
        //verwijder een fenotype van de geselecteerde plant
        public void DeleteFenotype(Fenotype fenotype)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.Id == fenotype.Id);

            _context.Fenotype.Remove(selectedfenotype);
            _context.SaveChanges();
        }
        //voeg een multifenotype toe

        public void AddMultiFenotype(Plant plant, string eigenschap, string maand, string waarde)
        {
            var fenotypeMultiPlant = new FenotypeMulti
            {
                PlantId = plant.PlantId,
                Eigenschap = eigenschap,
                Maand = maand,
                Waarde = waarde
            };
            _context.FenotypeMulti.Add(fenotypeMultiPlant);
            _context.SaveChanges();
        }

        //verander een multifenotype van de geselecteerde plant

        public void ChangeMultiFenotype(FenotypeMulti fenotypeMulti, string eigenschap, string maand, string waarde)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);

            selectedFenotypeMulti.Eigenschap = eigenschap ?? selectedFenotypeMulti.Eigenschap;
            selectedFenotypeMulti.Maand = maand ?? selectedFenotypeMulti.Maand;
            selectedFenotypeMulti.Waarde = waarde ?? selectedFenotypeMulti.Waarde;
       
            _context.SaveChanges();
        }

        public void RemoveMultiFenotype(FenotypeMulti fenotypeMulti)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);
            _context.Remove(fenotypeMulti);
            _context.SaveChanges();
        }
    }
}