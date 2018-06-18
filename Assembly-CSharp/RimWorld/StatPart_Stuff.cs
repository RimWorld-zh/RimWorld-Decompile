using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BA RID: 2490
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x060037B9 RID: 14265 RVA: 0x001DA820 File Offset: 0x001D8C20
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

		// Token: 0x060037BA RID: 14266 RVA: 0x001DA930 File Offset: 0x001D8D30
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef == null) ? 0f : req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null);
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x001DA978 File Offset: 0x001D8D78
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

		// Token: 0x040023C1 RID: 9153
		public StatDef stuffPowerStat;

		// Token: 0x040023C2 RID: 9154
		public StatDef multiplierStat;
	}
}
