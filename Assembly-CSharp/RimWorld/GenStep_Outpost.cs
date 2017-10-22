using RimWorld.BaseGen;
using System;
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
			ResolveParams resolveParams = new ResolveParams
			{
				rect = this.GetOutpostRect(rectToDefend, map),
				faction = faction,
				edgeDefenseWidth = new int?(2),
				edgeDefenseTurretsCount = new int?(Rand.RangeInclusive(0, 1)),
				edgeDefenseMortarsCount = new int?(0),
				factionBasePawnGroupPointsFactor = new float?(0.4f)
			};
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.globalSettings.minBuildings = 1;
			RimWorld.BaseGen.BaseGen.globalSettings.minBarracks = 1;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("factionBase", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
		}

		private CellRect GetOutpostRect(CellRect rectToDefend, Map map)
		{
			List<CellRect> obj = GenStep_Outpost.possibleRects;
			int minX = rectToDefend.minX - 1 - 16;
			IntVec3 centerCell = rectToDefend.CenterCell;
			obj.Add(new CellRect(minX, centerCell.z - 8, 16, 16));
			List<CellRect> obj2 = GenStep_Outpost.possibleRects;
			int minX2 = rectToDefend.maxX + 1;
			IntVec3 centerCell2 = rectToDefend.CenterCell;
			obj2.Add(new CellRect(minX2, centerCell2.z - 8, 16, 16));
			List<CellRect> obj3 = GenStep_Outpost.possibleRects;
			IntVec3 centerCell3 = rectToDefend.CenterCell;
			obj3.Add(new CellRect(centerCell3.x - 8, rectToDefend.minZ - 1 - 16, 16, 16));
			List<CellRect> obj4 = GenStep_Outpost.possibleRects;
			IntVec3 centerCell4 = rectToDefend.CenterCell;
			obj4.Add(new CellRect(centerCell4.x - 8, rectToDefend.maxZ + 1, 16, 16));
			IntVec3 size = map.Size;
			int x2 = size.x;
			IntVec3 size2 = map.Size;
			CellRect mapRect = new CellRect(0, 0, x2, size2.z);
			GenStep_Outpost.possibleRects.RemoveAll((Predicate<CellRect>)((CellRect x) => !x.FullyContainedWithin(mapRect)));
			return (!GenStep_Outpost.possibleRects.Any()) ? rectToDefend : GenStep_Outpost.possibleRects.RandomElement();
		}
	}
}
