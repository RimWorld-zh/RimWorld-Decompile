using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		public ThingDef forcedDefToScatter;

		public int forcedLumpSize;

		[Unsaved]
		protected List<IntVec3> recentLumpCells = new List<IntVec3>();

		public override void Generate(Map map)
		{
			base.minSpacing = 5f;
			base.warnOnFail = false;
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

		protected ThingDef ChooseThingDef()
		{
			if (this.forcedDefToScatter != null)
			{
				return this.forcedDefToScatter;
			}
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((Func<ThingDef, float>)delegate(ThingDef d)
			{
				if (d.building == null)
				{
					return 0f;
				}
				return d.building.mineableScatterCommonality;
			});
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (base.NearUsedSpot(c, base.minSpacing))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.building.isNaturalRock)
			{
				return true;
			}
			return false;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			int numCells = (this.forcedLumpSize <= 0) ? thingDef.building.mineableScatterLumpSizeRange.RandomInRange : this.forcedLumpSize;
			this.recentLumpCells.Clear();
			foreach (IntVec3 item in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				GenSpawn.Spawn(thingDef, item, map);
				this.recentLumpCells.Add(item);
			}
		}
	}
}
