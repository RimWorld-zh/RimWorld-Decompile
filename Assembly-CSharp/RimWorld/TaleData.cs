using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065C RID: 1628
	public abstract class TaleData : IExposable
	{
		// Token: 0x060021FB RID: 8699
		public abstract void ExposeData();

		// Token: 0x060021FC RID: 8700 RVA: 0x0011FF6C File Offset: 0x0011E36C
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0011FF98 File Offset: 0x0011E398
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
