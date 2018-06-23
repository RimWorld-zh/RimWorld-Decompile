using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B6 RID: 2486
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x040023BC RID: 9148
		public StatDef stuffPowerStat;

		// Token: 0x040023BD RID: 9149
		public StatDef multiplierStat;

		// Token: 0x060037B3 RID: 14259 RVA: 0x001DA9F8 File Offset: 0x001D8DF8
		public override string ExplanationPart(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.Def.MadeFromStuff)
			{
				string text = (req.StuffDef == null) ? "None".Translate() : req.StuffDef.LabelCap;
				string text2 = (req.StuffDef == null) ? "0" : req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null).ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute);
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					"StatsReport_Material".Translate(),
					" (",
					text,
					"): ",
					text2
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + this.GetMultiplier(req).ToStringPercent("F0"));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x001DAB08 File Offset: 0x001D8F08
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef == null) ? 0f : req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null);
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x001DAB50 File Offset: 0x001D8F50
		private float GetMultiplier(StatRequest req)
		{
			float result;
			if (req.HasThing)
			{
				result = req.Thing.GetStatValue(this.multiplierStat, true);
			}
			else
			{
				result = req.Def.GetStatValueAbstract(this.multiplierStat, null);
			}
			return result;
		}
	}
}
