using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000718 RID: 1816
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060027DD RID: 10205
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060027DE RID: 10206
		protected abstract int ResourceAmount { get; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060027DF RID: 10207
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060027E0 RID: 10208
		protected abstract string SaveKey { get; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x00154FAC File Offset: 0x001533AC
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060027E2 RID: 10210 RVA: 0x00154FC8 File Offset: 0x001533C8
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x00154FF8 File Offset: 0x001533F8
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x0015502F File Offset: 0x0015342F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00155050 File Offset: 0x00153450
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (float)(this.GatherResourcesIntervalDays * 60000);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x001550C4 File Offset: 0x001534C4
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent, false);
			}
			if (Rand.Value > doer.GetStatValue(StatDefOf.AnimalGatherYield, true))
			{
				Vector3 loc = (doer.DrawPos + this.parent.DrawPos) / 2f;
				MoteMaker.ThrowText(loc, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.ResourceDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null);
				}
				this.fullness = 0f;
			}
		}

		// Token: 0x040015E1 RID: 5601
		protected float fullness = 0f;
	}
}
