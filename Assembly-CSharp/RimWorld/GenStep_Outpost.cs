using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040E RID: 1038
	public class GenStep_Outpost : GenStep
	{
		// Token: 0x04000AD7 RID: 2775
		private const int Size = 16;

		// Token: 0x04000AD8 RID: 2776
		private static List<CellRect> possibleRects = new List<CellRect>();

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0009AE80 File Offset: 0x00099280
		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0009AE9C File Offset: 0x0009929C
		public override void Generate(Map map)
		{
			CellRect rectToDefend;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.SingleCell(map.Center);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = this.GetOutpostRect(rectToDefend, map);
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(2);
			resolveParams.edgeDefenseTurretsCount = new int?(Rand.RangeInclusive(0, 1));
			resolveParams.edgeDefenseMortarsCount = new int?(0);
			resolveParams.factionBasePawnGroupPointsFactor = new float?(0.4f);
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.symbolStack.Push("factionBase", resolveParams);
			BaseGen.Generate();
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0009AF90 File Offset: 0x00099390
		private CellRect GetOutpostRect(CellRect rectToDefend, Map map)
		{
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - 16, rectToDefend.CenterCell.z - 8, 16, 16));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - 8, 16, 16));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - 8, rectToDefend.minZ - 1 - 16, 16, 16));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - 8, rectToDefend.maxZ + 1, 16, 16));
			CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
			GenStep_Outpost.possibleRects.RemoveAll((CellRect x) => !x.FullyContainedWithin(mapRect));
			CellRect result;
			if (GenStep_Outpost.possibleRects.Any<CellRect>())
			{
				result = GenStep_Outpost.possibleRects.RandomElement<CellRect>();
			}
			else
			{
				result = rectToDefend;
			}
			return result;
		}
	}
}
