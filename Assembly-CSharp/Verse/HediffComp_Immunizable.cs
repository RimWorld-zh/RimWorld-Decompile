using System.Text;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityPerDay
	{
		private float severityPerDayNotImmuneRandomFactor = 1f;

		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);

		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)base.props;
			}
		}

		public override string CompLabelInBracketsExtra
		{
			get
			{
				return (!this.FullyImmune) ? null : "DevelopedImmunityLower".Translate();
			}
		}

		public override string CompTipStringExtra
		{
			get
			{
				return (!base.Def.PossibleToDevelopImmunityNaturally() || this.FullyImmune) ? null : ("Immunity".Translate() + ": " + ((float)(Mathf.Floor((float)(this.Immunity * 100.0)) / 100.0)).ToStringPercent());
			}
		}

		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def);
			}
		}

		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1.0;
			}
		}

		public override TextureAndColor CompStateIcon
		{
			get
			{
				return (!this.FullyImmune) ? TextureAndColor.None : HediffComp_Immunizable.IconImmune;
			}
		}

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		protected override float SeverityChangePerDay()
		{
			return (!this.FullyImmune) ? (this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor) : this.Props.severityPerDayImmune;
		}

		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1.0)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + ((float)(immunityRecord.ImmunityChangePerTick(base.Pawn, true, base.parent) * 60000.0)).ToString("F3"));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
