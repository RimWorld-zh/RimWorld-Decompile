using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000741 RID: 1857
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x0015D868 File Offset: 0x0015BC68
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x0015D888 File Offset: 0x0015BC88
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x0015D8AD File Offset: 0x0015BCAD
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0015D8C6 File Offset: 0x0015BCC6
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0015D8D4 File Offset: 0x0015BCD4
		public override void CompTick()
		{
			this.DoTicks(1);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0015D8DE File Offset: 0x0015BCDE
		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0015D8EC File Offset: 0x0015BCEC
		private void DoTicks(int ticks)
		{
			if (!this.Ruined)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				else if (ambientTemperature < this.Props.minSafeTemperature)
				{
					this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				if (this.ruinedPercent >= 1f)
				{
					this.ruinedPercent = 1f;
					this.parent.BroadcastCompSignal("RuinedByTemperature");
				}
				else if (this.ruinedPercent < 0f)
				{
					this.ruinedPercent = 0f;
				}
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0015D9DC File Offset: 0x0015BDDC
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0015DA20 File Offset: 0x0015BE20
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0015DA50 File Offset: 0x0015BE50
		public override void PostSplitOff(Thing piece)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)piece).GetComp<CompTemperatureRuinable>();
			comp.ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0015DA78 File Offset: 0x0015BE78
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.Ruined)
			{
				result = "RuinedByTemperature".Translate();
			}
			else if (this.ruinedPercent > 0f)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				string str;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
				}
				else
				{
					if (ambientTemperature >= this.Props.minSafeTemperature)
					{
						return null;
					}
					str = "Freezing".Translate();
				}
				result = str + ": " + this.ruinedPercent.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400166D RID: 5741
		protected float ruinedPercent = 0f;

		// Token: 0x0400166E RID: 5742
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
