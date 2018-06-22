using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D6 RID: 982
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x060010E8 RID: 4328 RVA: 0x00090482 File Offset: 0x0008E882
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp);
		}
	}
}
