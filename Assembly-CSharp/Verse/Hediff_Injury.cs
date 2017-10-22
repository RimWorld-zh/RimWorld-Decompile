using System.Text;
using UnityEngine;

namespace Verse
{
	public class Hediff_Injury : HediffWithComps
	{
		private static readonly Color OldInjuryColor = new Color(0.72f, 0.72f, 0.72f);

		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		public override string LabelBase
		{
			get
			{
				HediffComp_GetsOld hediffComp_GetsOld = this.TryGetComp<HediffComp_GetsOld>();
				string result;
				if (hediffComp_GetsOld != null && hediffComp_GetsOld.IsOld)
				{
					if (base.Part.def.IsDelicate && !hediffComp_GetsOld.Props.instantlyOldLabel.NullOrEmpty())
					{
						result = hediffComp_GetsOld.Props.instantlyOldLabel;
						goto IL_0088;
					}
					if (!hediffComp_GetsOld.Props.oldLabel.NullOrEmpty())
					{
						result = hediffComp_GetsOld.Props.oldLabel;
						goto IL_0088;
					}
				}
				result = base.LabelBase;
				goto IL_0088;
				IL_0088:
				return result;
			}
		}

		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (base.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(base.sourceHediffDef.label);
				}
				else if (base.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(base.source.label);
					if (base.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(base.sourceBodyPartGroup.label);
					}
				}
				return stringBuilder.ToString();
			}
		}

		public override Color LabelColor
		{
			get
			{
				return (!this.IsOld()) ? Color.white : Hediff_Injury.OldInjuryColor;
			}
		}

		public override string SeverityLabel
		{
			get
			{
				return (this.Severity != 0.0) ? this.Severity.ToString("0.##") : null;
			}
		}

		public override float SummaryHealthPercentImpact
		{
			get
			{
				return (float)((!this.IsOld() && this.Visible) ? (this.Severity / (75.0 * base.pawn.HealthScale)) : 0.0);
			}
		}

		public override float PainOffset
		{
			get
			{
				float result;
				if (base.pawn.Dead || base.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || base.causesNoPain)
				{
					result = 0f;
				}
				else
				{
					HediffComp_GetsOld hediffComp_GetsOld = this.TryGetComp<HediffComp_GetsOld>();
					result = ((hediffComp_GetsOld == null || !hediffComp_GetsOld.IsOld) ? (this.Severity * base.def.injuryProps.painPerSeverity) : (this.Severity * base.def.injuryProps.averagePainPerSeverityOld * hediffComp_GetsOld.painFactor));
				}
				return result;
			}
		}

		public override float BleedRate
		{
			get
			{
				float result;
				if (base.pawn.Dead)
				{
					result = 0f;
				}
				else if (this.BleedingStoppedDueToAge)
				{
					result = 0f;
				}
				else if (base.Part.def.IsSolid(base.Part, base.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsOld())
				{
					result = 0f;
				}
				else if (base.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					result = 0f;
				}
				else
				{
					float num = this.Severity * base.def.injuryProps.bleedRate;
					if (base.Part != null)
					{
						num *= base.Part.def.bleedingRateMultiplier;
					}
					result = num;
				}
				return result;
			}
		}

		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		private bool BleedingStoppedDueToAge
		{
			get
			{
				return base.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				base.pawn.health.Notify_HediffChanged(this);
			}
		}

		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (base.comps != null)
			{
				for (int i = 0; i < base.comps.Count; i++)
				{
					base.comps[i].CompPostInjuryHeal(amount);
				}
			}
			base.pawn.health.Notify_HediffChanged(this);
		}

		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == base.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsOld() && !this.IsTended() && !this.IsOld() && base.def.injuryProps.canMerge && base.TryMergeWith(other);
		}
	}
}
