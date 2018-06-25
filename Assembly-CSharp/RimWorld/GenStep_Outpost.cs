using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	public class GenStep_Outpost : GenStep
	{
		private const int Size = 16;

		private static List<CellRect> possibleRects = new List<CellRect>();

		public GenStep_Outpost()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static GenStep_Outpost()
		{
		}

		[CompilerGenerated]
		private sealed class <GetOutpostRect>c__AnonStorey0
		{
			internal CellRect mapRect;

			public <GetOutpostRect>c__AnonStorey0()
			{
			}

			internal bool <>m__0(CellRect x)
			{
				return !x.FullyContainedWithin(this.mapRect);
			}
		}
	}
}
