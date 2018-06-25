using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class Hediff_Injury : HediffWithComps
	{
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);

		public Hediff_Injury()
		{
		}

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
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part.def.IsDelicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.source.label);
					if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(this.sourceBodyPartGroup.LabelShort);
					}
				}
				return stringBuilder.ToString();
			}
		}

		public override Color LabelColor
		{
			get
			{
				Color result;
				if (this.IsPermanent())
				{
					result = Hediff_Injury.PermanentInjuryColor;
				}
				else
				{
					result = Color.white;
				}
				return result;
			}
		}

		public override string SeverityLabel
		{
			get
			{
				string result;
				if (this.Severity == 0f)
				{
					result = null;
				}
				else
				{
					result = this.Severity.ToString("0.##");
				}
				return result;
			}
		}

		public override float SummaryHealthPercentImpact
		{
			get
			{
				float result;
				if (this.IsPermanent() || !this.Visible)
				{
					result = 0f;
				}
				else
				{
					result = this.Severity / (75f * this.pawn.HealthScale);
				}
				return result;
			}
		}

		public override float PainOffset
		{
			get
			{
				float result;
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					result = 0f;
				}
				else
				{
					HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
					if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
					{
						result = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.painFactor;
					}
					else
					{
						result = this.Severity * this.def.injuryProps.painPerSeverity;
					}
				}
				return result;
			}
		}

		public override float BleedRate
		{
			get
			{
				float result;
				if (this.pawn.Dead)
				{
					result = 0f;
				}
				else if (this.BleedingStoppedDueToAge)
				{
					result = 0f;
				}
				else if (base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsPermanent())
				{
					result = 0f;
				}
				else if (this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					result = 0f;
				}
				else
				{
					float num = this.Severity * this.def.injuryProps.bleedRate;
					if (base.Part != null)
					{
						num *= base.Part.def.bleedRate;
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
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Hediff_Injury()
		{
		}
	}
}
