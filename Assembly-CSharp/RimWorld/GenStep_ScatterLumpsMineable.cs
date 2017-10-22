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
			return (this.forcedDefToScatter == null) ? DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((Func<ThingDef, float>)((ThingDef d) => (float)((d.building != null) ? d.building.mineableScatterCommonality : 0.0))) : this.forcedDefToScatter;
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (base.NearUsedSpot(c, base.minSpacing))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				result = ((byte)((edifice != null && edifice.def.building.isNaturalRock) ? 1 : 0) != 0);
			}
			return result;
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
