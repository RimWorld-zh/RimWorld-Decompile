using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D2E RID: 3374
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x04003246 RID: 12870
		public HediffDef lastInjury = null;

		// Token: 0x04003247 RID: 12871
		private bool isFreshInt = false;

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004A5C RID: 19036 RVA: 0x0026CD40 File Offset: 0x0026B140
		public override float SummaryHealthPercentImpact
		{
			get
			{
				float result;
				if (!this.IsFreshNonSolidExtremity)
				{
					result = 0f;
				}
				else if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					result = 0f;
				}
				else
				{
					result = (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
				}
				return result;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x0026CDD4 File Offset: 0x0026B1D4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x0026CDEC File Offset: 0x0026B1EC
		public override string LabelBase
		{
			get
			{
				string result;
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					result = "RemovedBodyPart".Translate();
				}
				else if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					result = HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				else if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					result = this.lastInjury.injuryProps.destroyedOutLabel;
				}
				else
				{
					result = this.lastInjury.injuryProps.destroyedLabel;
				}
				return result;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06004A5F RID: 19039 RVA: 0x0026CEE8 File Offset: 0x0026B2E8
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x0026CF4C File Offset: 0x0026B34C
		public override float BleedRate
		{
			get
			{
				float result;
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					result = 0f;
				}
				else
				{
					result = base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate;
				}
				return result;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x0026CFCC File Offset: 0x0026B3CC
		public override float PainOffset
		{
			get
			{
				float result;
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					result = 0f;
				}
				else
				{
					result = base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity;
				}
				return result;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x0026D048 File Offset: 0x0026B448
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06004A63 RID: 19043 RVA: 0x0026D0D0 File Offset: 0x0026B4D0
		// (set) Token: 0x06004A64 RID: 19044 RVA: 0x0026D0FC File Offset: 0x0026B4FC
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				this.isFreshInt = value;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x0026D108 File Offset: 0x0026B508
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x0026D190 File Offset: 0x0026B590
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x0026D1B8 File Offset: 0x0026B5B8
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0026D1D4 File Offset: 0x0026B5D4
		public override void Tick()
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.Tick();
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0026D20E File Offset: 0x0026B60E
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0026D234 File Offset: 0x0026B634
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0026D304 File Offset: 0x0026B704
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}
	}
}
