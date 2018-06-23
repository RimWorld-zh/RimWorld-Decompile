using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class CompEggLayer : ThingComp
	{
		// Token: 0x040015CF RID: 5583
		private float eggProgress = 0f;

		// Token: 0x040015D0 RID: 5584
		private int fertilizationCount = 0;

		// Token: 0x040015D1 RID: 5585
		private Pawn fertilizedBy;

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x0015320C File Offset: 0x0015160C
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x0015327C File Offset: 0x0015167C
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x001532B4 File Offset: 0x001516B4
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x001532E0 File Offset: 0x001516E0
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x0015331C File Offset: 0x0015171C
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x0015333C File Offset: 0x0015173C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x0015338C File Offset: 0x0015178C
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

		// Token: 0x0600278B RID: 10123 RVA: 0x0015341F File Offset: 0x0015181F
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x0015343C File Offset: 0x0015183C
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

		// Token: 0x0600278D RID: 10125 RVA: 0x00153544 File Offset: 0x00151944
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
