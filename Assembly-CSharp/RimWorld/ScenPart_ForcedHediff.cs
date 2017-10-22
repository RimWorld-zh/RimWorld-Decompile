using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		private HediffDef hediff;

		private FloatRange severityRange;

		private float MaxSeverity
		{
			get
			{
				return (float)((!(this.hediff.lethalSeverity > 0.0)) ? 1.0 : (this.hediff.lethalSeverity * 0.99000000953674316));
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, (float)(ScenPart.RowHeight * 3.0 + 31.0));
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu(this.PossibleHediffs(), (Func<HediffDef, string>)((HediffDef hd) => hd.LabelCap), (Func<HediffDef, Action>)((HediffDef hd) => (Action)delegate()
				{
					this.hediff = hd;
					if (this.severityRange.max > this.MaxSeverity)
					{
						this.severityRange.max = this.MaxSeverity;
					}
					if (this.severityRange.min > this.MaxSeverity)
					{
						this.severityRange.min = this.MaxSeverity;
					}
				}));
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels((float)(ScenPart.RowHeight * 2.0)));
		}

		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(base.context.ToStringHuman(), base.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement();
			this.severityRange.max = Rand.Range((float)(this.MaxSeverity * 0.20000000298023224), (float)(this.MaxSeverity * 0.949999988079071));
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				base.chance = GenMath.ChanceEitherHappens(base.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		protected override void ModifyPawn(Pawn p)
		{
			if (Rand.Value < base.chance)
			{
				Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
				hediff.Severity = this.severityRange.RandomInRange;
				p.health.AddHediff(hediff, null, default(DamageInfo?));
			}
		}
	}
}
