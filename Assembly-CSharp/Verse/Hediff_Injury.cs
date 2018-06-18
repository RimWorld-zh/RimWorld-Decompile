using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D2D RID: 3373
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004A35 RID: 18997 RVA: 0x0026AE04 File Offset: 0x00269204
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

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x0026AE38 File Offset: 0x00269238
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

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004A37 RID: 18999 RVA: 0x0026AED0 File Offset: 0x002692D0
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

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06004A38 RID: 19000 RVA: 0x0026AF9C File Offset: 0x0026939C
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

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004A39 RID: 19001 RVA: 0x0026AFCC File Offset: 0x002693CC
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

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0026B00C File Offset: 0x0026940C
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

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004A3B RID: 19003 RVA: 0x0026B05C File Offset: 0x0026945C
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

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0026B10C File Offset: 0x0026950C
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

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004A3D RID: 19005 RVA: 0x0026B208 File Offset: 0x00269608
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06004A3E RID: 19006 RVA: 0x0026B264 File Offset: 0x00269664
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0026B28C File Offset: 0x0026968C
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

		// Token: 0x06004A40 RID: 19008 RVA: 0x0026B2C8 File Offset: 0x002696C8
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

		// Token: 0x06004A41 RID: 19009 RVA: 0x0026B334 File Offset: 0x00269734
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x04003233 RID: 12851
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
