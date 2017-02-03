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

		public static List<PINode> CreatePINodes()
		{
			return new List<PINode>
			{
				new PINode()
				{
					Tier = PITier.Advanced,
					Name = nameof(PIConst.BroadcastNode),
					ID = PIConst.BroadcastNodeID,
					From = new List<int>() {PIConst.BiotechResearchReportsID, PIConst.CameraDronesID}
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
		public static string Plasmoids { get; }
		public static int PlasmoidsID { get; set; } = 2389;

		public static string Electrolytes { get; }
		public static int ElectrolytesID { get; set; } = 2390;

		public static string OxidizingCompound { get; }
		public static int OxidizingCompoundID { get; set; } = 2392;

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
	}
}
