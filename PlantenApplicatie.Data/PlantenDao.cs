using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.Data
{
    // De PlantenDao Klasse (Jim&Davy&Liam&Zakaria&Lily)
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

        // Geef alle planten terug met al hun properties van de andere tabellen (Lily & davy)
        public List<Plant> GetPlants()
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

        public List<ExtraEigenschap> GetExtraProperties(Plant selectedPlant)
        {
            return _context.ExtraEigenschap.Where(p => p.PlantId == selectedPlant.PlantId).ToList();
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
                .Select(p =>
                {
                    p.Variant ??= NoVariant;
                    return p;
                })
                .OrderBy(p => p.Fgsv)
                .ToList();
        }

        public string CreateExtraProperty(ExtraEigenschap extraProperty)
        {
            string message = "";
            var item = _context.ExtraEigenschap.Where(b => b.PlantId == extraProperty.PlantId);

            if (item.Count() == 1)
            {
                message = "You can only add 1 extra property.";
            }
            else
            {
                _context.ExtraEigenschap.Add(extraProperty);
                _context.SaveChanges();
            }

            return message;
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

        public void EditExtraProperty(ExtraEigenschap extraProperty)
        {
            _context.ExtraEigenschap.Update(extraProperty);
            _context.SaveChanges();
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

        public void DeleteExtraProperty(ExtraEigenschap extraProperty)
        {
            _context.ExtraEigenschap.Remove(extraProperty);
            _context.SaveChanges();
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
        public void ChangePlant(Plant plant, string? type, string? family, string? genus, string? species, string? variant, short? plantMin, short? plantMax)
        {
            //haal de id's van de verschillende types op
            var typeId = (int?)GetTypeId(type);
            var familyId = (int?)GetFamilyId(family);
            var genusId = (int?)GetGenusId(genus);
            var speciesId = (int?)GetSpeciesId(species);
            var variantId = (int?)GetVariantId(variant);
            //verander de gegevens van de plant op basis van wat er geselecteerd word, word er niks geselecteerd
            //dan verandert de waarde niet (Jim)

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

            //de plantnaam verandert mee als er iets verandert in de FGSV volgorde. (Jim)
            plant.Fgsv = plant.Familie + " " + plant.Geslacht + " " + plant.Soort + " " + plant.Variant;


            _context.SaveChanges();
        }

        //Voeg een fenotype toe aan de geselecteerde plant (Jim)
        public void AddFenotype(Plant plant,int leafSize,string leafShape,string ratioBloomLeaf,string Bloom,
        string habitus, string lifeForm,string sprout)
        {
            var fenotypePlant = new Fenotype
            {
                PlantId = plant.PlantId,
                Bladgrootte = leafSize,
                Bladvorm = leafShape,
                RatioBloeiBlad = ratioBloomLeaf,
                Bloeiwijze = Bloom,
                Habitus = habitus,
                Levensvorm = lifeForm,
                Spruitfenologie = sprout
            };


            _context.Fenotype.Add(fenotypePlant);
            _context.SaveChanges();
        }

        //verander een fenotype van de geselecteerde plant (Jim)
        public Fenotype ChangeFenotype(Plant plant, int? leafSize, string leafShape, string ratioBloomLeaf, string Bloom,
            string habitus, string lifeForm,string sprout)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.PlantId == plant.PlantId);

            selectedfenotype.Bladgrootte = leafSize ?? selectedfenotype.Bladgrootte;
            selectedfenotype.Bladvorm = leafShape ?? selectedfenotype.Bladvorm;
            selectedfenotype.RatioBloeiBlad = ratioBloomLeaf ?? selectedfenotype.RatioBloeiBlad;
            selectedfenotype.Bloeiwijze = Bloom ?? selectedfenotype.Bloeiwijze;
            selectedfenotype.Habitus = habitus ?? selectedfenotype.Habitus;
            selectedfenotype.Levensvorm = lifeForm ?? selectedfenotype.Levensvorm;
            selectedfenotype.Spruitfenologie = sprout ?? selectedfenotype.Spruitfenologie;
            _context.SaveChanges();

            return selectedfenotype;
        }
        //verwijder de fenotype van de geselecteerde plant (Jim)
        public void DeleteFenotype(Fenotype fenotype)
        {
            var selectedfenotype = _context.Fenotype.FirstOrDefault(i => i.Id == fenotype.Id);

            _context.Fenotype.Remove(selectedfenotype);
            _context.SaveChanges();
        }
        //voeg een multifenotype toe (Jim)

        public void AddMultiFenotype(Plant plant, string property, string month, string value)
        {
            var fenotypeMultiPlant = new FenotypeMulti
            {
                Id = GetLastFenoMultiId(),
                PlantId = plant.PlantId,
                Eigenschap = property,
                Maand = month,
                Waarde = value
            };
            _context.FenotypeMulti.Add(fenotypeMultiPlant);
            _context.SaveChanges();
        }

        //haal de laatste ID op van fenotype multi
        public long GetLastFenoMultiId()
        {
            var fenotypeMulti = _context.FenotypeMulti.Count();
            if (fenotypeMulti == null)
            {
                return 0;
            }

            return fenotypeMulti;
        }


        //verander een multifenotype van de geselecteerde plant (Jim)

        public void ChangeMultiFenotype(FenotypeMulti fenotypeMulti, string property, string month, string value)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);

            selectedFenotypeMulti.Eigenschap = property ?? selectedFenotypeMulti.Eigenschap;
            selectedFenotypeMulti.Maand = month ?? selectedFenotypeMulti.Maand;
            selectedFenotypeMulti.Waarde = value ?? selectedFenotypeMulti.Waarde;

            _context.SaveChanges();
        }
        //verwijder een multifenotype van de geselecteerde plant (Jim)
        public void RemoveMultiPhenotype(FenotypeMulti fenotypeMulti)
        {
            var selectedFenotypeMulti = _context.FenotypeMulti.FirstOrDefault(i => i.Id == fenotypeMulti.Id);
            _context.Remove(fenotypeMulti);
            _context.SaveChanges();
        }
        //voeg een extra eigenschap aan de geselecteerde plant (Jim)
        public void AddExtraProperty(Plant plant, string nectarValue, string pollenValue, bool bee, bool butterfly,
            bool edible, bool herb, bool odor, bool frost)
        {
            var extraProperty = new ExtraEigenschap
            {
                PlantId = plant.PlantId,
                Nectarwaarde = nectarValue,
                Pollenwaarde = pollenValue,
                Bijvriendelijke = bee,
                Vlindervriendelijk = butterfly,
                Eetbaar = edible,
                Kruidgebruik = herb,
                Geurend = odor,
                Vorstgevoelig = frost
            };

            _context.ExtraEigenschap.Add(extraProperty);
            _context.SaveChanges();
        }
        //verander de extra eigenschap aan de geselecteerde plant (Jim)

        public void EditExtraProperty(ExtraEigenschap extraProperty, string nectarValue, string pollenValue, bool bee, bool butterfly,
         bool edible, bool herb, bool odor, bool frost)
        {
            var selectedExtraProperty = _context.ExtraEigenschap.FirstOrDefault(i => i.Id == extraProperty.Id);

            selectedExtraProperty.Nectarwaarde = nectarValue ?? selectedExtraProperty.Nectarwaarde;
            selectedExtraProperty.Pollenwaarde = pollenValue ?? selectedExtraProperty.Pollenwaarde;
            selectedExtraProperty.Bijvriendelijke = bee;
            selectedExtraProperty.Vlindervriendelijk = butterfly;
            selectedExtraProperty.Eetbaar = edible;
            selectedExtraProperty.Kruidgebruik = herb;
            selectedExtraProperty.Geurend = odor;
            selectedExtraProperty.Vorstgevoelig = frost;


            _context.SaveChanges();
        }
      
        //Toevoegen, veranderen en verwijderen van abiotiek (Liam)
        public void AddAbiotiek(Plant plant, string? insolation, string? soilType, string? moistureRequirement, string? nutritionNeeds, string? antagonisticEnvironment)
        {
            //Habitat ontbreekt
            Abiotiek abiotiek = new Abiotiek
            {
                PlantId = plant.PlantId,
                Bezonning = insolation,
                Grondsoort = soilType,
                Vochtbehoefte = moistureRequirement,
                Voedingsbehoefte = nutritionNeeds,
                AntagonischeOmgeving = antagonisticEnvironment
            };

            _context.Add(abiotiek);

            _context.SaveChanges();


        }

        public void ChangeAbiotiek(Abiotiek abiotiek, string? insolation, string? soilType,
            string? moistureRequirement, string? nutritionNeeds, string? antagonisticEnvironment)
        {


            var selectedAbiotiek = _context.Abiotiek.FirstOrDefault(s => s.Id == abiotiek.Id);

            selectedAbiotiek.Bezonning = insolation ?? selectedAbiotiek.Bezonning;
            selectedAbiotiek.Grondsoort = soilType ?? selectedAbiotiek.Grondsoort;
            selectedAbiotiek.Vochtbehoefte = moistureRequirement ?? selectedAbiotiek.Vochtbehoefte;
            selectedAbiotiek.Voedingsbehoefte = nutritionNeeds ?? selectedAbiotiek.Voedingsbehoefte;
            selectedAbiotiek.AntagonischeOmgeving = antagonisticEnvironment ?? selectedAbiotiek.AntagonischeOmgeving;



            _context.SaveChanges();
        }

        public void DeleteAbiotiek(Abiotiek abiotiek)
        {
            var selectedAbiotiek = _context.Abiotiek.FirstOrDefault(s => s.Id == abiotiek.Id);

            _context.Abiotiek.Remove(selectedAbiotiek);

            _context.SaveChanges();
        }

        //Toevoegen, veranderen en verwijderen van commensalisme (Liam)

        public void AddCommensalisme(Plant plant, string developmentSpeed, string strategy)
        {
            //sociabiliteit ontbreekt
            Commensalisme commensalisme = new Commensalisme
            {
                PlantId = plant.PlantId,
                Ontwikkelsnelheid = developmentSpeed,
                Strategie = strategy
            };

            _context.Add(commensalisme);

            _context.SaveChanges();


        }

        public Commensalisme ChangeCommensalisme(Plant plant, string developmentSpeed, string strategy)
        {
            var selectedCommensalisme = _context.Commensalisme.FirstOrDefault(i => i.PlantId == plant.PlantId);

            selectedCommensalisme.Ontwikkelsnelheid = developmentSpeed ?? selectedCommensalisme.Ontwikkelsnelheid;
            selectedCommensalisme.Strategie = strategy ?? selectedCommensalisme.Strategie;

            _context.SaveChanges();

            return selectedCommensalisme;
        }
        public void DeleteCommensalisme(Commensalisme commensalisme)
        {
            var selectedCommensalisme = _context.Commensalisme.FirstOrDefault(s => s.Id == commensalisme.Id);

            _context.Commensalisme.Remove(selectedCommensalisme);

            _context.SaveChanges();
        }

        public void AddAbiotiekMulti(Plant plant, string property, string value)
        {

            AbiotiekMulti abiotiekMulti = new AbiotiekMulti
            {
                PlantId = plant.PlantId,
                Eigenschap = property,
                Waarde = value
            };

            _context.Add(abiotiekMulti);

            _context.SaveChanges();


        }

        public void ChangeAbiotiekMulti(AbiotiekMulti abiotiekMulti, string property, string value)
        {
            var selectedAbiotiekMulti = _context.AbiotiekMulti.FirstOrDefault(s => s.Id == abiotiekMulti.Id);

            selectedAbiotiekMulti.Eigenschap = property ?? selectedAbiotiekMulti.Eigenschap;
            selectedAbiotiekMulti.Waarde = value ?? selectedAbiotiekMulti.Waarde;

            _context.SaveChanges();
        }

        public void DeleteAbioticMulti(AbiotiekMulti abiotiekMulti)
        {
            var selectedAbiotiekMulti = _context.AbiotiekMulti.FirstOrDefault(s => s.Id == abiotiekMulti.Id);

            _context.AbiotiekMulti.Remove(selectedAbiotiekMulti);

            _context.SaveChanges();
        }

        public void AddCommensalismeMulti(Plant plant, string property, string value)
        {

            CommensalismeMulti commensalismeMulti = new CommensalismeMulti
            {
                PlantId = plant.PlantId,
                Eigenschap = property,
                Waarde = value
            };

            _context.Add(commensalismeMulti);

            _context.SaveChanges();


        }

        public void ChangeCommensalismeMulti(CommensalismeMulti commensalismeMulti, string property, string value)
        {
            var selectedCommensalismeMulti = _context.CommensalismeMulti.FirstOrDefault(s => s.Id == commensalismeMulti.Id);

            selectedCommensalismeMulti.Eigenschap = property ?? selectedCommensalismeMulti.Eigenschap;
            selectedCommensalismeMulti.Waarde = value ?? selectedCommensalismeMulti.Waarde;

            _context.SaveChanges();
        }

        public void DeleteCommensalismMulti(CommensalismeMulti commensalismeMulti)
        {
            var selectedCommensalismeMulti = _context.CommensalismeMulti.FirstOrDefault(s => s.Id == commensalismeMulti.Id);

            _context.CommensalismeMulti.Remove(selectedCommensalismeMulti);

            _context.SaveChanges();
        }

        //Liam
        public List<string> GetAbioInsolation()
        {
            return _context.AbioBezonning.Select(s => s.Naam).ToList();
        }
        //Liam
        public List<string> GetAbioSoilType()
        {
            return _context.AbioGrondsoort.Select(s => s.Grondsoort).ToList();
        }
        //Liam
        public List<string> GetAbioMoistureRequirement()
        {
            return _context.AbioVochtbehoefte.Select(s => s.Vochtbehoefte).ToList();
        }

        //Liam
        public List<string> GetAbioNutritionRequirement()
        {
            return _context.AbioVoedingsbehoefte.Select(s => s.Voedingsbehoefte).ToList();
        }

        public List<string> GetAbioHabitatNames()
        {
            return _context.AbioHabitat.Select(ah => ah.Waarde).ToList();
        }

        public List<string> GetAbioHabitatNames(Plant plant)
        {
            var habitatsAbbreviations = plant.AbiotiekMulti.Select(am => am.Waarde);

            return _context.AbioHabitat
                .Where(ah => habitatsAbbreviations.Contains(ah.Afkorting))
                .Select(ah => ah.Waarde)
                .ToList();
        }

        //Liam, Lily
        public List<string> GetAbioHabitatAbbreviations()
        {
            return _context.AbioHabitat.Select(s => s.Afkorting).ToList();
        }

        public string GetAbioHabitatAbbreviation(string value)
        {
            var habitat = _context.AbioHabitat.First(ah => ah.Waarde == value);

            return habitat.Afkorting;
        }

        //Liam
        public List<string> GetAbioAntagonianEnvironments()
        {
            return _context.AbioReactieAntagonischeOmg.Select(s => s.Antagonie).ToList();
        }
        //Liam
        public List<CommLevensvorm> GetCommLifeform()
        {
            return _context.CommLevensvorm.ToList();
        }
        //Liam
        public List<string> GetCommStrategy()
        {
            return _context.CommStrategie.Select(s => s.Strategie).ToList();
        }

        public List<string> GetCommDevelopmentSpeed()
        {
            return _context.CommOntwikkelsnelheid.Select(s => s.Snelheid).ToList();
        }
        //Liam
        public List<CommSocialbiliteit> GetCommSociability()
        {
            return _context.CommSocialbiliteit.ToList();
        }

       
        //Liam
        public List<AbiotiekMulti> GetAbioMulti(Plant plant)
        {
            return _context.AbiotiekMulti.Where(s => s.PlantId == plant.PlantId).ToList();

        }
        //Liam
        public List<CommensalismeMulti> GetCommensalismMulti(Plant plant)
        {
            return _context.CommensalismeMulti.Where(s => s.PlantId == plant.PlantId).ToList();
        }

        //Haal alle waardes op in een lijst voor gebruik (Jim)
        public List<string> GetExtraNectarValue()
        {
            return _context.ExtraNectarwaarde.Select(s => s.Waarde).ToList();
        }
        public List<string> GetExtraPollenValue()
        {
            return _context.ExtraPollenwaarde.Select(s => s.Waarde).ToList();
        }

        public List<string> GetPhenoLeafSize()
        {
            return _context.FenoBladgrootte.Select(s => s.Bladgrootte).ToList();
        }

        public List<string> GetPhenoLeafShape()
        {
            return _context.FenoBladvorm.Select(s => s.Vorm).ToList();
        }
        public List<string> GetPhenoInflorescence()
        {
            return _context.FenoBloeiwijze.Select(s => s.Naam).ToList();
        }
        public List<string> GetPhenoHabitat()
        {
            return _context.FenoHabitus.Select(s => s.Naam).ToList();
        }
        public List<FenoKleur> GetPhenoColour()
        {
            return _context.FenoKleur.ToList();
        }
        public List<string> GetPhenoLifeform()
        {
            return _context.FenoLevensvorm.Select(s => s.Levensvorm).ToList();
        }
        public List<string> GetPhenoSproutPhenology()
        {
            return _context.FenoSpruitfenologie.Select(s => s.Fenologie).ToList();
        }
        public List<FenotypeMulti> GetPhenoMultis(Plant plant)
        {
            return _context.FenotypeMulti.Where(f => f.PlantId == plant.PlantId).ToList();
        }

        // haal beheermaanden per plant (Davy)
        public List<BeheerMaand> GetManagementActs(Plant plant)
        {
            return _context.BeheerMaand.Where(b => b.PlantId == plant.PlantId).ToList();
        }

        // maak een BeheerMaand aan (Davy, Lily)
        public string CreateManagementAct(BeheerMaand beheerMaand)
        {
            string message = "";
            var item = _context.BeheerMaand.Where(b => b.PlantId == beheerMaand.PlantId);

            _context.BeheerMaand.Add(beheerMaand);
            _context.SaveChanges();

            return message;
        }

        // wijzig een BeheerMaand (Davy)
        public void EditManagementAct(BeheerMaand beheerMaand)
        {
            _context.BeheerMaand.Update(beheerMaand);
            _context.SaveChanges();
        }

        // verwijder een BeheerMaand uit database (Davy)
        public void RemoveManagementAct(BeheerMaand beheerMaand)
        {
            _context.BeheerMaand.Remove(beheerMaand);
            _context.SaveChanges();

        }


        public void CreateLogin(User gebruiker)
        {
            _context.Gebruiker.Add(gebruiker);
            _context.SaveChanges();
        }

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
      

        public User GetUser(string emailadres)
        {
            return _context.Gebruiker.SingleOrDefault(g => g.Emailadres.Equals(emailadres));

        }

        public Fenotype GetPhenotypeFromPlant(Plant plant)
        {
            return _context.Fenotype.Where(i => i.PlantId == plant.PlantId).SingleOrDefault();
        }
        public Commensalisme GetCommensialism(Plant plant)
        {
            return _context.Commensalisme.Where(i => i.PlantId == plant.PlantId).SingleOrDefault();
        }

        public List<string> GetImageProperties()
        {
            return new()
            {
                "Blad",
                "Bloei",
                "Habitus"
            };
        }

        public void AddPhoto(string? property, Plant plant, string? url, byte[]? imageBytes)
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

        public void ChangePhoto(Foto photo, string? property, string? url, byte[]? imageBytes)
        {
            photo.Eigenschap = property;
            photo.UrlLocatie = url;
            photo.Tumbnail = imageBytes;

            _context.Foto.Add(photo);
            _context.Entry(photo)
                .State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void DeletePhoto(Foto photo)
        {
            _context.Foto
                .Remove(photo);

            _context.SaveChanges();
        }
    }
}