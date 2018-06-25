using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D8 RID: 984
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x060010EC RID: 4332 RVA: 0x000905D2 File Offset: 0x0008E9D2
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp);
		}
	}
}
