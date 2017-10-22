using System.Text;

namespace Verse
{
	public class Hediff_MissingPart : HediffWithComps
	{
		public HediffDef lastInjury = null;

		private bool isFreshInt = false;

		public override float SummaryHealthPercentImpact
		{
			get
			{
				return (float)(this.IsFreshNonSolidExtremity ? ((!base.Part.def.tags.NullOrEmpty() || !base.Part.parts.NullOrEmpty() || base.Bleeding) ? ((float)base.Part.def.hitPoints / (75.0 * base.pawn.HealthScale)) : 0.0) : 0.0);
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
				string result;
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					result = "RemovedBodyPart".Translate();
				}
				else if (this.lastInjury != null && base.Part.depth != BodyPartDepth.Inside)
				{
					result = ((!base.Part.def.useDestroyedOutLabel || this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty()) ? this.lastInjury.injuryProps.destroyedLabel : this.lastInjury.injuryProps.destroyedOutLabel);
				}
				else
				{
					bool solid = base.Part.def.IsSolid(base.Part, base.pawn.health.hediffSet.hediffs);
					result = HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				return result;
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
				return (float)((!base.pawn.Dead && this.IsFreshNonSolidExtremity && !this.ParentIsMissing) ? (base.Part.def.GetMaxHealth(base.pawn) * base.def.injuryProps.bleedRate * base.Part.def.bleedingRateMultiplier) : 0.0);
			}
		}

		public override float PainOffset
		{
			get
			{
				return (float)((!base.pawn.Dead && !base.causesNoPain && this.IsFreshNonSolidExtremity && !this.ParentIsMissing) ? (base.Part.def.GetMaxHealth(base.pawn) * base.def.injuryProps.painPerSeverity) : 0.0);
			}
		}

		private bool ParentIsMissing
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < base.pawn.health.hediffSet.hediffs.Count)
					{
						Hediff_MissingPart hediff_MissingPart = base.pawn.health.hediffSet.hediffs[num] as Hediff_MissingPart;
						if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
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
				return (byte)((Current.ProgramState != 0) ? ((this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, base.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing) ? 1 : 0) : 0) != 0;
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
				base.pawn.health.hediffSet.AddDirect(hediff_MissingPart, default(DamageInfo?));
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
