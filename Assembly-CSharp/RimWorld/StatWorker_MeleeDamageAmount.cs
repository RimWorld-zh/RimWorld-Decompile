using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C3 RID: 2499
	public abstract class StatWorker_MeleeDamageAmount : StatWorker
	{
		// Token: 0x06003811 RID: 14353 RVA: 0x001DE69C File Offset: 0x001DCA9C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num = base.GetValueUnfinalized(req, true);
			ThingDef def = (ThingDef)req.Def;
			if (req.StuffDef != null)
			{
				StatDef statDef = null;
				if (this.CategoryOfDamage(def) != null)
				{
					statDef = this.CategoryOfDamage(def).multStat;
				}
				if (statDef != null)
				{
					num *= req.StuffDef.GetStatValueAbstract(statDef, null);
				}
			}
			return num;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x001DE708 File Offset: 0x001DCB08
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			stringBuilder.AppendLine();
			ThingDef def = (ThingDef)req.Def;
			if (req.StuffDef != null)
			{
				StatDef statDef = null;
				if (this.CategoryOfDamage(def) != null)
				{
					statDef = this.CategoryOfDamage(def).multStat;
				}
				if (statDef != null)
				{
					stringBuilder.AppendLine(req.StuffDef.LabelCap + ": x" + req.StuffDef.GetStatValueAbstract(statDef, null).ToStringPercent());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003813 RID: 14355
		protected abstract DamageArmorCategoryDef CategoryOfDamage(ThingDef def);
	}
}
