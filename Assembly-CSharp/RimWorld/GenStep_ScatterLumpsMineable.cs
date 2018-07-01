using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		public ThingDef forcedDefToScatter;

		public int forcedLumpSize;

		public float maxValue = float.MaxValue;

		[Unsaved]
		protected List<IntVec3> recentLumpCells = new List<IntVec3>();

		public GenStep_ScatterLumpsMineable()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 920906419;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
		{
			this.minSpacing = 5f;
			this.warnOnFail = false;
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

		protected ThingDef ChooseThingDef()
		{
			ThingDef result;
			if (this.forcedDefToScatter != null)
			{
				result = this.forcedDefToScatter;
			}
			else
			{
				result = DefDatabase<ThingDef>.AllDefs.RandomElementByWeightWithFallback(delegate(ThingDef d)
				{
					float result2;
					if (d.building == null)
					{
						result2 = 0f;
					}
					else if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
					{
						result2 = 0f;
					}
					else
					{
						result2 = d.building.mineableScatterCommonality;
					}
					return result2;
				}, null);
			}
			return result;
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (base.NearUsedSpot(c, this.minSpacing))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				result = (edifice != null && edifice.def.building.isNaturalRock);
			}
			return result;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			if (thingDef != null)
			{
				int numCells = (this.forcedLumpSize <= 0) ? thingDef.building.mineableScatterLumpSizeRange.RandomInRange : this.forcedLumpSize;
				this.recentLumpCells.Clear();
				foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(c, map, numCells))
				{
					GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
					this.recentLumpCells.Add(intVec);
				}
			}
		}

		[CompilerGenerated]
		private float <ChooseThingDef>m__0(ThingDef d)
		{
			float result;
			if (d.building == null)
			{
				result = 0f;
			}
			else if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
			{
				result = 0f;
			}
			else
			{
				result = d.building.mineableScatterCommonality;
			}
			return result;
		}
	}
}
