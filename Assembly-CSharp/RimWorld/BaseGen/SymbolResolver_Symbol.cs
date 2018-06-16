using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B9 RID: 953
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x0600108C RID: 4236 RVA: 0x0008C344 File Offset: 0x0008A744
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

		// Token: 0x0600108D RID: 4237 RVA: 0x0008C3F0 File Offset: 0x0008A7F0
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp);
		}

		// Token: 0x04000A25 RID: 2597
		public string symbol;
	}
}
