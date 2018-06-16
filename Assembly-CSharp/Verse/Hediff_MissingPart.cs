using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D2F RID: 3375
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004A46 RID: 19014 RVA: 0x0026B424 File Offset: 0x00269824
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
		// (get) Token: 0x06004A47 RID: 19015 RVA: 0x0026B4B8 File Offset: 0x002698B8
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004A48 RID: 19016 RVA: 0x0026B4D0 File Offset: 0x002698D0
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
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x0026B5CC File Offset: 0x002699CC
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
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x0026B630 File Offset: 0x00269A30
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
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0026B6B0 File Offset: 0x00269AB0
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
		// (get) Token: 0x06004A4C RID: 19020 RVA: 0x0026B72C File Offset: 0x00269B2C
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
		// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0026B7B4 File Offset: 0x00269BB4
		// (set) Token: 0x06004A4E RID: 19022 RVA: 0x0026B7E0 File Offset: 0x00269BE0
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
		// (get) Token: 0x06004A4F RID: 19023 RVA: 0x0026B7EC File Offset: 0x00269BEC
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0026B874 File Offset: 0x00269C74
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0026B89C File Offset: 0x00269C9C
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0026B8B8 File Offset: 0x00269CB8
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

		// Token: 0x06004A53 RID: 19027 RVA: 0x0026B8F2 File Offset: 0x00269CF2
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0026B918 File Offset: 0x00269D18
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

		// Token: 0x06004A55 RID: 19029 RVA: 0x0026B9E5 File Offset: 0x00269DE5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
		}

		// Token: 0x04003236 RID: 12854
		public HediffDef lastInjury = null;

		// Token: 0x04003237 RID: 12855
		private bool isFreshInt = false;
	}
}
