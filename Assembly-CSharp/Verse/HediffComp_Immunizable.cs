using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D21 RID: 3361
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityPerDay
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x0026A3B4 File Offset: 0x002687B4
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x0026A3D4 File Offset: 0x002687D4
		public override string CompLabelInBracketsExtra
		{
			get
			{
				string result;
				if (this.FullyImmune)
				{
					result = "DevelopedImmunityLower".Translate();
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x0026A408 File Offset: 0x00268808
		public override string CompTipStringExtra
		{
			get
			{
				string result;
				if (base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					result = "Immunity".Translate() + ": " + (Mathf.Floor(this.Immunity * 100f) / 100f).ToStringPercent();
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x0026A470 File Offset: 0x00268870
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def);
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x060049F7 RID: 18935 RVA: 0x0026A4A0 File Offset: 0x002688A0
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x0026A4C8 File Offset: 0x002688C8
		public override TextureAndColor CompStateIcon
		{
			get
			{
				TextureAndColor result;
				if (this.FullyImmune)
				{
					result = HediffComp_Immunizable.IconImmune;
				}
				else
				{
					result = TextureAndColor.None;
				}
				return result;
			}
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0026A4FD File Offset: 0x002688FD
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0026A51D File Offset: 0x0026891D
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0026A53C File Offset: 0x0026893C
		protected override float SeverityChangePerDay()
		{
			return (!this.FullyImmune) ? (this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor) : this.Props.severityPerDayImmune;
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x0026A580 File Offset: 0x00268980
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("  pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true), ToStringNumberSense.Absolute));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003227 RID: 12839
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x04003228 RID: 12840
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
