using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CB RID: 715
	public class RuleDef : Def
	{
		// Token: 0x04000702 RID: 1794
		[NoTranslate]
		public string symbol;

		// Token: 0x04000703 RID: 1795
		public List<SymbolResolver> resolvers;
	}
}
