using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000634 RID: 1588
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x00117CB4 File Offset: 0x001160B4
		private float MaxSeverity
		{
			get
			{
				return (this.hediff.lethalSeverity <= 0f) ? 1f : (this.hediff.lethalSeverity * 0.99f);
			}
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00117CFC File Offset: 0x001160FC
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

		// Token: 0x060020B2 RID: 8370 RVA: 0x00117DE8 File Offset: 0x001161E8
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00117E24 File Offset: 0x00116224
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00117E64 File Offset: 0x00116264
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(new object[]
			{
				this.context.ToStringHuman(),
				this.chance.ToStringPercent(),
				this.hediff.label
			}).CapitalizeFirst();
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00117EB8 File Offset: 0x001162B8
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00117F2C File Offset: 0x0011632C
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			bool result;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00117F80 File Offset: 0x00116380
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			bool result;
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				result = false;
			}
			else
			{
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
				result = true;
			}
			return result;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00118022 File Offset: 0x00116422
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x0011802C File Offset: 0x0011642C
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00118038 File Offset: 0x00116438
		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x040012C0 RID: 4800
		private HediffDef hediff;

		// Token: 0x040012C1 RID: 4801
		private FloatRange severityRange;
	}
}
