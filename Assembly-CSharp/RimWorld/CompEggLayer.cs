using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070F RID: 1807
	public class CompEggLayer : ThingComp
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x00152FEC File Offset: 0x001513EC
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x0015305C File Offset: 0x0015145C
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x00153094 File Offset: 0x00151494
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x001530C0 File Offset: 0x001514C0
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x001530FC File Offset: 0x001514FC
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x0015311C File Offset: 0x0015151C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0015316C File Offset: 0x0015156C
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

		// Token: 0x06002791 RID: 10129 RVA: 0x001531FF File Offset: 0x001515FF
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x0015321C File Offset: 0x0015161C
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

		// Token: 0x06002793 RID: 10131 RVA: 0x00153324 File Offset: 0x00151724
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

		// Token: 0x040015D1 RID: 5585
		private float eggProgress = 0f;

		// Token: 0x040015D2 RID: 5586
		private int fertilizationCount = 0;

		// Token: 0x040015D3 RID: 5587
		private Pawn fertilizedBy;
	}
}
