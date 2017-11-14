using RimWorld.BaseGen;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_Outpost : GenStep
	{
		private const int Size = 16;

		private static List<CellRect> possibleRects = new List<CellRect>();

		public override void Generate(Map map)
		{
			CellRect rectToDefend = default(CellRect);
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.SingleCell(map.Center);
			}
			Faction faction = (map.ParentFaction != null && map.ParentFaction != Faction.OfPlayer) ? map.ParentFaction : Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = this.GetOutpostRect(rectToDefend, map);
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = 2;
			resolveParams.edgeDefenseTurretsCount = Rand.RangeInclusive(0, 1);
			resolveParams.edgeDefenseMortarsCount = 0;
			resolveParams.factionBasePawnGroupPointsFactor = 0.4f;
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.globalSettings.minBuildings = 1;
			RimWorld.BaseGen.BaseGen.globalSettings.minBarracks = 1;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("factionBase", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
		}

		private CellRect GetOutpostRect(CellRect rectToDefend, Map map)
		{
			List<CellRect> list = GenStep_Outpost.possibleRects;
			int minX = rectToDefend.minX - 1 - 16;
			IntVec3 centerCell = rectToDefend.CenterCell;
			list.Add(new CellRect(minX, centerCell.z - 8, 16, 16));
			List<CellRect> list2 = GenStep_Outpost.possibleRects;
			int minX2 = rectToDefend.maxX + 1;
			IntVec3 centerCell2 = rectToDefend.CenterCell;
			list2.Add(new CellRect(minX2, centerCell2.z - 8, 16, 16));
			List<CellRect> list3 = GenStep_Outpost.possibleRects;
			IntVec3 centerCell3 = rectToDefend.CenterCell;
			list3.Add(new CellRect(centerCell3.x - 8, rectToDefend.minZ - 1 - 16, 16, 16));
			List<CellRect> list4 = GenStep_Outpost.possibleRects;
			IntVec3 centerCell4 = rectToDefend.CenterCell;
			list4.Add(new CellRect(centerCell4.x - 8, rectToDefend.maxZ + 1, 16, 16));
			IntVec3 size = map.Size;
			int x2 = size.x;
			IntVec3 size2 = map.Size;
			CellRect mapRect = new CellRect(0, 0, x2, size2.z);
			GenStep_Outpost.possibleRects.RemoveAll((CellRect x) => !x.FullyContainedWithin(mapRect));
			if (GenStep_Outpost.possibleRects.Any())
			{
				return GenStep_Outpost.possibleRects.RandomElement();
			}
			return rectToDefend;
		}
	}
}
