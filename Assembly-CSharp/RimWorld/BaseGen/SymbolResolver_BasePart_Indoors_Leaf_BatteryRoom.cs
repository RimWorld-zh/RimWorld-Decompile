using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000394 RID: 916
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		// Token: 0x06000FFE RID: 4094 RVA: 0x00086D34 File Offset: 0x00085134
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && BaseGen.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.06f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00086DDC File Offset: 0x000851DC
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("batteryRoom", rp);
			BaseGen.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040009F5 RID: 2549
		private const float MaxCoverage = 0.06f;
	}
}
