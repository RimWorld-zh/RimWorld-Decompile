using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040C RID: 1036
	public class GenStep_Outpost : GenStep
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x0009AB4C File Offset: 0x00098F4C
		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0009AB68 File Offset: 0x00098F68
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

		// Token: 0x060011D1 RID: 4561 RVA: 0x0009AC5C File Offset: 0x0009905C
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

		// Token: 0x04000AD6 RID: 2774
		private const int Size = 16;

		// Token: 0x04000AD7 RID: 2775
		private static List<CellRect> possibleRects = new List<CellRect>();
	}
}
