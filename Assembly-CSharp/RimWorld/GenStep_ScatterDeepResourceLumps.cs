using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_ScatterDeepResourceLumps : GenStep_Scatterer
	{
		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

		public GenStep_ScatterDeepResourceLumps()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1712041303;
			}
		}

		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				int num = base.CalculateFinalCount(map);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec;
					if (!this.TryFindScatterCell(map, out intVec))
					{
						return;
					}
					this.ScatterAt(intVec, map, 1);
					this.usedSpots.Add(intVec);
				}
				this.usedSpots.Clear();
			}
		}

		protected ThingDef ChooseThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return !base.NearUsedSpot(c, this.minSpacing);
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				if (!c2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(c2, thingDef, thingDef.deepCountPerCell);
				}
			}
		}

		[CompilerGenerated]
		private static float <ChooseThingDef>m__0(ThingDef def)
		{
			return def.deepCommonality;
		}
	}
}
