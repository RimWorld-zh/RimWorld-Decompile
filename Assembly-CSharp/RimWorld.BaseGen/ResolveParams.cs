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

		public Pawn singlePawnToSpawn;

		public PawnKindDef singlePawnKindDef;

		public bool? disableSinglePawn;

		public Lord singlePawnLord;

		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		public PawnGenerationRequest? singlePawnGenerationRequest;

		public int? mechanoidsCount;

		public int? hivesCount;

		public bool? disableHives;

		public Rot4? thingRot;

		public ThingDef wallStuff;

		public float? chanceToSkipWallBlock;

		public TerrainDef floorDef;

		public bool? clearEdificeOnly;

		public int? ancientCryptosleepCasketGroupID;

		public PodContentsType? podContentsType;

		public ItemCollectionGeneratorDef itemCollectionGeneratorDef;

		public ItemCollectionGeneratorParams? itemCollectionGeneratorParams;

		public IList<Thing> stockpileConcreteContents;

		public float? stockpileMarketValue;

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
			if (this.custom != null && this.custom.TryGetValue(name, out obj2))
			{
				obj = (T)obj2;
				return true;
			}
			obj = default(T);
			return false;
		}

		public T GetCustom<T>(string name)
		{
			object obj = default(object);
			if (this.custom != null && this.custom.TryGetValue(name, out obj))
			{
				return (T)obj;
			}
			return default(T);
		}

		public override string ToString()
		{
			object[] obj = new object[88]
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
			obj[26] = ", singlePawnToSpawn=";
			obj[27] = ((this.singlePawnToSpawn == null) ? "null" : this.singlePawnToSpawn.ToString());
			obj[28] = ", singlePawnKindDef=";
			obj[29] = ((this.singlePawnKindDef == null) ? "null" : this.singlePawnKindDef.ToString());
			obj[30] = ", disableSinglePawn=";
			bool? nullable5 = this.disableSinglePawn;
			obj[31] = ((!nullable5.HasValue) ? "null" : this.disableSinglePawn.ToString());
			obj[32] = ", singlePawnLord=";
			obj[33] = ((this.singlePawnLord == null) ? "null" : this.singlePawnLord.ToString());
			obj[34] = ", singlePawnSpawnCellExtraPredicate=";
			obj[35] = (((object)this.singlePawnSpawnCellExtraPredicate == null) ? "null" : this.singlePawnSpawnCellExtraPredicate.ToString());
			obj[36] = ", singlePawnGenerationRequest=";
			PawnGenerationRequest? nullable6 = this.singlePawnGenerationRequest;
			obj[37] = ((!nullable6.HasValue) ? "null" : this.singlePawnGenerationRequest.ToString());
			obj[38] = ", mechanoidsCount=";
			int? nullable7 = this.mechanoidsCount;
			obj[39] = ((!nullable7.HasValue) ? "null" : this.mechanoidsCount.ToString());
			obj[40] = ", hivesCount=";
			int? nullable8 = this.hivesCount;
			obj[41] = ((!nullable8.HasValue) ? "null" : this.hivesCount.ToString());
			obj[42] = ", disableHives=";
			bool? nullable9 = this.disableHives;
			obj[43] = ((!nullable9.HasValue) ? "null" : this.disableHives.ToString());
			obj[44] = ", thingRot=";
			Rot4? nullable10 = this.thingRot;
			obj[45] = ((!nullable10.HasValue) ? "null" : this.thingRot.ToString());
			obj[46] = ", wallStuff=";
			obj[47] = ((this.wallStuff == null) ? "null" : this.wallStuff.ToString());
			obj[48] = ", chanceToSkipWallBlock=";
			float? nullable11 = this.chanceToSkipWallBlock;
			obj[49] = ((!nullable11.HasValue) ? "null" : this.chanceToSkipWallBlock.ToString());
			obj[50] = ", floorDef=";
			obj[51] = ((this.floorDef == null) ? "null" : this.floorDef.ToString());
			obj[52] = ", clearEdificeOnly=";
			bool? nullable12 = this.clearEdificeOnly;
			obj[53] = ((!nullable12.HasValue) ? "null" : this.clearEdificeOnly.ToString());
			obj[54] = ", ancientCryptosleepCasketGroupID=";
			int? nullable13 = this.ancientCryptosleepCasketGroupID;
			obj[55] = ((!nullable13.HasValue) ? "null" : this.ancientCryptosleepCasketGroupID.ToString());
			obj[56] = ", podContentsType=";
			PodContentsType? nullable14 = this.podContentsType;
			obj[57] = ((!nullable14.HasValue) ? "null" : this.podContentsType.ToString());
			obj[58] = ", itemCollectionGeneratorDef=";
			obj[59] = ((this.itemCollectionGeneratorDef == null) ? "null" : this.itemCollectionGeneratorDef.ToString());
			obj[60] = ", itemCollectionGeneratorParams=";
			ItemCollectionGeneratorParams? nullable15 = this.itemCollectionGeneratorParams;
			obj[61] = ((!nullable15.HasValue) ? "null" : this.itemCollectionGeneratorParams.ToString());
			obj[62] = ", stockpileConcreteContents=";
			obj[63] = ((this.stockpileConcreteContents == null) ? "null" : this.stockpileConcreteContents.Count.ToString());
			obj[64] = ", stockpileMarketValue=";
			float? nullable16 = this.stockpileMarketValue;
			obj[65] = ((!nullable16.HasValue) ? "null" : this.stockpileMarketValue.ToString());
			obj[66] = ", edgeDefenseWidth=";
			int? nullable17 = this.edgeDefenseWidth;
			obj[67] = ((!nullable17.HasValue) ? "null" : this.edgeDefenseWidth.ToString());
			obj[68] = ", edgeDefenseTurretsCount=";
			int? nullable18 = this.edgeDefenseTurretsCount;
			obj[69] = ((!nullable18.HasValue) ? "null" : this.edgeDefenseTurretsCount.ToString());
			obj[70] = ", edgeDefenseMortarsCount=";
			int? nullable19 = this.edgeDefenseMortarsCount;
			obj[71] = ((!nullable19.HasValue) ? "null" : this.edgeDefenseMortarsCount.ToString());
			obj[72] = ", edgeDefenseGuardsCount=";
			int? nullable20 = this.edgeDefenseGuardsCount;
			obj[73] = ((!nullable20.HasValue) ? "null" : this.edgeDefenseGuardsCount.ToString());
			obj[74] = ", mortarDef=";
			obj[75] = ((this.mortarDef == null) ? "null" : this.mortarDef.ToString());
			obj[76] = ", pathwayFloorDef=";
			obj[77] = ((this.pathwayFloorDef == null) ? "null" : this.pathwayFloorDef.ToString());
			obj[78] = ", cultivatedPlantDef=";
			obj[79] = ((this.cultivatedPlantDef == null) ? "null" : this.cultivatedPlantDef.ToString());
			obj[80] = ", fillWithThingsPadding=";
			int? nullable21 = this.fillWithThingsPadding;
			obj[81] = ((!nullable21.HasValue) ? "null" : this.fillWithThingsPadding.ToString());
			obj[82] = ", factionBasePawnGroupPointsFactor=";
			float? nullable22 = this.factionBasePawnGroupPointsFactor;
			obj[83] = ((!nullable22.HasValue) ? "null" : this.factionBasePawnGroupPointsFactor.ToString());
			obj[84] = ", streetHorizontal=";
			bool? nullable23 = this.streetHorizontal;
			obj[85] = ((!nullable23.HasValue) ? "null" : this.streetHorizontal.ToString());
			obj[86] = ", edgeThingAvoidOtherEdgeThings=";
			bool? nullable24 = this.edgeThingAvoidOtherEdgeThings;
			obj[87] = ((!nullable24.HasValue) ? "null" : this.edgeThingAvoidOtherEdgeThings.ToString());
			return string.Concat(obj);
		}
	}
}
