using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D2D RID: 3373
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x0400323F RID: 12863
		public HediffDef lastInjury = null;

		// Token: 0x04003240 RID: 12864
		private bool isFreshInt = false;

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004A5C RID: 19036 RVA: 0x0026CA60 File Offset: 0x0026AE60
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
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x0026CAF4 File Offset: 0x0026AEF4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x0026CB0C File Offset: 0x0026AF0C
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
		// (get) Token: 0x06004A5F RID: 19039 RVA: 0x0026CC08 File Offset: 0x0026B008
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
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x0026CC6C File Offset: 0x0026B06C
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
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x0026CCEC File Offset: 0x0026B0EC
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
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x0026CD68 File Offset: 0x0026B168
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
		// (get) Token: 0x06004A63 RID: 19043 RVA: 0x0026CDF0 File Offset: 0x0026B1F0
		// (set) Token: 0x06004A64 RID: 19044 RVA: 0x0026CE1C File Offset: 0x0026B21C
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
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x0026CE28 File Offset: 0x0026B228
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x0026CEB0 File Offset: 0x0026B2B0
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x0026CED8 File Offset: 0x0026B2D8
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0026CEF4 File Offset: 0x0026B2F4
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

		// Token: 0x06004A69 RID: 19049 RVA: 0x0026CF2E File Offset: 0x0026B32E
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0026CF54 File Offset: 0x0026B354
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

		// Token: 0x06004A6B RID: 19051 RVA: 0x0026D024 File Offset: 0x0026B424
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
