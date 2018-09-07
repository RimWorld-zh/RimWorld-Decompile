using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		private HediffDef hediff;

		private FloatRange severityRange;

		[CompilerGenerated]
		private static Func<HediffDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<HediffDef, bool> <>f__am$cache1;

		public ScenPart_ForcedHediff()
		{
		}

		private float MaxSeverity
		{
			get
			{
				return (this.hediff.lethalSeverity <= 0f) ? 1f : (this.hediff.lethalSeverity * 0.99f);
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<HediffDef>(this.PossibleHediffs(), (HediffDef hd) => hd.LabelCap, (HediffDef hd) => delegate()
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
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
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
			return "ScenPart_PawnsHaveHediff".Translate(new object[]
			{
				this.context.ToStringHuman(),
				this.chance.ToStringPercent(),
				this.hediff.label
			}).CapitalizeFirst();
		}

		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
			if (this.hideOffMap)
			{
				if (!req.AllowDead && pawn.health.WouldDieAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
				if (!req.AllowDowned && pawn.health.WouldBeDownedAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
			}
			return true;
		}

		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		[CompilerGenerated]
		private static string <DoEditInterface>m__0(HediffDef hd)
		{
			return hd.LabelCap;
		}

		[CompilerGenerated]
		private Action <DoEditInterface>m__1(HediffDef hd)
		{
			return delegate()
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
			};
		}

		[CompilerGenerated]
		private static bool <PossibleHediffs>m__2(HediffDef x)
		{
			return x.scenarioCanAdd;
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey0
		{
			internal HediffDef hd;

			internal ScenPart_ForcedHediff $this;

			public <DoEditInterface>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.hediff = this.hd;
				if (this.$this.severityRange.max > this.$this.MaxSeverity)
				{
					this.$this.severityRange.max = this.$this.MaxSeverity;
				}
				if (this.$this.severityRange.min > this.$this.MaxSeverity)
				{
					this.$this.severityRange.min = this.$this.MaxSeverity;
				}
			}
		}
	}
}
