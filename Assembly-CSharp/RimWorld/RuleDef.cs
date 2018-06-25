using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CB RID: 715
	public class RuleDef : Def
	{
		// Token: 0x04000704 RID: 1796
		[NoTranslate]
		public string symbol;

		// Token: 0x04000705 RID: 1797
		public List<SymbolResolver> resolvers;
	}
}
