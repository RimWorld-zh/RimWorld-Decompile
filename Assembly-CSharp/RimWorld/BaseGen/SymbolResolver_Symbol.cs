using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Symbol : SymbolResolver
	{
		public string symbol;

		public SymbolResolver_Symbol()
		{
		}

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

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp);
		}
	}
}
