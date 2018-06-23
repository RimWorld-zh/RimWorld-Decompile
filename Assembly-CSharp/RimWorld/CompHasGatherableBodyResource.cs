using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000714 RID: 1812
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x040015DF RID: 5599
		protected float fullness = 0f;

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060027D5 RID: 10197
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060027D6 RID: 10198
		protected abstract int ResourceAmount { get; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060027D7 RID: 10199
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060027D8 RID: 10200
		protected abstract string SaveKey { get; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x00155164 File Offset: 0x00153564
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060027DA RID: 10202 RVA: 0x00155180 File Offset: 0x00153580
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x001551B0 File Offset: 0x001535B0
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x001551E7 File Offset: 0x001535E7
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x00155208 File Offset: 0x00153608
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

		// Token: 0x060027DE RID: 10206 RVA: 0x0015527C File Offset: 0x0015367C
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent, false);
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield, true)))
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
			}
			this.fullness = 0f;
		}
	}
}
