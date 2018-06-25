using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000396 RID: 918
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		// Token: 0x040009F8 RID: 2552
		private const float MaxCoverage = 0.06f;

		// Token: 0x06001001 RID: 4097 RVA: 0x00086E94 File Offset: 0x00085294
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && BaseGen.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.06f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00086F3C File Offset: 0x0008533C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("batteryRoom", rp);
			BaseGen.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}
	}
}
