using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02000391 RID: 913
	public struct ResolveParams
	{
		// Token: 0x040009BB RID: 2491
		public CellRect rect;

		// Token: 0x040009BC RID: 2492
		public Faction faction;

		// Token: 0x040009BD RID: 2493
		private Dictionary<string, object> custom;

		// Token: 0x040009BE RID: 2494
		public int? ancientTempleEntranceHeight;

		// Token: 0x040009BF RID: 2495
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x040009C0 RID: 2496
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x040009C1 RID: 2497
		public RoofDef roofDef;

		// Token: 0x040009C2 RID: 2498
		public bool? noRoof;

		// Token: 0x040009C3 RID: 2499
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x040009C4 RID: 2500
		public Thing singleThingToSpawn;

		// Token: 0x040009C5 RID: 2501
		public ThingDef singleThingDef;

		// Token: 0x040009C6 RID: 2502
		public ThingDef singleThingStuff;

		// Token: 0x040009C7 RID: 2503
		public int? singleThingStackCount;

		// Token: 0x040009C8 RID: 2504
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x040009C9 RID: 2505
		public Pawn singlePawnToSpawn;

		// Token: 0x040009CA RID: 2506
		public PawnKindDef singlePawnKindDef;

		// Token: 0x040009CB RID: 2507
		public bool? disableSinglePawn;

		// Token: 0x040009CC RID: 2508
		public Lord singlePawnLord;

		// Token: 0x040009CD RID: 2509
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x040009CE RID: 2510
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x040009CF RID: 2511
		public Action<Thing> postThingSpawn;

		// Token: 0x040009D0 RID: 2512
		public Action<Thing> postThingGenerate;

		// Token: 0x040009D1 RID: 2513
		public int? mechanoidsCount;

		// Token: 0x040009D2 RID: 2514
		public int? hivesCount;

		// Token: 0x040009D3 RID: 2515
		public bool? disableHives;

		// Token: 0x040009D4 RID: 2516
		public Rot4? thingRot;

		// Token: 0x040009D5 RID: 2517
		public ThingDef wallStuff;

		// Token: 0x040009D6 RID: 2518
		public float? chanceToSkipWallBlock;

		// Token: 0x040009D7 RID: 2519
		public TerrainDef floorDef;

		// Token: 0x040009D8 RID: 2520
		public float? chanceToSkipFloor;

		// Token: 0x040009D9 RID: 2521
		public ThingDef filthDef;

		// Token: 0x040009DA RID: 2522
		public FloatRange? filthDensity;

		// Token: 0x040009DB RID: 2523
		public bool? clearEdificeOnly;

		// Token: 0x040009DC RID: 2524
		public bool? clearFillageOnly;

		// Token: 0x040009DD RID: 2525
		public bool? clearRoof;

		// Token: 0x040009DE RID: 2526
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x040009DF RID: 2527
		public PodContentsType? podContentsType;

		// Token: 0x040009E0 RID: 2528
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x040009E1 RID: 2529
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x040009E2 RID: 2530
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x040009E3 RID: 2531
		public float? stockpileMarketValue;

		// Token: 0x040009E4 RID: 2532
		public int? innerStockpileSize;

		// Token: 0x040009E5 RID: 2533
		public int? edgeDefenseWidth;

		// Token: 0x040009E6 RID: 2534
		public int? edgeDefenseTurretsCount;

		// Token: 0x040009E7 RID: 2535
		public int? edgeDefenseMortarsCount;

		// Token: 0x040009E8 RID: 2536
		public int? edgeDefenseGuardsCount;

		// Token: 0x040009E9 RID: 2537
		public ThingDef mortarDef;

		// Token: 0x040009EA RID: 2538
		public TerrainDef pathwayFloorDef;

		// Token: 0x040009EB RID: 2539
		public ThingDef cultivatedPlantDef;

		// Token: 0x040009EC RID: 2540
		public int? fillWithThingsPadding;

		// Token: 0x040009ED RID: 2541
		public float? factionBasePawnGroupPointsFactor;

		// Token: 0x040009EE RID: 2542
		public bool? streetHorizontal;

		// Token: 0x040009EF RID: 2543
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x040009F0 RID: 2544
		public bool? allowPlacementOffEdge;

		// Token: 0x040009F1 RID: 2545
		public Rot4? thrustAxis;

		// Token: 0x040009F2 RID: 2546
		public FloatRange? hpPercentRange;

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00085D48 File Offset: 0x00084148
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

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00085DC2 File Offset: 0x000841C2
		public void RemoveCustom(string name)
		{
			if (this.custom != null)
			{
				this.custom = new Dictionary<string, object>(this.custom);
				this.custom.Remove(name);
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00085DF4 File Offset: 0x000841F4
		public bool TryGetCustom<T>(string name, out T obj)
		{
			object obj2;
			bool result;
			if (this.custom == null || !this.custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				result = false;
			}
			else
			{
				obj = (T)((object)obj2);
				result = true;
			}
			return result;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00085E4C File Offset: 0x0008424C
		public T GetCustom<T>(string name)
		{
			object obj;
			T result;
			if (this.custom == null || !this.custom.TryGetValue(name, out obj))
			{
				result = default(T);
			}
			else
			{
				result = (T)((object)obj);
			}
			return result;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00085E94 File Offset: 0x00084294
		public override string ToString()
		{
			object[] array = new object[110];
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
			array[28] = ", singlePawnToSpawn=";
			array[29] = ((this.singlePawnToSpawn == null) ? "null" : this.singlePawnToSpawn.ToString());
			array[30] = ", singlePawnKindDef=";
			array[31] = ((this.singlePawnKindDef == null) ? "null" : this.singlePawnKindDef.ToString());
			array[32] = ", disableSinglePawn=";
			int num8 = 33;
			bool? flag4 = this.disableSinglePawn;
			array[num8] = ((flag4 == null) ? "null" : this.disableSinglePawn.ToString());
			array[34] = ", singlePawnLord=";
			array[35] = ((this.singlePawnLord == null) ? "null" : this.singlePawnLord.ToString());
			array[36] = ", singlePawnSpawnCellExtraPredicate=";
			array[37] = ((this.singlePawnSpawnCellExtraPredicate == null) ? "null" : this.singlePawnSpawnCellExtraPredicate.ToString());
			array[38] = ", singlePawnGenerationRequest=";
			int num9 = 39;
			PawnGenerationRequest? pawnGenerationRequest = this.singlePawnGenerationRequest;
			array[num9] = ((pawnGenerationRequest == null) ? "null" : this.singlePawnGenerationRequest.ToString());
			array[40] = ", postThingSpawn=";
			array[41] = ((this.postThingSpawn == null) ? "null" : this.postThingSpawn.ToString());
			array[42] = ", postThingGenerate=";
			array[43] = ((this.postThingGenerate == null) ? "null" : this.postThingGenerate.ToString());
			array[44] = ", mechanoidsCount=";
			int num10 = 45;
			int? num11 = this.mechanoidsCount;
			array[num10] = ((num11 == null) ? "null" : this.mechanoidsCount.ToString());
			array[46] = ", hivesCount=";
			int num12 = 47;
			int? num13 = this.hivesCount;
			array[num12] = ((num13 == null) ? "null" : this.hivesCount.ToString());
			array[48] = ", disableHives=";
			int num14 = 49;
			bool? flag5 = this.disableHives;
			array[num14] = ((flag5 == null) ? "null" : this.disableHives.ToString());
			array[50] = ", thingRot=";
			int num15 = 51;
			Rot4? rot = this.thingRot;
			array[num15] = ((rot == null) ? "null" : this.thingRot.ToString());
			array[52] = ", wallStuff=";
			array[53] = ((this.wallStuff == null) ? "null" : this.wallStuff.ToString());
			array[54] = ", chanceToSkipWallBlock=";
			int num16 = 55;
			float? num17 = this.chanceToSkipWallBlock;
			array[num16] = ((num17 == null) ? "null" : this.chanceToSkipWallBlock.ToString());
			array[56] = ", floorDef=";
			array[57] = ((this.floorDef == null) ? "null" : this.floorDef.ToString());
			array[58] = ", chanceToSkipFloor=";
			int num18 = 59;
			float? num19 = this.chanceToSkipFloor;
			array[num18] = ((num19 == null) ? "null" : this.chanceToSkipFloor.ToString());
			array[60] = ", filthDef=";
			array[61] = ((this.filthDef == null) ? "null" : this.filthDef.ToString());
			array[62] = ", filthDensity=";
			int num20 = 63;
			FloatRange? floatRange = this.filthDensity;
			array[num20] = ((floatRange == null) ? "null" : this.filthDensity.ToString());
			array[64] = ", clearEdificeOnly=";
			int num21 = 65;
			bool? flag6 = this.clearEdificeOnly;
			array[num21] = ((flag6 == null) ? "null" : this.clearEdificeOnly.ToString());
			array[66] = ", clearFillageOnly=";
			int num22 = 67;
			bool? flag7 = this.clearFillageOnly;
			array[num22] = ((flag7 == null) ? "null" : this.clearFillageOnly.ToString());
			array[68] = ", clearRoof=";
			int num23 = 69;
			bool? flag8 = this.clearRoof;
			array[num23] = ((flag8 == null) ? "null" : this.clearRoof.ToString());
			array[70] = ", ancientCryptosleepCasketGroupID=";
			int num24 = 71;
			int? num25 = this.ancientCryptosleepCasketGroupID;
			array[num24] = ((num25 == null) ? "null" : this.ancientCryptosleepCasketGroupID.ToString());
			array[72] = ", podContentsType=";
			int num26 = 73;
			PodContentsType? podContentsType = this.podContentsType;
			array[num26] = ((podContentsType == null) ? "null" : this.podContentsType.ToString());
			array[74] = ", thingSetMakerDef=";
			array[75] = ((this.thingSetMakerDef == null) ? "null" : this.thingSetMakerDef.ToString());
			array[76] = ", thingSetMakerParams=";
			int num27 = 77;
			ThingSetMakerParams? thingSetMakerParams = this.thingSetMakerParams;
			array[num27] = ((thingSetMakerParams == null) ? "null" : this.thingSetMakerParams.ToString());
			array[78] = ", stockpileConcreteContents=";
			array[79] = ((this.stockpileConcreteContents == null) ? "null" : this.stockpileConcreteContents.Count.ToString());
			array[80] = ", stockpileMarketValue=";
			int num28 = 81;
			float? num29 = this.stockpileMarketValue;
			array[num28] = ((num29 == null) ? "null" : this.stockpileMarketValue.ToString());
			array[82] = ", innerStockpileSize=";
			int num30 = 83;
			int? num31 = this.innerStockpileSize;
			array[num30] = ((num31 == null) ? "null" : this.innerStockpileSize.ToString());
			array[84] = ", edgeDefenseWidth=";
			int num32 = 85;
			int? num33 = this.edgeDefenseWidth;
			array[num32] = ((num33 == null) ? "null" : this.edgeDefenseWidth.ToString());
			array[86] = ", edgeDefenseTurretsCount=";
			int num34 = 87;
			int? num35 = this.edgeDefenseTurretsCount;
			array[num34] = ((num35 == null) ? "null" : this.edgeDefenseTurretsCount.ToString());
			array[88] = ", edgeDefenseMortarsCount=";
			int num36 = 89;
			int? num37 = this.edgeDefenseMortarsCount;
			array[num36] = ((num37 == null) ? "null" : this.edgeDefenseMortarsCount.ToString());
			array[90] = ", edgeDefenseGuardsCount=";
			int num38 = 91;
			int? num39 = this.edgeDefenseGuardsCount;
			array[num38] = ((num39 == null) ? "null" : this.edgeDefenseGuardsCount.ToString());
			array[92] = ", mortarDef=";
			array[93] = ((this.mortarDef == null) ? "null" : this.mortarDef.ToString());
			array[94] = ", pathwayFloorDef=";
			array[95] = ((this.pathwayFloorDef == null) ? "null" : this.pathwayFloorDef.ToString());
			array[96] = ", cultivatedPlantDef=";
			array[97] = ((this.cultivatedPlantDef == null) ? "null" : this.cultivatedPlantDef.ToString());
			array[98] = ", fillWithThingsPadding=";
			int num40 = 99;
			int? num41 = this.fillWithThingsPadding;
			array[num40] = ((num41 == null) ? "null" : this.fillWithThingsPadding.ToString());
			array[100] = ", factionBasePawnGroupPointsFactor=";
			int num42 = 101;
			float? num43 = this.factionBasePawnGroupPointsFactor;
			array[num42] = ((num43 == null) ? "null" : this.factionBasePawnGroupPointsFactor.ToString());
			array[102] = ", streetHorizontal=";
			int num44 = 103;
			bool? flag9 = this.streetHorizontal;
			array[num44] = ((flag9 == null) ? "null" : this.streetHorizontal.ToString());
			array[104] = ", edgeThingAvoidOtherEdgeThings=";
			int num45 = 105;
			bool? flag10 = this.edgeThingAvoidOtherEdgeThings;
			array[num45] = ((flag10 == null) ? "null" : this.edgeThingAvoidOtherEdgeThings.ToString());
			array[106] = ", allowPlacementOffEdge=";
			int num46 = 107;
			bool? flag11 = this.allowPlacementOffEdge;
			array[num46] = ((flag11 == null) ? "null" : this.allowPlacementOffEdge.ToString());
			array[108] = ", thrustAxis=";
			int num47 = 109;
			Rot4? rot2 = this.thrustAxis;
			array[num47] = ((rot2 == null) ? "null" : this.thrustAxis.ToString());
			return string.Concat(array);
		}
	}
}
