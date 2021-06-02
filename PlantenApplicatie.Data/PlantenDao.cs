using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.Data
{
    //PlantenDao class (Jim&Davy&Liam&Zakaria&Lily)
    public class PlantenDao
    {
        public const string NoVariant = "N/A";

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

        //get all the plants and their different properties for later use(Lily & davy)
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

        //Get the ID's of TFGSV and call a method to search for its Id and name, 
        //keeps the null values in mind(Lily)
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

        //update the selected user and its values(Davy)
        public string UpdateUser(string email, string password)
        {
            var gebruiker = _context.Gebruiker.SingleOrDefault(g => g.Emailadres == email);

            if (gebruiker != null)
            {
                gebruiker.HashPaswoord = Encryptor.GenerateMD5Hash(password);

                _context.Gebruiker.Update(gebruiker);
                _context.SaveChanges();

                return "Wachtwoord aangepast";
            } else
            {
                return "Emailadres werd niet teruggevonden in de database";
            }
        }

        //get the Extra properties of the current plant(Davy)
        public List<ExtraEigenschap> getExtraEigenschappen(Plant selectedPlant)
        {
            return _context.ExtraEigenschap.Where(p => p.PlantId == selectedPlant.PlantId).ToList();
        }

        //Search the plants based on Id and Name(Lily)
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
                .Select(p =>
                {
                    p.Variant ??= NoVariant;
                    return p;
                })
                .OrderBy(p => p.Fgsv)
                .ToList();
        }
        //create Extra property for a plant (Davy)
        public string CreateExtraEigenschap(ExtraEigenschap extraEigenschap)
        {
            string message = "";
            var item = _context.ExtraEigenschap.Where(b => b.PlantId == extraEigenschap.PlantId);

            if (item.Count() == 1)
            {
                message = "Je kan maar 1 extra eigenschap toevoegen.";
            }
            else
            {
                _context.ExtraEigenschap.Add(extraEigenschap);
                _context.SaveChanges();
            }

            return message;
        }

        //Get the Type Id's (Lily)
        private List<long> GetTypeIds(string type)
        {
            return _context.TfgsvType.ToList().Where(t =>
                PlantenParser.ParseSearchText(t.Planttypenaam)
                    .Contains(PlantenParser.ParseSearchText(type)))
                .Select(t => t.Planttypeid)
                .ToList();
        }

        //Get the family id's (Lily)
        private List<long> GetFamilyIds(string family)
        {
            return _context.TfgsvFamilie.ToList().Where(f =>
                PlantenParser.ParseSearchText(f.Familienaam)
                    .Contains(PlantenParser.ParseSearchText(family)))
                .Select(f => f.FamileId)
                .ToList();
        }

        //Get the Genus id's (Lily)
        private List<long> GetGenusIds(string genus)
        {
            return _context.TfgsvGeslacht.ToList().Where(g =>
                PlantenParser.ParseSearchText(g.Geslachtnaam)
                    .Contains(PlantenParser.ParseSearchText(genus)))
                .Select(g => g.GeslachtId)
                .ToList();
        }

        //Get the Species id's (Lily)
        private List<long> GetSpeciesIds(string species)
        {
            return _context.TfgsvSoort.ToList().Where(s =>
                PlantenParser.ParseSearchText(s.Soortnaam)
                    .Contains(PlantenParser.ParseSearchText(species)))
                .Select(s => s.Soortid)
                .ToList();
        }

        //Get the Variant id's (Lily)
        private List<long?> GetVariantIds(string variant)
        {
            //possibility to set variable to N/A because some plants have no variants
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

            //add null to the list to find plants without variants
            if (variant == string.Empty)
            {
                variants.Add(null);
            }

            return variants;
        }


        //call the id's of individual types/family/genus/species and variants instead of the entire list. (Jim)
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

        //edit the extra property of the current plant (Davy)
        public void EditExtraEigenschap(ExtraEigenschap extraEigenschap)
        {
            _context.ExtraEigenschap.Update(extraEigenschap);
            _context.SaveChanges();
        }
        //remove the extra property of the current plant (Davy)
        public void RemoveExtraEigenschap(ExtraEigenschap extraEigenschap)
        {
            _context.ExtraEigenschap.Remove(extraEigenschap);
            _context.SaveChanges();
        }


        //get all unique type names (Davy&Lily&Jim)
        public List<string> GetTypes()
        {
            return _context.TfgsvType
                .Select(t => t.Planttypenaam)
                .Distinct()
                .ToList();
        }

        //get all unique Family names (Davy&Lily&Jim)
        public List<string> GetUniqueFamilyNames()
        {
            return _context.TfgsvFamilie
                .Select(f => f.Familienaam)
                .Distinct()
                .ToList();
        }

        //get all unique genus names (Davy&Lily&Jim)
        public List<string> GetUniqueGenusNames()
        {
            return _context.TfgsvGeslacht
                .Select(g => g.Geslachtnaam)
                .Distinct()
                .ToList();
        }

        //get all unique species names (Davy&Lily&Jim)
        public List<string> GetUniqueSpeciesNames()
        {
            return _context.TfgsvSoort
                .ToList()
                .Select(s => PlantenParser.ParseSearchText(s.Soortnaam))
                .Distinct()
                .OrderBy(soortnaam => soortnaam)
                .ToList();
        }

        //get all unique variants (Davy&Lily&Jim)
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

        //get all habitats based on their values (Lily)
        public List<AbioHabitat> GetHabitatsByValues(List<string> habitatKeys)
        {
            return _context.AbioHabitat
                .Where(ah => habitatKeys.Contains(ah.Afkorting))
                .ToList();
        }

        //get all sociabiliteiten based on their values (Lily)
        public List<CommSocialbiliteit> GetCommSociabiliteitByValues(List<string> commensalismeKeys)
        {
            return _context.CommSocialbiliteit
                .Where(cm => commensalismeKeys.Contains(cm.Sociabiliteit))
                .ToList();
        }

        //change the plant its values based on the plant that is selected (jim)
        public void ChangePlant(Plant plant, string? type, string? family, string? genus, string? species, string? variant, short? plantMin, short? plantMax)
        {
            //call the different ID's of the plant
            var typeId = (int?)GetTypeId(type);
            var familyId = (int?)GetFamilyId(family);
            var genusId = (int?)GetGenusId(genus);
            var speciesId = (int?)GetSpeciesId(species);
            var variantId = (int?)GetVariantId(variant);
            //change the values of the plant based on what is selected,
            //if nothing is selected the plant keeps its previous values (Jim)

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

            //If you change a detail to the plant, the name changes with it to keep consistency. (Jim)
            plant.Fgsv = plant.Familie + " " + plant.Geslacht + " " + plant.Soort + " " + plant.Variant;


            _context.SaveChanges();
        }

        //Add a fenotype to the selected plant (Jim)
        public void AddFenotype(Plant plant,int bladgrootte,string bladvorm,string ratioBloeiBlad,string bloeiwijze,
        string habitus, string levensvorm,string spruitfenologie)
        {
            var fenotypePlant = new Fenotype
            {
                PlantId = plant.PlantId,
                Bladgrootte = bladgrootte,
                Bladvorm = bladvorm,
                RatioBloeiBlad = ratioBloeiBlad,
                Bloeiwijze = bloeiwijze,
                Habitus = habitus,
                Levensvorm = levensvorm,
                Spruitfenologie = spruitfenologie
            };


            _context.Fenotype.Add(fenotypePlant);
            _context.SaveChanges();
        }

        //Change the fenotype of the selected plant (Jim)
        public Fenotype ChangeFenotype(Plant plant, int? bladgrootte, string bladvorm, string ratioBloeiBlad, string bloeiwijze,
            string habitus, string levensvorm,string spruitfenologie)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.PlantId == plant.PlantId);

            selectedfenotype.Bladgrootte = bladgrootte ?? selectedfenotype.Bladgrootte;
            selectedfenotype.Bladvorm = bladvorm ?? selectedfenotype.Bladvorm;
            selectedfenotype.RatioBloeiBlad = ratioBloeiBlad ?? selectedfenotype.RatioBloeiBlad;
            selectedfenotype.Bloeiwijze = bloeiwijze ?? selectedfenotype.Bloeiwijze;
            selectedfenotype.Habitus = habitus ?? selectedfenotype.Habitus;
            selectedfenotype.Levensvorm = levensvorm ?? selectedfenotype.Levensvorm;
            selectedfenotype.Spruitfenologie = spruitfenologie ?? selectedfenotype.Spruitfenologie;
            _context.SaveChanges();

            return selectedfenotype;
        }

        //delete the fenotype of the selected plant (Jim)

        /* public void DeleteFenotype(Fenotype fenotype)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.Id == fenotype.Id);

            _context.Fenotype.Remove(selectedfenotype);
            _context.SaveChanges();
        } */


        //add a multi fenotype to the selected plant (Jim)
        public void AddMultiFenotype(Plant plant, string eigenschap, string maand, string waarde)
        {
            var fenotypeMultiPlant = new FenotypeMulti
            {
                Id = GetLastFenoMultiId(),
                PlantId = plant.PlantId,
                Eigenschap = eigenschap,
                Maand = maand,
                Waarde = waarde
            };
            _context.FenotypeMulti.Add(fenotypeMultiPlant);
            _context.SaveChanges();
        }

        //get the last FenoMultiId to increment manually. (Jim)
        public long GetLastFenoMultiId()
        {
            var fenotypeMulti = _context.FenotypeMulti.Count();
            if (fenotypeMulti == null)
            {
                return 0;
            }

            return fenotypeMulti;
        }


        //change the multi fenotype of the selected plant (Jim)

        public void ChangeMultiFenotype(FenotypeMulti fenotypeMulti, string eigenschap, string maand, string waarde)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);

            selectedFenotypeMulti.Eigenschap = eigenschap ?? selectedFenotypeMulti.Eigenschap;
            selectedFenotypeMulti.Maand = maand ?? selectedFenotypeMulti.Maand;
            selectedFenotypeMulti.Waarde = waarde ?? selectedFenotypeMulti.Waarde;

            _context.SaveChanges();
        }
        //delete a fenotype multi of the selected plant(Jim)
        public void RemoveMultiFenotype(FenotypeMulti fenotypeMulti)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);
            _context.Remove(fenotypeMulti);
            _context.SaveChanges();
        }
        //add the extra properties to a plant(Jim)
        public void AddExtraEigenschap(Plant plant, string nectaWaarde, string pollenWaarde, bool bij, bool vlinder,
            bool eetbaar, bool kruid, bool geur, bool vorst)
        {
            var extraEigenschap = new ExtraEigenschap
            {
                PlantId = plant.PlantId,
                Nectarwaarde = nectaWaarde,
                Pollenwaarde = pollenWaarde,
                Bijvriendelijke = bij,
                Vlindervriendelijk = vlinder,
                Eetbaar = eetbaar,
                Kruidgebruik = kruid,
                Geurend = geur,
                Vorstgevoelig = vorst
            };

            _context.ExtraEigenschap.Add(extraEigenschap);
            _context.SaveChanges();
        }

        //edit the extra properties of the selected plant (Jim)
        public void ChangeExtraEigenschap(ExtraEigenschap extraEigenschap, string nectaWaarde, string pollenWaarde, bool bij, bool vlinder,
         bool eetbaar, bool kruid, bool geur, bool vorst)
        {
            var selectedExtraEigenschap = _context.ExtraEigenschap.FirstOrDefault(i => i.Id == extraEigenschap.Id);

            selectedExtraEigenschap.Nectarwaarde = nectaWaarde ?? selectedExtraEigenschap.Nectarwaarde;
            selectedExtraEigenschap.Pollenwaarde = pollenWaarde ?? selectedExtraEigenschap.Pollenwaarde;
            selectedExtraEigenschap.Bijvriendelijke = bij;
            selectedExtraEigenschap.Eetbaar = eetbaar;
            selectedExtraEigenschap.Kruidgebruik = kruid;
            selectedExtraEigenschap.Geurend = geur;
            selectedExtraEigenschap.Vorstgevoelig = vorst;


            _context.SaveChanges();
        }
        //delete the extra properties of the selected plant (Jim)

        public void DeleteExtraEigenschap(ExtraEigenschap extraEigenschap)
        {
            var selectedExtraEigenschap = _context.ExtraEigenschap.FirstOrDefault(i => i.Id == extraEigenschap.Id);
            _context.ExtraEigenschap.Remove(selectedExtraEigenschap);
            _context.SaveChanges();
        }
        //add the abiotiek to the selected plant (Liam)
        public void AddAbiotiek(Plant plant, string? bezonning, string? grondsoort, string? vochtbehoefte, string? voedingsbehoefte, string? antagonischeOmgeving)
        {
            //Habitat ontbreekt
            Abiotiek abiotiek = new Abiotiek
            {
                PlantId = plant.PlantId,
                Bezonning = bezonning,
                Grondsoort = grondsoort,
                Vochtbehoefte = vochtbehoefte,
                Voedingsbehoefte = voedingsbehoefte,
                AntagonischeOmgeving = antagonischeOmgeving
            };

            _context.Add(abiotiek);

            _context.SaveChanges();


        }
        //edit the  abiotiek of the selected plant (Liam)
        public void ChangeAbiotiek(Abiotiek abiotiek, string? bezonning, string? grondsoort,
            string? vochtbehoefte, string? voedingsbehoefte, string? antagonischeOmgeving)
        {


            var selectedAbiotiek = _context.Abiotiek.FirstOrDefault(s => s.Id == abiotiek.Id);

            selectedAbiotiek.Bezonning = bezonning ?? selectedAbiotiek.Bezonning;
            selectedAbiotiek.Grondsoort = grondsoort ?? selectedAbiotiek.Grondsoort;
            selectedAbiotiek.Vochtbehoefte = vochtbehoefte ?? selectedAbiotiek.Vochtbehoefte;
            selectedAbiotiek.Voedingsbehoefte = voedingsbehoefte ?? selectedAbiotiek.Voedingsbehoefte;
            selectedAbiotiek.AntagonischeOmgeving = antagonischeOmgeving ?? selectedAbiotiek.AntagonischeOmgeving;



            _context.SaveChanges();
        }

        //delete the abiotiek of the selected plant (Liam)
        public void DeleteAbiotiek(Abiotiek abiotiek)
        {
            var selectedAbiotiek = _context.Abiotiek.FirstOrDefault(s => s.Id == abiotiek.Id);

            _context.Abiotiek.Remove(selectedAbiotiek);

            _context.SaveChanges();
        }

        //add a commensalisme to the selected plant(Liam)

        public void AddCommensalisme(Plant plant, string ontwikkelingssnelheid, string strategie)
        {
            //sociabiliteit ontbreekt
            Commensalisme commensalisme = new Commensalisme
            {
                PlantId = plant.PlantId,
                Ontwikkelsnelheid = ontwikkelingssnelheid,
                Strategie = strategie
            };

            _context.Add(commensalisme);

            _context.SaveChanges();


        }
        //edit the commensalisme of the current plant (Liam)
        public Commensalisme ChangeCommensalisme(Plant plant, string ontwikkelingssnelheid, string strategie)
        {
            var selectedCommensalisme = _context.Commensalisme.FirstOrDefault(i => i.PlantId == plant.PlantId);

            selectedCommensalisme.Ontwikkelsnelheid = ontwikkelingssnelheid ?? selectedCommensalisme.Ontwikkelsnelheid;
            selectedCommensalisme.Strategie = strategie ?? selectedCommensalisme.Strategie;

            _context.SaveChanges();

            return selectedCommensalisme;
        }

        //delete the commensalisme of the current plant(Liam)
        public void DeleteCommensalisme(Commensalisme commensalisme)
        {
            var selectedCommensalisme = _context.Commensalisme.FirstOrDefault(s => s.Id == commensalisme.Id);

            _context.Commensalisme.Remove(selectedCommensalisme);

            _context.SaveChanges();
        }

        //add a abioMulti to the current plant (Liam)
        public void AddAbiotiekMulti(Plant plant, string eigenschap, string waarde)
        {

            AbiotiekMulti abiotiekMulti = new AbiotiekMulti
            {
                PlantId = plant.PlantId,
                Eigenschap = eigenschap,
                Waarde = waarde
            };

            _context.Add(abiotiekMulti);

            _context.SaveChanges();


        }
        //edit the selected abiotiek multi of the selected plant (Liam)

        public void ChangeAbiotiekMulti(AbiotiekMulti abiotiekMulti, string eigenschap, string waarde)
        {
            var selectedAbiotiekMulti = _context.AbiotiekMulti.FirstOrDefault(s => s.Id == abiotiekMulti.Id);

            selectedAbiotiekMulti.Eigenschap = eigenschap ?? selectedAbiotiekMulti.Eigenschap;
            selectedAbiotiekMulti.Waarde = waarde ?? selectedAbiotiekMulti.Waarde;

            _context.SaveChanges();
        }

        //delete the selected abiotiek multi of the selected plant (Liam)
        public void DeleteAbiotiekMulti(AbiotiekMulti abiotiekMulti)
        {
            var selectedAbiotiekMulti = _context.AbiotiekMulti.FirstOrDefault(s => s.Id == abiotiekMulti.Id);

            _context.AbiotiekMulti.Remove(selectedAbiotiekMulti);

            _context.SaveChanges();
        }
        //add a commensalisme multi to the selected plant(Liam)
        public void AddCommensalismeMulti(Plant plant, string eigenschap, string waarde)
        {

            CommensalismeMulti commensalismeMulti = new CommensalismeMulti
            {
                PlantId = plant.PlantId,
                Eigenschap = eigenschap,
                Waarde = waarde
            };

            _context.Add(commensalismeMulti);

            _context.SaveChanges();


        }
        //edit the selected commensalisme multi of the selected plant(Liam)

        public void ChangeCommensalismeMulti(CommensalismeMulti commensalismeMulti, string eigenschap, string waarde)
        {
            var selectedCommensalismeMulti = _context.CommensalismeMulti.FirstOrDefault(s => s.Id == commensalismeMulti.Id);

            selectedCommensalismeMulti.Eigenschap = eigenschap ?? selectedCommensalismeMulti.Eigenschap;
            selectedCommensalismeMulti.Waarde = waarde ?? selectedCommensalismeMulti.Waarde;

            _context.SaveChanges();
        }
        //delete the selected Commensalisme Multi(Liam)
        public void DeleteCommensalismeMulti(CommensalismeMulti commensalismeMulti)
        {
            var selectedCommensalismeMulti = _context.CommensalismeMulti.FirstOrDefault(s => s.Id == commensalismeMulti.Id);

            _context.CommensalismeMulti.Remove(selectedCommensalismeMulti);

            _context.SaveChanges();
        }

        //get the different abio values out of the tables (Liam)
        public List<string> GetAbioBezonning()
        {
            return _context.AbioBezonning.Select(s => s.Naam).ToList();
        }
        //Liam
        public List<string> GetAbioGrondsoort()
        {
            return _context.AbioGrondsoort.Select(s => s.Grondsoort).ToList();
        }
        //Liam
        public List<string> GetAbioVochtbehoefte()
        {
            return _context.AbioVochtbehoefte.Select(s => s.Vochtbehoefte).ToList();
        }

        //Liam
        public List<string> GetAbioVoedingsbehoefte()
        {
            return _context.AbioVoedingsbehoefte.Select(s => s.Voedingsbehoefte).ToList();
        }

        //get the different values of the habitats (Lily)
        public List<string> GetAbioHabitatNames()
        {
            return _context.AbioHabitat.Select(ah => ah.Waarde).ToList();
        }

        //get the names of the different habitats in the current plant
        public List<string> GetAbioHabitatNames(Plant plant)
        {
            var habitatsAbbreviations = plant.AbiotiekMulti.Select(am => am.Waarde);

            return _context.AbioHabitat
                .Where(ah => habitatsAbbreviations.Contains(ah.Afkorting))
                .Select(ah => ah.Waarde)
                .ToList();
        }

        //Get the different abbreviations of the habitats(Lily)
        public List<string> GetAbioHabitatAbbreviations()
        {
            return _context.AbioHabitat.Select(s => s.Afkorting).ToList();
        }

        //get a single abbreviation of the habitat (Lily)
        public string GetAbioHabitatAbbreviation(string value)
        {
            var habitat = _context.AbioHabitat.First(ah => ah.Waarde == value);

            return habitat.Afkorting;
        }

        //Liam
        public List<string> GetAbioAntagonischeOmgeving()
        {
            return _context.AbioReactieAntagonischeOmg.Select(s => s.Antagonie).ToList();
        }
        //get the different values of the Commensalisme property (Liam)
        public List<CommLevensvorm> GetCommLevensvorm()
        {
            return _context.CommLevensvorm.ToList();
        }
        //Liam
        public List<string> GetCommStrategie()
        {
            return _context.CommStrategie.Select(s => s.Strategie).ToList();
        }

        public List<string> GetCommOntwikkelsnelheid()
        {
            return _context.CommOntwikkelsnelheid.Select(s => s.Snelheid).ToList();
        }
        //Liam
        public List<CommSocialbiliteit> GetCommSociabiliteit()
        {
            return _context.CommSocialbiliteit.ToList();
        }

       
        //get the abio Multis of the selected plant (Liam)
        public List<AbiotiekMulti> GetAbioMulti(Plant plant)
        {
            return _context.AbiotiekMulti.Where(s => s.PlantId == plant.PlantId).ToList();

        }
        //get the commensalisme multis of the selected plant (Liam)
        public List<CommensalismeMulti> GetCommensalismeMulti(Plant plant)
        {
            return _context.CommensalismeMulti.Where(s => s.PlantId == plant.PlantId).ToList();
        }

        //get the different values in the tables (Jim)
        public List<string> GetExtraNectarwaarde()
        {
            return _context.ExtraNectarwaarde.Select(s => s.Waarde).ToList();
        }
        public List<string> GetExtraPollenwaarde()
        {
            return _context.ExtraPollenwaarde.Select(s => s.Waarde).ToList();
        }

        public List<string> GetFenoBladGrootte()
        {
            return _context.FenoBladgrootte.Select(s => s.Bladgrootte).ToList();
        }

        public List<string> GetFenoBladVorm()
        {
            return _context.FenoBladvorm.Select(s => s.Vorm).ToList();
        }
        public List<string> GetFenoBloeiWijze()
        {
            return _context.FenoBloeiwijze.Select(s => s.Naam).ToList();
        }
        public List<string> GetFenoHabitus()
        {
            return _context.FenoHabitus.Select(s => s.Naam).ToList();
        }
        public List<FenoKleur> GetFenoKleur()
        {
            return _context.FenoKleur.ToList();
        }
        public List<string> GetFenoLevensVorm()
        {
            return _context.FenoLevensvorm.Select(s => s.Levensvorm).ToList();
        }
        public List<string> GetFenoFenologie()
        {
            return _context.FenoSpruitfenologie.Select(s => s.Fenologie).ToList();
        }
        public List<FenotypeMulti> GetFenoMultis(Plant plant)
        {
            return _context.FenotypeMulti.Where(f => f.PlantId == plant.PlantId).ToList();
        }

        //get all management acts of the current plant (Davy)
        public List<BeheerMaand> GetBeheerMaanden(Plant plant)
        {
            return _context.BeheerMaand.Where(b => b.PlantId == plant.PlantId).ToList();
        }

        //create a management act (Davy, Lily)
        public string CreateBeheerMaand(BeheerMaand beheerMaand)
        {
            string message = "";
            var item = _context.BeheerMaand.Where(b => b.PlantId == beheerMaand.PlantId);

            _context.BeheerMaand.Add(beheerMaand);
            _context.SaveChanges();

            return message;
        }

        //edit the selected management act (Davy)
        public void EditBeheerMaand(BeheerMaand beheerMaand)
        {
            _context.BeheerMaand.Update(beheerMaand);
            _context.SaveChanges();
        }

        //delete the selected management act (Davy)
        public void RemoveBeheerMaand(BeheerMaand beheerMaand)
        {
            _context.BeheerMaand.Remove(beheerMaand);
            _context.SaveChanges();

        }

        //create an account for a user(Davy)
        public void CreateLogin(Gebruiker gebruiker)
        {
            _context.Gebruiker.Add(gebruiker);
            _context.SaveChanges();
        }
        //check the login of the user(Davy)
        public bool CheckLogin(string emailadress, string password, out string message)
        {
            //Gebruiker gebruiker = new Gebruiker();
            //gebruiker.Emailadres = emailadress;
            //gebruiker.Rol = "gebruiker";
            //gebruiker.HashPaswoord = Encryptor.GenerateMD5Hash(password);

            // maak gebruiker aan in database met hash waarde voor wachtwoord
            //CreateLogin(gebruiker);

            message = "";


            var user = _context.Gebruiker.SingleOrDefault(g => g.Emailadres == emailadress && g.HashPaswoord == (Encryptor.GenerateMD5Hash(password)));

            if (user != null)
            {
                message = "U bent geverifieerd, even geduld ...";
                return true;
            }

            message = "Gebruiker niet gevonden, Controleer of u juiste email en paswoord gebruikt";
            return false;

        }
      
        //get the selected user (Jim)
        public Gebruiker GetGebruiker(string emailadres)
        {
            return _context.Gebruiker.SingleOrDefault(g => g.Emailadres.Equals(emailadres));

        }

        public Fenotype GetFenotypeFromPlant(Plant plant)
        {
            return _context.Fenotype.Where(i => i.PlantId == plant.PlantId).SingleOrDefault();
        }
        public Commensalisme GetCommensialisme(Plant plant)
        {
            return _context.Commensalisme.Where(i => i.PlantId == plant.PlantId).SingleOrDefault();
        }
        //get the values for the foto properties, in excel there are currently 3 different properties asked(Lily)
        public List<string> GetFotoEigenschappen()
        {
            return new()
            {
                "Blad",
                "Bloei",
                "Habitus"
            };
        }
        //add an image to the selected plant(Lily)
        public void AddFoto(string? property, Plant plant, string? url, byte[]? imageBytes)
        {
            var foto = new Foto
            {
                PlantNavigation = plant,
                Eigenschap = property,
                UrlLocatie = url,
                Tumbnail = imageBytes
            };

            _context.Foto.Add(foto);

            _context.SaveChanges();
        }

        //edit the current photo(Lily)
        public void ChangeFoto(Foto foto, string? property, string? url, byte[]? imageBytes)
        {
            foto.Eigenschap = property;
            foto.UrlLocatie = url;
            foto.Tumbnail = imageBytes;

            _context.Foto.Add(foto);
            //Entry is to check if there is already an image in the database, if there is none it gets added
            //if there is one it gets edited instead of adding one. (Lily)
            _context.Entry(foto)
                .State = EntityState.Modified;

            _context.SaveChanges();
        }
        //delete the selected photo(Lily)
        public void DeleteFoto(Foto foto)
        {
            _context.Foto
                .Remove(foto);

            _context.SaveChanges();
        }

        //get the current roles in the database
        public List<string> GetRoles()
        {
            return _context.Rol.Select(r => r.Omschrijving).ToList();

           
        }
    }
}