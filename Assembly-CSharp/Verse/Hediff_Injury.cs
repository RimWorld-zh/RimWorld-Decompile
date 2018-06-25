using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D2D RID: 3373
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x04003245 RID: 12869
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004A4C RID: 19020 RVA: 0x0026C6F4 File Offset: 0x0026AAF4
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

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0026C728 File Offset: 0x0026AB28
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

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0026C7C0 File Offset: 0x0026ABC0
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

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004A4F RID: 19023 RVA: 0x0026C88C File Offset: 0x0026AC8C
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

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0026C8BC File Offset: 0x0026ACBC
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

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x0026C8FC File Offset: 0x0026ACFC
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

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004A52 RID: 19026 RVA: 0x0026C94C File Offset: 0x0026AD4C
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

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004A53 RID: 19027 RVA: 0x0026C9FC File Offset: 0x0026ADFC
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

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06004A54 RID: 19028 RVA: 0x0026CAF8 File Offset: 0x0026AEF8
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x0026CB54 File Offset: 0x0026AF54
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0026CB7C File Offset: 0x0026AF7C
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

		// Token: 0x06004A57 RID: 19031 RVA: 0x0026CBB8 File Offset: 0x0026AFB8
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

		// Token: 0x06004A58 RID: 19032 RVA: 0x0026CC24 File Offset: 0x0026B024
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0026CCB8 File Offset: 0x0026B0B8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}
	}
}
