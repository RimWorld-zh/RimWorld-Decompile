using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000658 RID: 1624
	public abstract class TaleData : IExposable
	{
		// Token: 0x060021F3 RID: 8691
		public abstract void ExposeData();

		// Token: 0x060021F4 RID: 8692 RVA: 0x0012006C File Offset: 0x0011E46C
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00120098 File Offset: 0x0011E498
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
