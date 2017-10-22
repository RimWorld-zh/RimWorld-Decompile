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

		public ItemCollectionGeneratorDef itemCollectionGeneratorDef;

		public ItemCollectionGeneratorParams? itemCollectionGeneratorParams;

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

		public float? factionBasePawnGroupPointsFactor;

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
				this.custom = new Dictionary<string, object>((IDictionary<string, object>)this.custom);
			}
			if (!this.custom.ContainsKey(name))
			{
				this.custom.Add(name, (object)obj);
			}
			else if (!inherit)
			{
				this.custom[name] = obj;
			}
		}

		public void RemoveCustom(string name)
		{
			if (this.custom != null)
			{
				this.custom = new Dictionary<string, object>(this.custom);
				this.custom.Remove(name);
			}
		}

		public bool TryGetCustom<T>(string name, out T obj)
		{
			object obj2 = default(object);
			bool result;
			if (this.custom == null || !this.custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				result = false;
			}
			else
			{
				obj = (T)obj2;
				result = true;
			}
			return result;
		}

		public T GetCustom<T>(string name)
		{
			object obj = default(object);
			return (this.custom != null && this.custom.TryGetValue(name, out obj)) ? ((T)obj) : default(T);
		}

		public override string ToString()
		{
			object[] obj = new object[110]
			{
				"rect=",
				this.rect,
				", faction=",
				(this.faction == null) ? "null" : this.faction.ToString(),
				", custom=",
				(this.custom == null) ? "null" : this.custom.Count.ToString(),
				", ancientTempleEntranceHeight=",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			int? nullable = this.ancientTempleEntranceHeight;
			obj[7] = ((!nullable.HasValue) ? "null" : this.ancientTempleEntranceHeight.ToString());
			obj[8] = ", pawnGroupMakerParams=";
			obj[9] = ((this.pawnGroupMakerParams == null) ? "null" : this.pawnGroupMakerParams.ToString());
			obj[10] = ", pawnGroupKindDef=";
			obj[11] = ((this.pawnGroupKindDef == null) ? "null" : this.pawnGroupKindDef.ToString());
			obj[12] = ", roofDef=";
			obj[13] = ((this.roofDef == null) ? "null" : this.roofDef.ToString());
			obj[14] = ", noRoof=";
			bool? nullable2 = this.noRoof;
			obj[15] = ((!nullable2.HasValue) ? "null" : this.noRoof.ToString());
			obj[16] = ", addRoomCenterToRootsToUnfog=";
			bool? nullable3 = this.addRoomCenterToRootsToUnfog;
			obj[17] = ((!nullable3.HasValue) ? "null" : this.addRoomCenterToRootsToUnfog.ToString());
			obj[18] = ", singleThingToSpawn=";
			obj[19] = ((this.singleThingToSpawn == null) ? "null" : this.singleThingToSpawn.ToString());
			obj[20] = ", singleThingDef=";
			obj[21] = ((this.singleThingDef == null) ? "null" : this.singleThingDef.ToString());
			obj[22] = ", singleThingStuff=";
			obj[23] = ((this.singleThingStuff == null) ? "null" : this.singleThingStuff.ToString());
			obj[24] = ", singleThingStackCount=";
			int? nullable4 = this.singleThingStackCount;
			obj[25] = ((!nullable4.HasValue) ? "null" : this.singleThingStackCount.ToString());
			obj[26] = ", skipSingleThingIfHasToWipeBuildingOrDoesntFit=";
			bool? nullable5 = this.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
			obj[27] = ((!nullable5.HasValue) ? "null" : this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString());
			obj[28] = ", singlePawnToSpawn=";
			obj[29] = ((this.singlePawnToSpawn == null) ? "null" : this.singlePawnToSpawn.ToString());
			obj[30] = ", singlePawnKindDef=";
			obj[31] = ((this.singlePawnKindDef == null) ? "null" : this.singlePawnKindDef.ToString());
			obj[32] = ", disableSinglePawn=";
			bool? nullable6 = this.disableSinglePawn;
			obj[33] = ((!nullable6.HasValue) ? "null" : this.disableSinglePawn.ToString());
			obj[34] = ", singlePawnLord=";
			obj[35] = ((this.singlePawnLord == null) ? "null" : this.singlePawnLord.ToString());
			obj[36] = ", singlePawnSpawnCellExtraPredicate=";
			obj[37] = (((object)this.singlePawnSpawnCellExtraPredicate == null) ? "null" : this.singlePawnSpawnCellExtraPredicate.ToString());
			obj[38] = ", singlePawnGenerationRequest=";
			PawnGenerationRequest? nullable7 = this.singlePawnGenerationRequest;
			obj[39] = ((!nullable7.HasValue) ? "null" : this.singlePawnGenerationRequest.ToString());
			obj[40] = ", postThingSpawn=";
			obj[41] = (((object)this.postThingSpawn == null) ? "null" : this.postThingSpawn.ToString());
			obj[42] = ", postThingGenerate=";
			obj[43] = (((object)this.postThingGenerate == null) ? "null" : this.postThingGenerate.ToString());
			obj[44] = ", mechanoidsCount=";
			int? nullable8 = this.mechanoidsCount;
			obj[45] = ((!nullable8.HasValue) ? "null" : this.mechanoidsCount.ToString());
			obj[46] = ", hivesCount=";
			int? nullable9 = this.hivesCount;
			obj[47] = ((!nullable9.HasValue) ? "null" : this.hivesCount.ToString());
			obj[48] = ", disableHives=";
			bool? nullable10 = this.disableHives;
			obj[49] = ((!nullable10.HasValue) ? "null" : this.disableHives.ToString());
			obj[50] = ", thingRot=";
			Rot4? nullable11 = this.thingRot;
			obj[51] = ((!nullable11.HasValue) ? "null" : this.thingRot.ToString());
			obj[52] = ", wallStuff=";
			obj[53] = ((this.wallStuff == null) ? "null" : this.wallStuff.ToString());
			obj[54] = ", chanceToSkipWallBlock=";
			float? nullable12 = this.chanceToSkipWallBlock;
			obj[55] = ((!nullable12.HasValue) ? "null" : this.chanceToSkipWallBlock.ToString());
			obj[56] = ", floorDef=";
			obj[57] = ((this.floorDef == null) ? "null" : this.floorDef.ToString());
			obj[58] = ", chanceToSkipFloor=";
			float? nullable13 = this.chanceToSkipFloor;
			obj[59] = ((!nullable13.HasValue) ? "null" : this.chanceToSkipFloor.ToString());
			obj[60] = ", filthDef=";
			obj[61] = ((this.filthDef == null) ? "null" : this.filthDef.ToString());
			obj[62] = ", filthDensity=";
			FloatRange? nullable14 = this.filthDensity;
			obj[63] = ((!nullable14.HasValue) ? "null" : this.filthDensity.ToString());
			obj[64] = ", clearEdificeOnly=";
			bool? nullable15 = this.clearEdificeOnly;
			obj[65] = ((!nullable15.HasValue) ? "null" : this.clearEdificeOnly.ToString());
			obj[66] = ", clearFillageOnly=";
			bool? nullable16 = this.clearFillageOnly;
			obj[67] = ((!nullable16.HasValue) ? "null" : this.clearFillageOnly.ToString());
			obj[68] = ", clearRoof=";
			bool? nullable17 = this.clearRoof;
			obj[69] = ((!nullable17.HasValue) ? "null" : this.clearRoof.ToString());
			obj[70] = ", ancientCryptosleepCasketGroupID=";
			int? nullable18 = this.ancientCryptosleepCasketGroupID;
			obj[71] = ((!nullable18.HasValue) ? "null" : this.ancientCryptosleepCasketGroupID.ToString());
			obj[72] = ", podContentsType=";
			PodContentsType? nullable19 = this.podContentsType;
			obj[73] = ((!nullable19.HasValue) ? "null" : this.podContentsType.ToString());
			obj[74] = ", itemCollectionGeneratorDef=";
			obj[75] = ((this.itemCollectionGeneratorDef == null) ? "null" : this.itemCollectionGeneratorDef.ToString());
			obj[76] = ", itemCollectionGeneratorParams=";
			ItemCollectionGeneratorParams? nullable20 = this.itemCollectionGeneratorParams;
			obj[77] = ((!nullable20.HasValue) ? "null" : this.itemCollectionGeneratorParams.ToString());
			obj[78] = ", stockpileConcreteContents=";
			obj[79] = ((this.stockpileConcreteContents == null) ? "null" : this.stockpileConcreteContents.Count.ToString());
			obj[80] = ", stockpileMarketValue=";
			float? nullable21 = this.stockpileMarketValue;
			obj[81] = ((!nullable21.HasValue) ? "null" : this.stockpileMarketValue.ToString());
			obj[82] = ", innerStockpileSize=";
			int? nullable22 = this.innerStockpileSize;
			obj[83] = ((!nullable22.HasValue) ? "null" : this.innerStockpileSize.ToString());
			obj[84] = ", edgeDefenseWidth=";
			int? nullable23 = this.edgeDefenseWidth;
			obj[85] = ((!nullable23.HasValue) ? "null" : this.edgeDefenseWidth.ToString());
			obj[86] = ", edgeDefenseTurretsCount=";
			int? nullable24 = this.edgeDefenseTurretsCount;
			obj[87] = ((!nullable24.HasValue) ? "null" : this.edgeDefenseTurretsCount.ToString());
			obj[88] = ", edgeDefenseMortarsCount=";
			int? nullable25 = this.edgeDefenseMortarsCount;
			obj[89] = ((!nullable25.HasValue) ? "null" : this.edgeDefenseMortarsCount.ToString());
			obj[90] = ", edgeDefenseGuardsCount=";
			int? nullable26 = this.edgeDefenseGuardsCount;
			obj[91] = ((!nullable26.HasValue) ? "null" : this.edgeDefenseGuardsCount.ToString());
			obj[92] = ", mortarDef=";
			obj[93] = ((this.mortarDef == null) ? "null" : this.mortarDef.ToString());
			obj[94] = ", pathwayFloorDef=";
			obj[95] = ((this.pathwayFloorDef == null) ? "null" : this.pathwayFloorDef.ToString());
			obj[96] = ", cultivatedPlantDef=";
			obj[97] = ((this.cultivatedPlantDef == null) ? "null" : this.cultivatedPlantDef.ToString());
			obj[98] = ", fillWithThingsPadding=";
			int? nullable27 = this.fillWithThingsPadding;
			obj[99] = ((!nullable27.HasValue) ? "null" : this.fillWithThingsPadding.ToString());
			obj[100] = ", factionBasePawnGroupPointsFactor=";
			float? nullable28 = this.factionBasePawnGroupPointsFactor;
			obj[101] = ((!nullable28.HasValue) ? "null" : this.factionBasePawnGroupPointsFactor.ToString());
			obj[102] = ", streetHorizontal=";
			bool? nullable29 = this.streetHorizontal;
			obj[103] = ((!nullable29.HasValue) ? "null" : this.streetHorizontal.ToString());
			obj[104] = ", edgeThingAvoidOtherEdgeThings=";
			bool? nullable30 = this.edgeThingAvoidOtherEdgeThings;
			obj[105] = ((!nullable30.HasValue) ? "null" : this.edgeThingAvoidOtherEdgeThings.ToString());
			obj[106] = ", allowPlacementOffEdge=";
			bool? nullable31 = this.allowPlacementOffEdge;
			obj[107] = ((!nullable31.HasValue) ? "null" : this.allowPlacementOffEdge.ToString());
			obj[108] = ", thrustAxis=";
			Rot4? nullable32 = this.thrustAxis;
			obj[109] = ((!nullable32.HasValue) ? "null" : this.thrustAxis.ToString());
			return string.Concat(obj);
		}
	}
}
