using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065A RID: 1626
	public abstract class TaleData : IExposable
	{
		// Token: 0x060021F7 RID: 8695
		public abstract void ExposeData();

		// Token: 0x060021F8 RID: 8696 RVA: 0x001201BC File Offset: 0x0011E5BC
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x001201E8 File Offset: 0x0011E5E8
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
