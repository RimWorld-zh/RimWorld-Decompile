using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065C RID: 1628
	public abstract class TaleData : IExposable
	{
		// Token: 0x060021F9 RID: 8697
		public abstract void ExposeData();

		// Token: 0x060021FA RID: 8698 RVA: 0x0011FEF4 File Offset: 0x0011E2F4
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0011FF20 File Offset: 0x0011E320
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
