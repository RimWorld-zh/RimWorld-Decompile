using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		protected float fullness = 0f;

		protected abstract int GatherResourcesIntervalDays
		{
			get;
		}

		protected abstract int ResourceAmount
		{
			get;
		}

		protected abstract ThingDef ResourceDef
		{
			get;
		}

		protected abstract string SaveKey
		{
			get;
		}

		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		protected virtual bool Active
		{
			get
			{
				return (byte)((base.parent.Faction != null) ? 1 : 0) != 0;
			}
		}

		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1.0;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		public override void CompTick()
		{
			if (this.Active)
			{
				float num = (float)(1.0 / (float)(this.GatherResourcesIntervalDays * 60000));
				Pawn pawn = base.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1.0)
				{
					this.fullness = 1f;
				}
			}
		}

		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + base.parent);
			}
			if (Rand.Value > doer.GetStatValue(StatDefOf.AnimalGatherYield, true))
			{
				Vector3 loc = (doer.DrawPos + base.parent.DrawPos) / 2f;
				MoteMaker.ThrowText(loc, base.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int num = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (num > 0)
				{
					int num2 = Mathf.Clamp(num, 1, this.ResourceDef.stackLimit);
					num -= num2;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num2;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null);
				}
				this.fullness = 0f;
			}
		}
	}
}
