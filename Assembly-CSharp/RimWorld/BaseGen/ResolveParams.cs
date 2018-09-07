using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public struct ResolveParams
	{
		public CellRect rect;

		public Faction faction;

		private Dictionary<string, object> custom;

		public int? ancientTempleEntranceHeight;

		public PawnGroupMakerParms pawnGroupMakerParams;

		public PawnGroupKindDef pawnGroupKindDef;

		public RoofDef roofDef;

		public bool? noRoof;

		public bool? addRoomCenterToRootsToUnfog;

		public Thing singleThingToSpawn;

		public ThingDef singleThingDef;

		public ThingDef singleThingStuff;

		public int? singleThingStackCount;

		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		public bool? spawnBridgeIfTerrainCantSupportThing;

		public Pawn singlePawnToSpawn;

		public PawnKindDef singlePawnKindDef;

		public bool? disableSinglePawn;

		public Lord singlePawnLord;

		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		public PawnGenerationRequest? singlePawnGenerationRequest;

		public Action<Thing> postThingSpawn;

		public Action<Thing> postThingGenerate;

		public int? mechanoidsCount;

		public int? hivesCount;

		public bool? disableHives;

		public Rot4? thingRot;

		public ThingDef wallStuff;

		public float? chanceToSkipWallBlock;

		public TerrainDef floorDef;

		public float? chanceToSkipFloor;

		public ThingDef filthDef;

		public FloatRange? filthDensity;

		public bool? clearEdificeOnly;

		public bool? clearFillageOnly;

		public bool? clearRoof;

		public int? ancientCryptosleepCasketGroupID;

		public PodContentsType? podContentsType;

		public ThingSetMakerDef thingSetMakerDef;

		public ThingSetMakerParams? thingSetMakerParams;

		public IList<Thing> stockpileConcreteContents;

		public float? stockpileMarketValue;

		public int? innerStockpileSize;

		public int? edgeDefenseWidth;

		public int? edgeDefenseTurretsCount;

		public int? edgeDefenseMortarsCount;

		public int? edgeDefenseGuardsCount;

		public ThingDef mortarDef;

		public TerrainDef pathwayFloorDef;

		public ThingDef cultivatedPlantDef;

		public int? fillWithThingsPadding;

		public float? settlementPawnGroupPoints;

		public int? settlementPawnGroupSeed;

		public bool? streetHorizontal;

		public bool? edgeThingAvoidOtherEdgeThings;

		public bool? allowPlacementOffEdge;

		public Rot4? thrustAxis;

		public FloatRange? hpPercentRange;

		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			if (this.custom == null)
			{
				this.custom = new Dictionary<string, object>();
			}
			else
			{
				this.custom = new Dictionary<string, object>(this.custom);
			}
			if (!this.custom.ContainsKey(name))
			{
				this.custom.Add(name, obj);
			}
			else if (!inherit)
			{
				this.custom[name] = obj;
			}
		}

		public void RemoveCustom(string name)
		{
			if (this.custom == null)
			{
				return;
			}
			this.custom = new Dictionary<string, object>(this.custom);
			this.custom.Remove(name);
		}

		public bool TryGetCustom<T>(string name, out T obj)
		{
			object obj2;
			if (this.custom == null || !this.custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				return false;
			}
			obj = (T)((object)obj2);
			return true;
		}

		public T GetCustom<T>(string name)
		{
			object obj;
			if (this.custom == null || !this.custom.TryGetValue(name, out obj))
			{
				return default(T);
			}
			return (T)((object)obj);
		}

		public override string ToString()
		{
			object[] array = new object[114];
			array[0] = "rect=";
			array[1] = this.rect;
			array[2] = ", faction=";
			array[3] = ((this.faction == null) ? "null" : this.faction.ToString());
			array[4] = ", custom=";
			array[5] = ((this.custom == null) ? "null" : this.custom.Count.ToString());
			array[6] = ", ancientTempleEntranceHeight=";
			int num = 7;
			int? num2 = this.ancientTempleEntranceHeight;
			array[num] = ((num2 == null) ? "null" : this.ancientTempleEntranceHeight.ToString());
			array[8] = ", pawnGroupMakerParams=";
			array[9] = ((this.pawnGroupMakerParams == null) ? "null" : this.pawnGroupMakerParams.ToString());
			array[10] = ", pawnGroupKindDef=";
			array[11] = ((this.pawnGroupKindDef == null) ? "null" : this.pawnGroupKindDef.ToString());
			array[12] = ", roofDef=";
			array[13] = ((this.roofDef == null) ? "null" : this.roofDef.ToString());
			array[14] = ", noRoof=";
			int num3 = 15;
			bool? flag = this.noRoof;
			array[num3] = ((flag == null) ? "null" : this.noRoof.ToString());
			array[16] = ", addRoomCenterToRootsToUnfog=";
			int num4 = 17;
			bool? flag2 = this.addRoomCenterToRootsToUnfog;
			array[num4] = ((flag2 == null) ? "null" : this.addRoomCenterToRootsToUnfog.ToString());
			array[18] = ", singleThingToSpawn=";
			array[19] = ((this.singleThingToSpawn == null) ? "null" : this.singleThingToSpawn.ToString());
			array[20] = ", singleThingDef=";
			array[21] = ((this.singleThingDef == null) ? "null" : this.singleThingDef.ToString());
			array[22] = ", singleThingStuff=";
			array[23] = ((this.singleThingStuff == null) ? "null" : this.singleThingStuff.ToString());
			array[24] = ", singleThingStackCount=";
			int num5 = 25;
			int? num6 = this.singleThingStackCount;
			array[num5] = ((num6 == null) ? "null" : this.singleThingStackCount.ToString());
			array[26] = ", skipSingleThingIfHasToWipeBuildingOrDoesntFit=";
			int num7 = 27;
			bool? flag3 = this.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
			array[num7] = ((flag3 == null) ? "null" : this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString());
			array[28] = ", spawnBridgeIfTerrainCantSupportThing=";
			int num8 = 29;
			bool? flag4 = this.spawnBridgeIfTerrainCantSupportThing;
			array[num8] = ((flag4 == null) ? "null" : this.spawnBridgeIfTerrainCantSupportThing.ToString());
			array[30] = ", singlePawnToSpawn=";
			array[31] = ((this.singlePawnToSpawn == null) ? "null" : this.singlePawnToSpawn.ToString());
			array[32] = ", singlePawnKindDef=";
			array[33] = ((this.singlePawnKindDef == null) ? "null" : this.singlePawnKindDef.ToString());
			array[34] = ", disableSinglePawn=";
			int num9 = 35;
			bool? flag5 = this.disableSinglePawn;
			array[num9] = ((flag5 == null) ? "null" : this.disableSinglePawn.ToString());
			array[36] = ", singlePawnLord=";
			array[37] = ((this.singlePawnLord == null) ? "null" : this.singlePawnLord.ToString());
			array[38] = ", singlePawnSpawnCellExtraPredicate=";
			array[39] = ((this.singlePawnSpawnCellExtraPredicate == null) ? "null" : this.singlePawnSpawnCellExtraPredicate.ToString());
			array[40] = ", singlePawnGenerationRequest=";
			int num10 = 41;
			PawnGenerationRequest? pawnGenerationRequest = this.singlePawnGenerationRequest;
			array[num10] = ((pawnGenerationRequest == null) ? "null" : this.singlePawnGenerationRequest.ToString());
			array[42] = ", postThingSpawn=";
			array[43] = ((this.postThingSpawn == null) ? "null" : this.postThingSpawn.ToString());
			array[44] = ", postThingGenerate=";
			array[45] = ((this.postThingGenerate == null) ? "null" : this.postThingGenerate.ToString());
			array[46] = ", mechanoidsCount=";
			int num11 = 47;
			int? num12 = this.mechanoidsCount;
			array[num11] = ((num12 == null) ? "null" : this.mechanoidsCount.ToString());
			array[48] = ", hivesCount=";
			int num13 = 49;
			int? num14 = this.hivesCount;
			array[num13] = ((num14 == null) ? "null" : this.hivesCount.ToString());
			array[50] = ", disableHives=";
			int num15 = 51;
			bool? flag6 = this.disableHives;
			array[num15] = ((flag6 == null) ? "null" : this.disableHives.ToString());
			array[52] = ", thingRot=";
			int num16 = 53;
			Rot4? rot = this.thingRot;
			array[num16] = ((rot == null) ? "null" : this.thingRot.ToString());
			array[54] = ", wallStuff=";
			array[55] = ((this.wallStuff == null) ? "null" : this.wallStuff.ToString());
			array[56] = ", chanceToSkipWallBlock=";
			int num17 = 57;
			float? num18 = this.chanceToSkipWallBlock;
			array[num17] = ((num18 == null) ? "null" : this.chanceToSkipWallBlock.ToString());
			array[58] = ", floorDef=";
			array[59] = ((this.floorDef == null) ? "null" : this.floorDef.ToString());
			array[60] = ", chanceToSkipFloor=";
			int num19 = 61;
			float? num20 = this.chanceToSkipFloor;
			array[num19] = ((num20 == null) ? "null" : this.chanceToSkipFloor.ToString());
			array[62] = ", filthDef=";
			array[63] = ((this.filthDef == null) ? "null" : this.filthDef.ToString());
			array[64] = ", filthDensity=";
			int num21 = 65;
			FloatRange? floatRange = this.filthDensity;
			array[num21] = ((floatRange == null) ? "null" : this.filthDensity.ToString());
			array[66] = ", clearEdificeOnly=";
			int num22 = 67;
			bool? flag7 = this.clearEdificeOnly;
			array[num22] = ((flag7 == null) ? "null" : this.clearEdificeOnly.ToString());
			array[68] = ", clearFillageOnly=";
			int num23 = 69;
			bool? flag8 = this.clearFillageOnly;
			array[num23] = ((flag8 == null) ? "null" : this.clearFillageOnly.ToString());
			array[70] = ", clearRoof=";
			int num24 = 71;
			bool? flag9 = this.clearRoof;
			array[num24] = ((flag9 == null) ? "null" : this.clearRoof.ToString());
			array[72] = ", ancientCryptosleepCasketGroupID=";
			int num25 = 73;
			int? num26 = this.ancientCryptosleepCasketGroupID;
			array[num25] = ((num26 == null) ? "null" : this.ancientCryptosleepCasketGroupID.ToString());
			array[74] = ", podContentsType=";
			int num27 = 75;
			PodContentsType? podContentsType = this.podContentsType;
			array[num27] = ((podContentsType == null) ? "null" : this.podContentsType.ToString());
			array[76] = ", thingSetMakerDef=";
			array[77] = ((this.thingSetMakerDef == null) ? "null" : this.thingSetMakerDef.ToString());
			array[78] = ", thingSetMakerParams=";
			int num28 = 79;
			ThingSetMakerParams? thingSetMakerParams = this.thingSetMakerParams;
			array[num28] = ((thingSetMakerParams == null) ? "null" : this.thingSetMakerParams.ToString());
			array[80] = ", stockpileConcreteContents=";
			array[81] = ((this.stockpileConcreteContents == null) ? "null" : this.stockpileConcreteContents.Count.ToString());
			array[82] = ", stockpileMarketValue=";
			int num29 = 83;
			float? num30 = this.stockpileMarketValue;
			array[num29] = ((num30 == null) ? "null" : this.stockpileMarketValue.ToString());
			array[84] = ", innerStockpileSize=";
			int num31 = 85;
			int? num32 = this.innerStockpileSize;
			array[num31] = ((num32 == null) ? "null" : this.innerStockpileSize.ToString());
			array[86] = ", edgeDefenseWidth=";
			int num33 = 87;
			int? num34 = this.edgeDefenseWidth;
			array[num33] = ((num34 == null) ? "null" : this.edgeDefenseWidth.ToString());
			array[88] = ", edgeDefenseTurretsCount=";
			int num35 = 89;
			int? num36 = this.edgeDefenseTurretsCount;
			array[num35] = ((num36 == null) ? "null" : this.edgeDefenseTurretsCount.ToString());
			array[90] = ", edgeDefenseMortarsCount=";
			int num37 = 91;
			int? num38 = this.edgeDefenseMortarsCount;
			array[num37] = ((num38 == null) ? "null" : this.edgeDefenseMortarsCount.ToString());
			array[92] = ", edgeDefenseGuardsCount=";
			int num39 = 93;
			int? num40 = this.edgeDefenseGuardsCount;
			array[num39] = ((num40 == null) ? "null" : this.edgeDefenseGuardsCount.ToString());
			array[94] = ", mortarDef=";
			array[95] = ((this.mortarDef == null) ? "null" : this.mortarDef.ToString());
			array[96] = ", pathwayFloorDef=";
			array[97] = ((this.pathwayFloorDef == null) ? "null" : this.pathwayFloorDef.ToString());
			array[98] = ", cultivatedPlantDef=";
			array[99] = ((this.cultivatedPlantDef == null) ? "null" : this.cultivatedPlantDef.ToString());
			array[100] = ", fillWithThingsPadding=";
			int num41 = 101;
			int? num42 = this.fillWithThingsPadding;
			array[num41] = ((num42 == null) ? "null" : this.fillWithThingsPadding.ToString());
			array[102] = ", settlementPawnGroupPoints=";
			int num43 = 103;
			float? num44 = this.settlementPawnGroupPoints;
			array[num43] = ((num44 == null) ? "null" : this.settlementPawnGroupPoints.ToString());
			array[104] = ", settlementPawnGroupSeed=";
			int num45 = 105;
			int? num46 = this.settlementPawnGroupSeed;
			array[num45] = ((num46 == null) ? "null" : this.settlementPawnGroupSeed.ToString());
			array[106] = ", streetHorizontal=";
			int num47 = 107;
			bool? flag10 = this.streetHorizontal;
			array[num47] = ((flag10 == null) ? "null" : this.streetHorizontal.ToString());
			array[108] = ", edgeThingAvoidOtherEdgeThings=";
			int num48 = 109;
			bool? flag11 = this.edgeThingAvoidOtherEdgeThings;
			array[num48] = ((flag11 == null) ? "null" : this.edgeThingAvoidOtherEdgeThings.ToString());
			array[110] = ", allowPlacementOffEdge=";
			int num49 = 111;
			bool? flag12 = this.allowPlacementOffEdge;
			array[num49] = ((flag12 == null) ? "null" : this.allowPlacementOffEdge.ToString());
			array[112] = ", thrustAxis=";
			int num50 = 113;
			Rot4? rot2 = this.thrustAxis;
			array[num50] = ((rot2 == null) ? "null" : this.thrustAxis.ToString());
			return string.Concat(array);
		}
	}
}
