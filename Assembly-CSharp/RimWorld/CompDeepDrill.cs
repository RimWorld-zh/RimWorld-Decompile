using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x001526DC File Offset: 0x00150ADC
		public float ProgressToNextLumpPercent
		{
			get
			{
				return this.lumpProgress / 16000f;
			}
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x001526FE File Offset: 0x00150AFE
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x00152714 File Offset: 0x00150B14
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.lumpProgress, "lumpProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.lumpYieldPct, "lumpYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x00152760 File Offset: 0x00150B60
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

		// Token: 0x06002779 RID: 10105 RVA: 0x001527F0 File Offset: 0x00150BF0
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

		// Token: 0x0600277A RID: 10106 RVA: 0x0015293C File Offset: 0x00150D3C
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00152970 File Offset: 0x00150D70
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x001529CC File Offset: 0x00150DCC
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x001529F0 File Offset: 0x00150DF0
		public bool UsedRecently()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00152A1C File Offset: 0x00150E1C
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

		// Token: 0x040015CA RID: 5578
		private CompPowerTrader powerComp;

		// Token: 0x040015CB RID: 5579
		private float lumpProgress = 0f;

		// Token: 0x040015CC RID: 5580
		private float lumpYieldPct = 0f;

		// Token: 0x040015CD RID: 5581
		private int lastUsedTick = -99999;

		// Token: 0x040015CE RID: 5582
		private const float ResourceLumpWork = 16000f;
	}
}
