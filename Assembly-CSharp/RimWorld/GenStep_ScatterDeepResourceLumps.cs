using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_ScatterDeepResourceLumps : GenStep_Scatterer
	{
		private const float LumpSizeFactor = 1.6f;

		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				int num = base.CalculateFinalCount(map);
				int num2 = 0;
				while (num2 < num)
				{
					IntVec3 intVec = default(IntVec3);
					if (((GenStep_Scatterer)this).TryFindScatterCell(map, out intVec))
					{
						this.ScatterAt(intVec, map, 1);
						base.usedSpots.Add(intVec);
						num2++;
						continue;
					}
					return;
				}
				base.usedSpots.Clear();
			}
		}

		protected ThingDef ChooseThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((Func<ThingDef, float>)((ThingDef def) => def.deepCommonality));
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return (byte)((!base.NearUsedSpot(c, base.minSpacing)) ? 1 : 0) != 0;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			int numCells = Mathf.CeilToInt((float)((float)this.GetScatterLumpSizeRange(thingDef).RandomInRange * 1.6000000238418579));
			foreach (IntVec3 item in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				map.deepResourceGrid.SetAt(item, thingDef, thingDef.deepCountPerCell);
			}
		}

		private IntRange GetScatterLumpSizeRange(ThingDef def)
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			int num = 0;
			IntRange result;
			while (true)
			{
				if (num < allDefsListForReading.Count)
				{
					if (allDefsListForReading[num].building != null && allDefsListForReading[num].building.mineableThing == def)
					{
						result = allDefsListForReading[num].building.mineableScatterLumpSizeRange;
						break;
					}
					num++;
					continue;
				}
				result = new IntRange(2, 30);
				break;
			}
			return result;
		}
	}
}
