using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Symbol : SymbolResolver
	{
		public string symbol;

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
								goto IL_0066;
						}
					}
				}
				result = false;
			}
			goto IL_009e;
			IL_009e:
			return result;
			IL_0066:
			result = true;
			goto IL_009e;
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp);
		}
	}
}
