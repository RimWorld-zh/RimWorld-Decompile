using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompDeepDrill : ThingComp
	{
		private CompPowerTrader powerComp;

		private float lumpProgress = 0f;

		private float lumpYieldPct = 0f;

		private int lastUsedTick = -99999;

		private const float ResourceLumpWork = 16000f;

		public CompDeepDrill()
		{
		}

		public float ProgressToNextLumpPercent
		{
			get
			{
				return this.lumpProgress / 16000f;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.lumpProgress, "lumpProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.lumpYieldPct, "lumpYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		public void DrillWorkDone(Pawn driller)
		{
			float statValue = driller.GetStatValue(StatDefOf.MiningSpeed, true);
			this.lumpProgress += statValue;
			this.lumpYieldPct += statValue * driller.GetStatValue(StatDefOf.MiningYield, true) / 16000f;
			this.lastUsedTick = Find.TickManager.TicksGame;
			if (this.lumpProgress > 16000f)
			{
				this.TryProduceLump(this.lumpYieldPct);
				this.lumpProgress = 0f;
				this.lumpYieldPct = 0f;
			}
		}

		private void TryProduceLump(float yieldPct)
		{
			ThingDef thingDef;
			int num;
			IntVec3 c;
			bool nextResource = this.GetNextResource(out thingDef, out num, out c);
			if (thingDef != null)
			{
				int num2 = Mathf.Min(num, Mathf.CeilToInt((float)thingDef.stackLimit / 2f));
				if (nextResource)
				{
					this.parent.Map.deepResourceGrid.SetAt(c, thingDef, num - num2);
				}
				int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, null);
				if (nextResource && !this.ValuableResourcesPresent())
				{
					if (DeepDrillUtility.GetBaseResource(this.parent.Map) == null)
					{
						Messages.Message("DeepDrillExhaustedNoFallback".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
					else
					{
						Messages.Message("DeepDrillExhausted".Translate(new object[]
						{
							Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(this.parent.Map).label, -1)
						}), this.parent, MessageTypeDefOf.TaskCompletion, true);
						this.parent.SetForbidden(true, true);
					}
				}
			}
		}

		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map) != null || this.ValuableResourcesPresent());
		}

		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		public bool UsedRecently()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		public override string CompInspectStringExtra()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			this.GetNextResource(out thingDef, out num, out intVec);
			string result;
			if (thingDef == null)
			{
				result = "DeepDrillNoResources".Translate();
			}
			else
			{
				result = string.Concat(new string[]
				{
					"ResourceBelow".Translate(),
					": ",
					thingDef.LabelCap,
					"\n",
					"ProgressToNextLump".Translate(),
					": ",
					this.ProgressToNextLumpPercent.ToStringPercent("F0")
				});
			}
			return result;
		}
	}
}
