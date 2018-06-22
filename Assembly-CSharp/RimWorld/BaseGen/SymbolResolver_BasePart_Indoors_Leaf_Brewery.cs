using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000395 RID: 917
	public class SymbolResolver_BasePart_Indoors_Leaf_Brewery : SymbolResolver
	{
		// Token: 0x06001001 RID: 4097 RVA: 0x00086E34 File Offset: 0x00085234
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && BaseGen.globalSettings.basePart_breweriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.08f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Medieval);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00086EDC File Offset: 0x000852DC
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("brewery", rp);
			BaseGen.globalSettings.basePart_breweriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040009F6 RID: 2550
		private const float MaxCoverage = 0.08f;
	}
}
