using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public abstract class StatWorker_MeleeDamageAmount : StatWorker
	{
		// Token: 0x06003817 RID: 14359 RVA: 0x001DE4C4 File Offset: 0x001DC8C4
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

		// Token: 0x06003818 RID: 14360 RVA: 0x001DE530 File Offset: 0x001DC930
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

		// Token: 0x06003819 RID: 14361
		protected abstract DamageArmorCategoryDef CategoryOfDamage(ThingDef def);
	}
}
