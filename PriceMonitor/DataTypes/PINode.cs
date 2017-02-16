using System.Collections.Generic;

namespace PriceMonitor.DataTypes
{
	public enum PITier
	{
		Raw, Basic, Refined, Specialized, Advanced
	}

	public class PINode
	{
		public List<int> From = null;
		public List<int> To = null;

		public string Name { get; set; }
		public int ID { get; set; }

		public PITier Tier = PITier.Raw;

		public static List<PINode> AllPlanetaryItems = CreatePINodes();

		private static List<PINode> CreatePINodes()
		{
			return new List<PINode>
			{
				//tier 4
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.BroadcastNode),
					ID = PIConst.BroadcastNodeID,
					From = new List<int>() {PIConst.DataChipsID, PIConst.HighTechTransmittersID, PIConst.NeocomsID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.IntegrityResponseDrones),
					ID = PIConst.IntegrityResponseDronesID,
					From = new List<int>() {PIConst.GelMatrixBiopasteID, PIConst.HazmatDetectionSystemsID, PIConst.PlanetaryVehiclesID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.NanoFactory),
					ID = PIConst.NanoFactoryID,
					From = new List<int>() {PIConst.IndustrialExplosivesID, PIConst.UkomiSuperconductorsID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.OrganicMortarApplicators),
					ID = PIConst.OrganicMortarApplicatorsID,
					From = new List<int>() {PIConst.CondensatesID, PIConst.RoboticsID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.RecursiveComputingModule),
					ID = PIConst.RecursiveComputingModuleID,
					From = new List<int>() {PIConst.GuidanceSystemsID, PIConst.SyntheticSynapsesID, PIConst.TranscranialMicrocontrollersID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.SelfHarmonizingPowerCore),
					ID = PIConst.SelfHarmonizingPowerCoreID,
					From = new List<int>() {PIConst.CameraDronesID, PIConst.HermeticMembranesID, PIConst.NuclearReactorsID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.SterileConduits),
					ID = PIConst.SterileConduitsID,
					From = new List<int>() {PIConst.SmartfabUnitsID, PIConst.VaccinesID}
				},
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.WetwareMainframe),
					ID = PIConst.WetwareMainframeID,
					From = new List<int>() {PIConst.BiotechResearchReportsID, PIConst.CryoprotectantSolutionID, PIConst.SupercomputersID}
				},
				// tier3
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.BiotechResearchReports),
					ID = PIConst.BiotechResearchReportsID,
					From = new List<int>() {PIConst.ConstructionBlocksID, PIConst.LivestockID, PIConst.NanitesID},
					To = new List<int>() {PIConst.WetwareMainframeID}
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.CameraDrones),
					ID = PIConst.CameraDronesID,
					From = new List<int>() {PIConst.RocketFuelID, PIConst.SilicateGlassID},
					To = new List<int>() {PIConst.SelfHarmonizingPowerCoreID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.Condensates),
					ID = PIConst.CondensatesID,
					From = new List<int>() {PIConst.CoolantID, PIConst.OxidesID},
					To = new List<int>() {PIConst.OrganicMortarApplicatorsID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.CryoprotectantSolution),
					ID = PIConst.CryoprotectantSolutionID,
					From = new List<int>() {PIConst.FertilizerID, PIConst.TestCulturesID, PIConst.SyntheticOilID},
					To = new List<int>() {PIConst.WetwareMainframeID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.DataChips),
					ID = PIConst.DataChipsID,
					From = new List<int>() {PIConst.MicrofiberShieldingID, PIConst.SupertensilePlasticsID},
					To = new List<int>() {PIConst.BroadcastNodeID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.GelMatrixBiopaste),
					ID = PIConst.GelMatrixBiopasteID,
					From = new List<int>() {PIConst.BiocellsID, PIConst.OxidesID, PIConst.SuperconductorsID},
					To = new List<int>() {PIConst.IntegrityResponseDronesID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.GuidanceSystems),
					ID = PIConst.GuidanceSystemsID,
					From = new List<int>() {PIConst.TransmitterID, PIConst.WaterCooledCPUID},
					To = new List<int>() {PIConst.RecursiveComputingModuleID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.HazmatDetectionSystems),
					ID = PIConst.HazmatDetectionSystemsID,
					From = new List<int>() {PIConst.PolytextilesID, PIConst.TransmitterID, PIConst.ViralAgentID},
					To = new List<int>() {PIConst.IntegrityResponseDronesID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.HermeticMembranes),
					ID = PIConst.HermeticMembranesID,
					From = new List<int>() {PIConst.GeneticallyEnhancedLivestockID, PIConst.PolyaramidsID},
					To = new List<int>() {PIConst.SelfHarmonizingPowerCoreID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.HighTechTransmitters),
					ID = PIConst.HighTechTransmittersID,
					From = new List<int>() {PIConst.TransmitterID, PIConst.PolyaramidsID},
					To = new List<int>() {PIConst.BroadcastNodeID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.IndustrialExplosives),
					ID = PIConst.IndustrialExplosivesID,
					From = new List<int>() {PIConst.FertilizerID, PIConst.PolytextilesID},
					To = new List<int>() {PIConst.NanoFactoryID}
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.Neocoms),
					ID = PIConst.NeocomsID,
					From = new List<int>() {PIConst.BiocellsID, PIConst.SilicateGlassID},
					To = new List<int>() {PIConst.BroadcastNodeID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.NuclearReactors),
					ID = PIConst.NuclearReactorsID,
					From = new List<int>() {PIConst.EnrichedUraniumID, PIConst.MicrofiberShieldingID},
					To = new List<int>() {PIConst.SelfHarmonizingPowerCoreID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.PlanetaryVehicles),
					ID = PIConst.PlanetaryVehiclesID,
					From = new List<int>() {PIConst.MechanicalPartsID, PIConst.MiniatureElectronicsID, PIConst.SupertensilePlasticsID},
					To = new List<int>() {PIConst.IntegrityResponseDronesID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.Robotics),
					ID = PIConst.RoboticsID,
					From = new List<int>() {PIConst.ConsumerElectronicsID, PIConst.MechanicalPartsID},
					To = new List<int>() {PIConst.OrganicMortarApplicatorsID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.SmartfabUnits),
					ID = PIConst.SmartfabUnitsID,
					From = new List<int>() {PIConst.ConstructionBlocksID, PIConst.MiniatureElectronicsID},
					To = new List<int>() {PIConst.SterileConduitsID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.Supercomputers),
					ID = PIConst.SupercomputersID,
					From = new List<int>() {PIConst.ConsumerElectronicsID, PIConst.CoolantID, PIConst.WaterCooledCPUID},
					To = new List<int>() {PIConst.WetwareMainframeID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.SyntheticSynapses),
					ID = PIConst.SyntheticSynapsesID,
					From = new List<int>() {PIConst.SupertensilePlasticsID, PIConst.TestCulturesID},
					To = new List<int>() {PIConst.RecursiveComputingModuleID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.TranscranialMicrocontrollers),
					ID = PIConst.TranscranialMicrocontrollersID,
					From = new List<int>() {PIConst.BiocellsID, PIConst.NanitesID},
					To = new List<int>() {PIConst.RecursiveComputingModuleID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.UkomiSuperconductors),
					ID = PIConst.UkomiSuperconductorsID,
					From = new List<int>() {PIConst.SuperconductorsID, PIConst.SyntheticOilID},
					To = new List<int>() {PIConst.NanoFactoryID }
				},
				new PINode()
				{
					Tier = PITier.Specialized,
					Name = nameof(PIConst.Vaccines),
					ID = PIConst.VaccinesID,
					From = new List<int>() {PIConst.LivestockID, PIConst.ViralAgentID},
					To = new List<int>() {PIConst.SterileConduitsID }
				},
				//tier2
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Biocells),
					ID = PIConst.BiocellsID,
					From = new List<int>() {PIConst.BiofuelsID, PIConst.PreciousMetalsID},
					To = new List<int>() {PIConst.GelMatrixBiopasteID, PIConst.NeocomsID , PIConst.TranscranialMicrocontrollersID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.ConstructionBlocks),
					ID = PIConst.ConstructionBlocksID,
					From = new List<int>() {PIConst.ReactiveMetalsID, PIConst.ToxicMetalsID},
					To = new List<int>() {PIConst.BiotechResearchReportsID, PIConst.SmartfabUnitsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.ConsumerElectronics),
					ID = PIConst.ConsumerElectronicsID,
					From = new List<int>() {PIConst.ToxicMetalsID, PIConst.ChiralStructuresID},
					To = new List<int>() {PIConst.RoboticsID, PIConst.SupercomputersID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Coolant),
					ID = PIConst.CoolantID,
					From = new List<int>() {PIConst.WaterID, PIConst.ElectrolytesID},
					To = new List<int>() {PIConst.CondensatesID, PIConst.SupercomputersID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.EnrichedUranium),
					ID = PIConst.EnrichedUraniumID,
					From = new List<int>() {PIConst.ToxicMetalsID, PIConst.PreciousMetalsID},
					To = new List<int>() {PIConst.NuclearReactorsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Fertilizer),
					ID = PIConst.FertilizerID,
					From = new List<int>() {PIConst.ProteinsID, PIConst.BacteriaID},
					To = new List<int>() {PIConst.CryoprotectantSolutionID, PIConst.IndustrialExplosivesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.GeneticallyEnhancedLivestock),
					ID = PIConst.GeneticallyEnhancedLivestockID,
					From = new List<int>() {PIConst.ProteinsID, PIConst.BiomassID},
					To = new List<int>() {PIConst.HermeticMembranesID}
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Livestock),
					ID = PIConst.LivestockID,
					From = new List<int>() {PIConst.ProteinsID, PIConst.BiofuelsID},
					To = new List<int>() {PIConst.BiocellsID, PIConst.VaccinesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.MechanicalParts),
					ID = PIConst.MechanicalPartsID,
					From = new List<int>() {PIConst.ReactiveMetalsID, PIConst.PreciousMetalsID},
					To = new List<int>() {PIConst.PlanetaryVehiclesID, PIConst.RoboticsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.MicrofiberShielding),
					ID = PIConst.MicrofiberShieldingID,
					From = new List<int>() {PIConst.IndustrialFibersID, PIConst.SiliconID},
					To = new List<int>() {PIConst.DataChipsID, PIConst.NuclearReactorsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.MiniatureElectronics),
					ID = PIConst.MiniatureElectronicsID,
					From = new List<int>() {PIConst.PlanetaryVehiclesID, PIConst.SmartfabUnitsID}
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Nanites),
					ID = PIConst.NanitesID,
					From = new List<int>() {PIConst.BacteriaID, PIConst.ReactiveMetalsID},
					To = new List<int>() {PIConst.BiotechResearchReportsID, PIConst.TranscranialMicrocontrollersID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Oxides),
					ID = PIConst.OxidesID,
					From = new List<int>() {PIConst.CondensatesID, PIConst.GelMatrixBiopasteID}
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Polyaramids),
					ID = PIConst.PolyaramidsID,
					From = new List<int>() {PIConst.HermeticMembranesID , PIConst.HighTechTransmittersID}
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Polytextiles),
					ID = PIConst.PolytextilesID,
					From = new List<int>() {PIConst.BiofuelsID, PIConst.IndustrialFibersID},
					To = new List<int>() {PIConst.HazmatDetectionSystemsID, PIConst.IndustrialExplosivesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.RocketFuel),
					ID = PIConst.RocketFuelID,
					From = new List<int>() {PIConst.ElectrolytesID, PIConst.PlasmoidsID},
					To = new List<int>() {PIConst.CameraDronesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.SilicateGlass),
					ID = PIConst.SilicateGlassID,
					From = new List<int>() {PIConst.SiliconID, PIConst.OxidizingCompoundID },
					To = new List<int>() {PIConst.CameraDronesID, PIConst.NeocomsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Superconductors),
					ID = PIConst.SuperconductorsID,
					From = new List<int>() {PIConst.WaterID, PIConst.PlasmoidsID },
					To = new List<int>() {PIConst.GelMatrixBiopasteID, PIConst.UkomiSuperconductorsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.SupertensilePlastics),
					ID = PIConst.SupertensilePlasticsID,
					From = new List<int>() {PIConst.OxygenID, PIConst.BiomassID },
					To = new List<int>() {PIConst.DataChipsID, PIConst.PlanetaryVehiclesID, PIConst.SyntheticSynapsesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.SyntheticOil),
					ID = PIConst.SyntheticOilID,
					From = new List<int>() {PIConst.OxygenID, PIConst.ElectrolytesID },
					To = new List<int>() {PIConst.CryoprotectantSolutionID, PIConst.UkomiSuperconductorsID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.TestCultures),
					ID = PIConst.TestCulturesID,
					From = new List<int>() {PIConst.WaterID, PIConst.BacteriaID },
					To = new List<int>() {PIConst.CryoprotectantSolutionID, PIConst.SyntheticSynapsesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.Transmitter),
					ID = PIConst.TransmitterID,
					From = new List<int>() {PIConst.PlasmoidsID, PIConst.ChiralStructuresID },
					To = new List<int>() {PIConst.GuidanceSystemsID, PIConst.HazmatDetectionSystemsID, PIConst.HighTechTransmittersID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.ViralAgent),
					ID = PIConst.ViralAgentID,
					From = new List<int>() {PIConst.BacteriaID, PIConst.BiomassID },
					To = new List<int>() {PIConst.HazmatDetectionSystemsID, PIConst.VaccinesID }
				},
				new PINode()
				{
					Tier = PITier.Refined,
					Name = nameof(PIConst.WaterCooledCPU),
					ID = PIConst.WaterCooledCPUID,
					From = new List<int>() {PIConst.WaterID, PIConst.ReactiveMetalsID },
					To = new List<int>() {PIConst.GuidanceSystemsID, PIConst.SupercomputersID}
				},
				//tier1
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Water),
					ID = PIConst.WaterID,
					From = new List<int>() {PIConst.AqueousLiquidsID},
					To = new List<int>() {PIConst.CoolantID, PIConst.SuperconductorsID, PIConst.TestCulturesID, PIConst.WaterCooledCPUID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.IndustrialFibers),
					ID = PIConst.IndustrialFibersID,
					From = new List<int>() {PIConst.AutotrophsID},
					To = new List<int>() {PIConst.MicrofiberShieldingID, PIConst.PolyaramidsID, PIConst.PolytextilesID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.ReactiveMetals),
					ID = PIConst.ReactiveMetalsID,
					From = new List<int>() {PIConst.BaseMetalsID},
					To = new List<int>() {PIConst.ConstructionBlocksID, PIConst.MechanicalPartsID, PIConst.NanitesID, PIConst.WaterCooledCPUID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Biofuels),
					ID = PIConst.BiofuelsID,
					From = new List<int>() {PIConst.CarbonCompoundsID},
					To = new List<int>() {PIConst.BiocellsID, PIConst.LivestockID, PIConst.PolytextilesID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Proteins),
					ID = PIConst.ProteinsID,
					From = new List<int>() {PIConst.ComplexOrganismsID},
					To = new List<int>() {PIConst.FertilizerID, PIConst.LivestockID, PIConst.GeneticallyEnhancedLivestockID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Silicon),
					ID = PIConst.SiliconID,
					From = new List<int>() {PIConst.FelsicMagmaID},
					To = new List<int>() {PIConst.MicrofiberShieldingID, PIConst.MiniatureElectronicsID, PIConst.SilicateGlassID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.ToxicMetals),
					ID = PIConst.ToxicMetalsID,
					From = new List<int>() {PIConst.HeavyMetalsID},
					To = new List<int>() {PIConst.ConstructionBlocksID, PIConst.ConsumerElectronicsID, PIConst.EnrichedUraniumID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Electrolytes),
					ID = PIConst.ElectrolytesID,
					From = new List<int>() {PIConst.IonicSolutionsID},
					To = new List<int>() {PIConst.CoolantID, PIConst.RocketFuelID, PIConst.SyntheticOilID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Bacteria),
					ID = PIConst.BacteriaID,
					From = new List<int>() {PIConst.MicroOrganismsID},
					To = new List<int>() {PIConst.FertilizerID, PIConst.NanitesID, PIConst.TestCulturesID, PIConst.ViralAgentID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Oxygen),
					ID = PIConst.OxygenID,
					From = new List<int>() {PIConst.NobleGasID},
					To = new List<int>() {PIConst.OxidesID, PIConst.SupertensilePlasticsID, PIConst.SyntheticOilID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.PreciousMetals),
					ID = PIConst.PreciousMetalsID,
					From = new List<int>() {PIConst.NobleMetalsID},
					To = new List<int>() {PIConst.BiocellsID, PIConst.EnrichedUraniumID, PIConst.MechanicalPartsID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.ChiralStructures),
					ID = PIConst.ChiralStructuresID,
					From = new List<int>() {PIConst.NonCsCrystalsID},
					To = new List<int>() {PIConst.ConsumerElectronicsID, PIConst.MiniatureElectronicsID, PIConst.TransmitterID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Biomass),
					ID = PIConst.BiomassID,
					From = new List<int>() {PIConst.PlankticColoniesID},
					To = new List<int>() {PIConst.GeneticallyEnhancedLivestockID, PIConst.SupertensilePlasticsID, PIConst.ViralAgentID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.OxidizingCompound),
					ID = PIConst.OxidizingCompoundID,
					From = new List<int>() {PIConst.ReactiveGasID},
					To = new List<int>() {PIConst.OxidesID, PIConst.PolyaramidsID, PIConst.SilicateGlassID},
				},
				new PINode()
				{
					Tier = PITier.Basic,
					Name = nameof(PIConst.Plasmoids),
					ID = PIConst.PlasmoidsID,
					From = new List<int>() {PIConst.SuspendedPlasmaID},
					To = new List<int>() {PIConst.RocketFuelID, PIConst.SuperconductorsID, PIConst.TransmitterID},
				},
				//tier0
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.AqueousLiquids),
					ID = PIConst.AqueousLiquidsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.WaterID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.Autotrophs),
					ID = PIConst.AutotrophsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.IndustrialFibersID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.BaseMetals),
					ID = PIConst.BaseMetalsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.ReactiveMetalsID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.CarbonCompounds),
					ID = PIConst.CarbonCompoundsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.BiofuelsID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.ComplexOrganisms),
					ID = PIConst.ComplexOrganismsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.ProteinsID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.FelsicMagma),
					ID = PIConst.FelsicMagmaID,
					From = new List<int>(),
					To = new List<int>() {PIConst.SiliconID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.HeavyMetals),
					ID = PIConst.HeavyMetalsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.ToxicMetalsID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.IonicSolutions),
					ID = PIConst.IonicSolutionsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.ElectrolytesID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.MicroOrganisms),
					ID = PIConst.MicroOrganismsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.BacteriaID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.NobleGas),
					ID = PIConst.NobleGasID,
					From = new List<int>(),
					To = new List<int>() {PIConst.OxygenID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.NobleMetals),
					ID = PIConst.NobleMetalsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.PreciousMetalsID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.NonCsCrystals),
					ID = PIConst.NonCsCrystalsID,
					From = new List<int>(),
					To = new List<int>() {PIConst.ChiralStructuresID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.PlankticColonies),
					ID = PIConst.PlankticColoniesID,
					From = new List<int>(),
					To = new List<int>() {PIConst.BiomassID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.ReactiveGas),
					ID = PIConst.ReactiveGasID,
					From = new List<int>(),
					To = new List<int>() {PIConst.OxidizingCompoundID},
				},
				new PINode()
				{
					Tier = PITier.Raw,
					Name = nameof(PIConst.SuspendedPlasma),
					ID = PIConst.SuspendedPlasmaID,
					From = new List<int>(),
					To = new List<int>() {PIConst.PlasmoidsID},
				}
			};
		}
	}

	public static class PIConst
	{
		//tier4
		public static string BroadcastNode { get; }
		public static int BroadcastNodeID { get; set; } = 2867;

		public static string IntegrityResponseDrones { get; }
		public static int IntegrityResponseDronesID { get; set; } = 2868;

		public static string NanoFactory { get; }
		public static int NanoFactoryID { get; set; } = 2869;

		public static string OrganicMortarApplicators { get; }
		public static int OrganicMortarApplicatorsID { get; set; } = 2870;

		public static string RecursiveComputingModule { get; }
		public static int RecursiveComputingModuleID { get; set; } = 2871;

		public static string SelfHarmonizingPowerCore { get; }
		public static int SelfHarmonizingPowerCoreID { get; set; } = 2872;

		public static string SterileConduits { get; }
		public static int SterileConduitsID { get; set; } = 2875;

		public static string WetwareMainframe { get; }
		public static int WetwareMainframeID { get; set; } = 2876;

		//tier3
		public static string Condensates { get; }
		public static int CondensatesID { get; set; } = 2344;

		public static string CameraDrones { get; }
		public static int CameraDronesID { get; set; } = 2345;

		public static string SyntheticSynapses { get; }
		public static int SyntheticSynapsesID { get; set; } = 2346;

		public static string GelMatrixBiopaste { get; }
		public static int GelMatrixBiopasteID { get; set; } = 2348;

		public static string Supercomputers { get; }
		public static int SupercomputersID { get; set; } = 2349;

		public static string SmartfabUnits { get; }
		public static int SmartfabUnitsID { get; set; } = 2351;

		public static string NuclearReactors { get; }
		public static int NuclearReactorsID { get; set; } = 2352;

		public static string Neocoms { get; }
		public static int NeocomsID { get; set; } = 2354;

		public static string BiotechResearchReports { get; }
		public static int BiotechResearchReportsID { get; set; } = 2358;

		public static string IndustrialExplosives { get; }
		public static int IndustrialExplosivesID { get; set; } = 2360;

		public static string HermeticMembranes { get; }
		public static int HermeticMembranesID { get; set; } = 2361;

		public static string HazmatDetectionSystems { get; }
		public static int HazmatDetectionSystemsID { get; set; } = 2366;

		public static string CryoprotectantSolution { get; }
		public static int CryoprotectantSolutionID { get; set; } = 2367;

		public static string GuidanceSystems { get; }
		public static int GuidanceSystemsID { get; set; } = 9834;

		public static string PlanetaryVehicles { get; }
		public static int PlanetaryVehiclesID { get; set; } = 9846;

		public static string Robotics { get; }
		public static int RoboticsID { get; set; } = 9848;

		public static string TranscranialMicrocontrollers { get; }
		public static int TranscranialMicrocontrollersID { get; set; } = 12836;

		public static string UkomiSuperconductors { get; }
		public static int UkomiSuperconductorsID { get; set; } = 17136;

		public static string DataChips { get; }
		public static int DataChipsID { get; set; } = 17392;

		public static string HighTechTransmitters { get; }
		public static int HighTechTransmittersID { get; set; } = 17898;

		public static string Vaccines { get; }
		public static int VaccinesID { get; set; } = 28974;

		//tier2
		public static string EnrichedUranium { get; }
		public static int EnrichedUraniumID { get; set; } = 44;

		public static string SupertensilePlastics { get; }
		public static int SupertensilePlasticsID { get; set; } = 2312;

		public static string Oxides { get; }
		public static int OxidesID { get; set; } = 2317;

		public static string TestCultures { get; }
		public static int TestCulturesID { get; set; } = 2319;

		public static string Polyaramids { get; }
		public static int PolyaramidsID { get; set; } = 2321;

		public static string MicrofiberShielding { get; }
		public static int MicrofiberShieldingID { get; set; } = 2327;

		public static string WaterCooledCPU { get; }
		public static int WaterCooledCPUID { get; set; } = 2328;

		public static string Biocells { get; }
		public static int BiocellsID { get; set; } = 2329;

		public static string Nanites { get; }
		public static int NanitesID { get; set; } = 2463;

		public static string MechanicalParts { get; }
		public static int MechanicalPartsID { get; set; } = 3689;

		public static string SyntheticOil { get; }
		public static int SyntheticOilID { get; set; } = 3691;

		public static string Fertilizer { get; }
		public static int FertilizerID { get; set; } = 3693;

		public static string Polytextiles { get; }
		public static int PolytextilesID { get; set; } = 3695;

		public static string SilicateGlass { get; }
		public static int SilicateGlassID { get; set; } = 3697;

		public static string Livestock { get; }
		public static int LivestockID { get; set; } = 3725;

		public static string ViralAgent { get; }
		public static int ViralAgentID { get; set; } = 3775;

		public static string ConstructionBlocks { get; }
		public static int ConstructionBlocksID { get; set; } = 3828;

		public static string RocketFuel { get; }
		public static int RocketFuelID { get; set; } = 9830;

		public static string Coolant { get; }
		public static int CoolantID { get; set; } = 9832;

		public static string ConsumerElectronics { get; }
		public static int ConsumerElectronicsID { get; set; } = 9836;

		public static string Superconductors { get; }
		public static int SuperconductorsID { get; set; } = 9838;

		public static string Transmitter { get; }
		public static int TransmitterID { get; set; } = 9840;

		public static string MiniatureElectronics { get; }
		public static int MiniatureElectronicsID { get; set; } = 9842;

		public static string GeneticallyEnhancedLivestock { get; }
		public static int GeneticallyEnhancedLivestockID { get; set; } = 15317;

		// tier1
		public static string Water { get; }
		public static int WaterID { get; set; } = 3645;

		public static string Plasmoids { get; }
		public static int PlasmoidsID { get; set; } = 2389;

		public static string Electrolytes { get; }
		public static int ElectrolytesID { get; set; } = 2390;

		public static string OxidizingCompound { get; }
		public static int OxidizingCompoundID { get; set; } = 2392;

		public static string Oxygen { get; }
		public static int OxygenID { get; set; } = 3683;

		public static string Bacteria { get; }
		public static int BacteriaID { get; set; } = 2393;

		public static string Proteins { get; }
		public static int ProteinsID { get; set; } = 2395;

		public static string Biofuels { get; }
		public static int BiofuelsID { get; set; } = 2396;

		public static string IndustrialFibers { get; }
		public static int IndustrialFibersID { get; set; } = 2397;

		public static string ReactiveMetals { get; }
		public static int ReactiveMetalsID { get; set; } = 2398;

		public static string PreciousMetals { get; }
		public static int PreciousMetalsID { get; set; } = 2399;

		public static string ToxicMetals { get; }
		public static int ToxicMetalsID { get; set; } = 2400;

		public static string ChiralStructures { get; }
		public static int ChiralStructuresID { get; set; } = 2401;

		public static string Biomass { get; }
		public static int BiomassID { get; set; } = 3779;

		public static string Silicon { get; }
		public static int SiliconID { get; set; } = 9828;

		// tier0
		public static string AqueousLiquids { get; }
		public static int AqueousLiquidsID { get; set; } = 2268;

		public static string Autotrophs { get; }
		public static int AutotrophsID { get; set; } = 2305;

		public static string BaseMetals { get; }
		public static int BaseMetalsID { get; set; } = 2267;

		public static string CarbonCompounds { get; }
		public static int CarbonCompoundsID { get; set; } = 2288;

		public static string ComplexOrganisms { get; }
		public static int ComplexOrganismsID { get; set; } = 2287;

		public static string FelsicMagma { get; }
		public static int FelsicMagmaID { get; set; } = 2307;

		public static string HeavyMetals { get; }
		public static int HeavyMetalsID { get; set; } = 2272;

		public static string IonicSolutions { get; }
		public static int IonicSolutionsID { get; set; } = 2309;

		public static string MicroOrganisms { get; }
		public static int MicroOrganismsID { get; set; } = 2073;

		public static string NobleGas { get; }
		public static int NobleGasID { get; set; } = 2310;

		public static string NobleMetals { get; }
		public static int NobleMetalsID { get; set; } = 2270;

		public static string NonCsCrystals { get; }
		public static int NonCsCrystalsID { get; set; } = 2306;

		public static string PlankticColonies { get; }
		public static int PlankticColoniesID { get; set; } = 2286;

		public static string ReactiveGas { get; }
		public static int ReactiveGasID { get; set; } = 2311;

		public static string SuspendedPlasma { get; }
		public static int SuspendedPlasmaID { get; set; } = 2308;
	}
}
