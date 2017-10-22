using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompDeepDrill : ThingComp
	{
		private CompPowerTrader powerComp;

		private float lumpProgress = 0f;

		private float lumpYieldPct = 0f;

		private const float ResourceLumpWork = 14000f;

		public float ProgressToNextLumpPercent
		{
			get
			{
				return (float)(this.lumpProgress / 14000.0);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = base.parent.TryGetComp<CompPowerTrader>();
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.lumpProgress, "lumpProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.lumpYieldPct, "lumpYieldPct", 0f, false);
		}

		public void DrillWorkDone(Pawn driller)
		{
			float statValue = driller.GetStatValue(StatDefOf.MiningSpeed, true);
			this.lumpProgress += statValue;
			this.lumpYieldPct += (float)(statValue * driller.GetStatValue(StatDefOf.MiningYield, true) / 14000.0);
			if (this.lumpProgress > 14000.0)
			{
				this.TryProduceLump(this.lumpYieldPct);
				this.lumpProgress = 0f;
				this.lumpYieldPct = 0f;
			}
		}

		private void TryProduceLump(float yieldPct)
		{
			ThingDef thingDef = default(ThingDef);
			int num = default(int);
			IntVec3 c = default(IntVec3);
			if (this.TryGetNextResource(out thingDef, out num, out c))
			{
				int num2 = Mathf.Min(num, thingDef.deepCountPerCell / 2, thingDef.stackLimit);
				base.parent.Map.deepResourceGrid.SetAt(c, thingDef, num - num2);
				int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, base.parent.InteractionCell, base.parent.Map, ThingPlaceMode.Near, null);
			}
			else
			{
				Log.Error("Drill tried to ProduceLump but couldn't.");
			}
			if (!this.ResourcesPresent())
			{
				Messages.Message("DeepDrillExhausted".Translate(), (Thing)base.parent, MessageTypeDefOf.TaskCompletion);
			}
		}

		public bool TryGetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < 9)
				{
					IntVec3 intVec = base.parent.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(base.parent.Map))
					{
						ThingDef thingDef = base.parent.Map.deepResourceGrid.ThingDefAt(intVec);
						if (thingDef != null)
						{
							resDef = thingDef;
							countPresent = base.parent.Map.deepResourceGrid.CountAt(intVec);
							cell = intVec;
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				resDef = null;
				countPresent = 0;
				cell = IntVec3.Invalid;
				result = false;
				break;
			}
			return result;
		}

		public bool CanDrillNow()
		{
			return (byte)((this.powerComp == null || this.powerComp.PowerOn) ? (this.ResourcesPresent() ? 1 : 0) : 0) != 0;
		}

		public bool ResourcesPresent()
		{
			ThingDef thingDef = default(ThingDef);
			int num = default(int);
			IntVec3 intVec = default(IntVec3);
			return this.TryGetNextResource(out thingDef, out num, out intVec);
		}

		public override string CompInspectStringExtra()
		{
			ThingDef thingDef = default(ThingDef);
			int num = default(int);
			IntVec3 intVec = default(IntVec3);
			return (!this.TryGetNextResource(out thingDef, out num, out intVec)) ? ("ResourceBelow".Translate() + ": " + "NothingLower".Translate()) : ("ResourceBelow".Translate() + ": " + thingDef.label + "\n" + "ProgressToNextLump".Translate() + ": " + this.ProgressToNextLumpPercent.ToStringPercent("F0"));
		}
	}
}
