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
                .Select(v => PlantenParser.ParseSearchText(v.Variantnaam))
                .Distinct()
                .OrderBy(variantnaam => variantnaam)
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
    }
}