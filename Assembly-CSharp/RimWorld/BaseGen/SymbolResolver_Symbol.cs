using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BB RID: 955
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x04000A2A RID: 2602
		public string symbol;

		// Token: 0x0600108F RID: 4239 RVA: 0x0008C690 File Offset: 0x0008AA90
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else
			{
				List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					RuleDef ruleDef = allDefsListForReading[i];
					if (!(ruleDef.symbol != this.symbol))
					{
						for (int j = 0; j < ruleDef.resolvers.Count; j++)
						{
							if (ruleDef.resolvers[j].CanResolve(rp))
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0008C73C File Offset: 0x0008AB3C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp);
		}
	}
}
