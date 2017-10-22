using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompTemperatureRuinable : ThingComp
	{
		protected float ruinedPercent = 0f;

		public const string RuinedSignal = "RuinedByTemperature";

		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)base.props;
			}
		}

		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1.0;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		public override void CompTick()
		{
			this.DoTicks(1);
		}

		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		private void DoTicks(int ticks)
		{
			if (!this.Ruined)
			{
				float ambientTemperature = base.parent.AmbientTemperature;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				else if (ambientTemperature < this.Props.minSafeTemperature)
				{
					this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				if (this.ruinedPercent >= 1.0)
				{
					this.ruinedPercent = 1f;
					base.parent.BroadcastCompSignal("RuinedByTemperature");
				}
				else if (this.ruinedPercent < 0.0)
				{
					this.ruinedPercent = 0f;
				}
			}
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(base.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		public override void PostSplitOff(Thing piece)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)piece).GetComp<CompTemperatureRuinable>();
			comp.ruinedPercent = this.ruinedPercent;
		}

		public override string CompInspectStringExtra()
		{
			string result;
			string str;
			if (this.Ruined)
			{
				result = "RuinedByTemperature".Translate();
			}
			else if (this.ruinedPercent > 0.0)
			{
				float ambientTemperature = base.parent.AmbientTemperature;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
					goto IL_0082;
				}
				if (ambientTemperature < this.Props.minSafeTemperature)
				{
					str = "Freezing".Translate();
					goto IL_0082;
				}
				result = (string)null;
			}
			else
			{
				result = (string)null;
			}
			goto IL_00a5;
			IL_0082:
			result = str + ": " + this.ruinedPercent.ToStringPercent();
			goto IL_00a5;
			IL_00a5:
			return result;
		}
	}
}
