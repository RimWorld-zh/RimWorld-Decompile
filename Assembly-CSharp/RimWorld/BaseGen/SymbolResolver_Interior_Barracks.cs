using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D8 RID: 984
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x000905E2 File Offset: 0x0008E9E2
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp);
		}
	}
}
