using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BB RID: 955
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x04000A27 RID: 2599
		public string symbol;

		// Token: 0x06001090 RID: 4240 RVA: 0x0008C680 File Offset: 0x0008AA80
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

		// Token: 0x06001091 RID: 4241 RVA: 0x0008C72C File Offset: 0x0008AB2C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp);
		}
	}
}
