using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000396 RID: 918
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		// Token: 0x040009F5 RID: 2549
		private const float MaxCoverage = 0.06f;

		// Token: 0x06001002 RID: 4098 RVA: 0x00086E84 File Offset: 0x00085284
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && BaseGen.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.06f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00086F2C File Offset: 0x0008532C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("batteryRoom", rp);
			BaseGen.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}
	}
}
