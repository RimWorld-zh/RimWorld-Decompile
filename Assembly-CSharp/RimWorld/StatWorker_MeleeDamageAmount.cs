using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public abstract class StatWorker_MeleeDamageAmount : StatWorker
	{
		// Token: 0x06003815 RID: 14357 RVA: 0x001DE7E0 File Offset: 0x001DCBE0
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

		// Token: 0x06003816 RID: 14358 RVA: 0x001DE84C File Offset: 0x001DCC4C
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

		// Token: 0x06003817 RID: 14359
		protected abstract DamageArmorCategoryDef CategoryOfDamage(ThingDef def);
	}
}
