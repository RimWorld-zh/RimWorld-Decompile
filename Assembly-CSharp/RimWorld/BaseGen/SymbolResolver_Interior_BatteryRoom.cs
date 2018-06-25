using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D9 RID: 985
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x00090608 File Offset: 0x0008EA08
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp);
			BaseGen.symbolStack.Push("chargeBatteries", rp);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?((!Rand.Bool) ? Rot4.East : Rot4.North);
			int? fillWithThingsPadding = rp.fillWithThingsPadding;
			resolveParams.fillWithThingsPadding = new int?((fillWithThingsPadding == null) ? 1 : fillWithThingsPadding.Value);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams);
		}
	}
}
