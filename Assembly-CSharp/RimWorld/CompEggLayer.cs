using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070D RID: 1805
	public class CompEggLayer : ThingComp
	{
		// Token: 0x040015CF RID: 5583
		private float eggProgress = 0f;

		// Token: 0x040015D0 RID: 5584
		private int fertilizationCount = 0;

		// Token: 0x040015D1 RID: 5585
		private Pawn fertilizedBy;

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x0015335C File Offset: 0x0015175C
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x001533CC File Offset: 0x001517CC
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x00153404 File Offset: 0x00151804
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x00153430 File Offset: 0x00151830
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x0015346C File Offset: 0x0015186C
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x0015348C File Offset: 0x0015188C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x001534DC File Offset: 0x001518DC
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (this.Props.eggLayIntervalDays * 60000f);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.eggProgress += num;
				if (this.eggProgress > 1f)
				{
					this.eggProgress = 1f;
				}
				if (this.ProgressStoppedBecauseUnfertilized)
				{
					this.eggProgress = this.Props.eggProgressUnfertilizedMax;
				}
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x0015356F File Offset: 0x0015196F
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0015358C File Offset: 0x0015198C
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				Log.Error("LayEgg while not Active: " + this.parent, false);
			}
			this.eggProgress = 0f;
			int randomInRange = this.Props.eggCountRange.RandomInRange;
			Thing result;
			if (randomInRange == 0)
			{
				result = null;
			}
			else
			{
				Thing thing;
				if (this.fertilizationCount > 0)
				{
					thing = ThingMaker.MakeThing(this.Props.eggFertilizedDef, null);
					this.fertilizationCount = Mathf.Max(0, this.fertilizationCount - randomInRange);
				}
				else
				{
					thing = ThingMaker.MakeThing(this.Props.eggUnfertilizedDef, null);
				}
				thing.stackCount = randomInRange;
				CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
				if (compHatcher != null)
				{
					compHatcher.hatcheeFaction = this.parent.Faction;
					Pawn pawn = this.parent as Pawn;
					if (pawn != null)
					{
						compHatcher.hatcheeParent = pawn;
					}
					if (this.fertilizedBy != null)
					{
						compHatcher.otherParent = this.fertilizedBy;
					}
				}
				result = thing;
			}
			return result;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x00153694 File Offset: 0x00151A94
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				string text = "EggProgress".Translate() + ": " + this.eggProgress.ToStringPercent();
				if (this.fertilizationCount > 0)
				{
					text = text + "\n" + "Fertilized".Translate();
				}
				else if (this.ProgressStoppedBecauseUnfertilized)
				{
					text = text + "\n" + "ProgressStoppedUntilFertilized".Translate();
				}
				result = text;
			}
			return result;
		}
	}
}
