using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020002C9 RID: 713
	public class RuleDef : Def
	{
		// Token: 0x04000703 RID: 1795
		[NoTranslate]
		public string symbol;

		// Token: 0x04000704 RID: 1796
		public List<SymbolResolver> resolvers;
	}
}
