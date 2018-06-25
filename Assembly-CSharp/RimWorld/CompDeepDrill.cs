using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000709 RID: 1801
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x040015CC RID: 5580
		private CompPowerTrader powerComp;

		// Token: 0x040015CD RID: 5581
		private float lumpProgress = 0f;

		// Token: 0x040015CE RID: 5582
		private float lumpYieldPct = 0f;

		// Token: 0x040015CF RID: 5583
		private int lastUsedTick = -99999;

		// Token: 0x040015D0 RID: 5584
		private const float ResourceLumpWork = 16000f;

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x00152CA8 File Offset: 0x001510A8
		public float ProgressToNextLumpPercent
		{
			get
			{
				return this.lumpProgress / 16000f;
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x00152CCA File Offset: 0x001510CA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x00152CE0 File Offset: 0x001510E0
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.lumpProgress, "lumpProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.lumpYieldPct, "lumpYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x00152D2C File Offset: 0x0015112C
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

		// Token: 0x06002776 RID: 10102 RVA: 0x00152DBC File Offset: 0x001511BC
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

		// Token: 0x06002777 RID: 10103 RVA: 0x00152F08 File Offset: 0x00151308
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x00152F3C File Offset: 0x0015133C
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x00152F98 File Offset: 0x00151398
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x00152FBC File Offset: 0x001513BC
		public bool UsedRecently()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00152FE8 File Offset: 0x001513E8
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
