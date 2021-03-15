using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.Data
{
    public class PlantenDao
    {
        private readonly PlantenContext _context;

        static PlantenDao()
        {
            Instance = new PlantenDao();
        }

        private PlantenDao()
        {
            _context = new PlantenContext();
        }

        public static PlantenDao Instance { get; }

        public List<Plant> GetPlanten()
        {
            return _context.Plant
                .ToList();
        }

        public List<Plant> SearchPlants(string type, string family, string genus,
            string species, string? variant, string name)
        {
            var typeIds = GetTypeIds(type);
            var familyIds = GetFamilyIds(family);
            var genusIds = GetGenusIds(genus);
            var speciesIds = GetSpeciesIds(species);
            var variantIds = GetVariantIds(variant);

            return SearchPlantsWithTgsvAndName(typeIds, familyIds, genusIds, speciesIds, variantIds, name);
        }

        private List<Plant> SearchPlantsWithTgsvAndName(List<long> typeIds, List<long> familyIds, List<long> genusIds, List<long> speciesIds, 
            List<long?> variantIds, string name)
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
            
        private List<long> GetTypeIds(string type)
        {
            return _context.TfgsvType.ToList().Where(t => 
                PlantenParser.ParseSearchText(t.Planttypenaam)
                    .Contains(PlantenParser.ParseSearchText(type)))
                .Select(t => t.Planttypeid)
                .ToList();
        }

        private List<long> GetFamilyIds(string family)
        {
            return _context.TfgsvFamilie.ToList().Where(f =>
                PlantenParser.ParseSearchText(f.Familienaam)
                    .Contains(PlantenParser.ParseSearchText(family)))
                .Select(f => f.FamileId)
                .ToList();
        }

        private List<long> GetGenusIds(string genus)
        {
            return _context.TfgsvGeslacht.ToList().Where(g =>
                PlantenParser.ParseSearchText(g.Geslachtnaam)
                    .Contains(PlantenParser.ParseSearchText(genus)))
                .Select(g => g.GeslachtId)
                .ToList();
        }

        private List<long> GetSpeciesIds(string species)
        {
            return _context.TfgsvSoort.ToList().Where(s =>
                PlantenParser.ParseSearchText(s.Soortnaam)
                    .Contains(PlantenParser.ParseSearchText(species)))
                .Select(s => s.Soortid)
                .ToList();
        }

        private List<long?> GetVariantIds(string? variant)
        {
            if (variant is null)
            {
                return new List<long?>() { null };
            }

            return _context.TfgsvVariant.ToList().Where(v =>
                PlantenParser.ParseSearchText(v.Variantnaam)
                    .Contains(PlantenParser.ParseSearchText(variant)))
                .Select(v => v.VariantId)
                .Cast<long?>()
                .ToList();
        }


        public List<string> GetUniqueFamilyNames()
        {
            return _context.TfgsvFamilie
                .Select(f => f.Familienaam)
                .Distinct()
                .ToList();
        }

        public List<string> GetUniqueGenusNames()
        {
            return _context.TfgsvGeslacht
                .Select(g => g.Geslachtnaam)
                .Distinct()
                .ToList();
        }

        public List<string> GetUniqueSpeciesNames()
        {
            return _context.TfgsvSoort
                .Select(s => s.Soortnaam)
                .Distinct()
                .ToList();
        }

        public List<TfgsvType> GetTypes()
        {
            return _context.TfgsvType
                .ToList();
        }
    }
}