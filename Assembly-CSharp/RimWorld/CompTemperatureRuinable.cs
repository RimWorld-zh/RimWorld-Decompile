using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073D RID: 1853
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x0400166B RID: 5739
		protected float ruinedPercent = 0f;

		// Token: 0x0400166C RID: 5740
		public const string RuinedSignal = "RuinedByTemperature";

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x0015DA40 File Offset: 0x0015BE40
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x0015DA60 File Offset: 0x0015BE60
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x0015DA85 File Offset: 0x0015BE85
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x0015DA9E File Offset: 0x0015BE9E
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x0015DAAC File Offset: 0x0015BEAC
		public override void CompTick()
		{
			this.DoTicks(1);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0015DAB6 File Offset: 0x0015BEB6
		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x0015DAC4 File Offset: 0x0015BEC4
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

		// Token: 0x060028FE RID: 10494 RVA: 0x0015DBB4 File Offset: 0x0015BFB4
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x0015DBF8 File Offset: 0x0015BFF8
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x0015DC28 File Offset: 0x0015C028
		public override void PostSplitOff(Thing piece)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)piece).GetComp<CompTemperatureRuinable>();
			comp.ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0015DC50 File Offset: 0x0015C050
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
	}
}
