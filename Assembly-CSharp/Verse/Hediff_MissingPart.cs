using System.Text;

namespace Verse
{
	public class Hediff_MissingPart : HediffWithComps
	{
		public HediffDef lastInjury;

		private bool isFreshInt;

		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty() && base.Part.parts.NullOrEmpty() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)((float)base.Part.def.hitPoints / (75.0 * base.pawn.HealthScale));
			}
		}

		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		public override bool TendableNow
		{
			get
			{
				return this.IsFreshNonSolidExtremity;
			}
		}

		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury != null && base.Part.depth != BodyPartDepth.Inside)
				{
					if (base.Part.def.useDestroyedOutLabel && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
					{
						return this.lastInjury.injuryProps.destroyedOutLabel;
					}
					return this.lastInjury.injuryProps.destroyedLabel;
				}
				bool solid = base.Part.def.IsSolid(base.Part, base.pawn.health.hediffSet.hediffs);
				return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
			}
		}

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

		public override float BleedRate
		{
			get
			{
				if (!base.pawn.Dead && this.IsFreshNonSolidExtremity && !this.ParentIsMissing)
				{
					return base.Part.def.GetMaxHealth(base.pawn) * base.def.injuryProps.bleedRate * base.Part.def.bleedingRateMultiplier;
				}
				return 0f;
			}
		}

		public override float PainOffset
		{
			get
			{
				if (!base.pawn.Dead && !base.causesNoPain && this.IsFreshNonSolidExtremity && !this.ParentIsMissing)
				{
					return base.Part.def.GetMaxHealth(base.pawn) * base.def.injuryProps.painPerSeverity;
				}
				return 0f;
			}
		}

		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < base.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = base.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

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

		public bool IsFreshNonSolidExtremity
		{
			get
			{
				if (Current.ProgramState == ProgramState.Entry)
				{
					return false;
				}
				if (this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, base.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing)
				{
					return true;
				}
				return false;
			}
		}

		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return base.ageTicks >= 90000;
			}
		}

		public override void Tick()
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.Tick();
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				base.pawn.health.Notify_HediffChanged(this);
			}
		}

		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, batchPosition);
			this.IsFresh = false;
			base.pawn.health.Notify_HediffChanged(this);
		}

		public override void PostAdd(DamageInfo? dinfo)
		{
			base.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(base.def, base.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				base.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
		}
	}
}
