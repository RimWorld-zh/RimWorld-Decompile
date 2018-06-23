using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x040015C8 RID: 5576
		private CompPowerTrader powerComp;

		// Token: 0x040015C9 RID: 5577
		private float lumpProgress = 0f;

		// Token: 0x040015CA RID: 5578
		private float lumpYieldPct = 0f;

		// Token: 0x040015CB RID: 5579
		private int lastUsedTick = -99999;

		// Token: 0x040015CC RID: 5580
		private const float ResourceLumpWork = 16000f;

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x001528F8 File Offset: 0x00150CF8
		public float ProgressToNextLumpPercent
		{
			get
			{
				return this.lumpProgress / 16000f;
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x0015291A File Offset: 0x00150D1A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x00152930 File Offset: 0x00150D30
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.lumpProgress, "lumpProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.lumpYieldPct, "lumpYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x0015297C File Offset: 0x00150D7C
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

		// Token: 0x06002773 RID: 10099 RVA: 0x00152A0C File Offset: 0x00150E0C
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

		// Token: 0x06002774 RID: 10100 RVA: 0x00152B58 File Offset: 0x00150F58
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x00152B8C File Offset: 0x00150F8C
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x00152BE8 File Offset: 0x00150FE8
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x00152C0C File Offset: 0x0015100C
		public bool UsedRecently()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x00152C38 File Offset: 0x00151038
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
