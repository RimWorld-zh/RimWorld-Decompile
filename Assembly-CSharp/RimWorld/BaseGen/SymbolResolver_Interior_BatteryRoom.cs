using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D7 RID: 983
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x060010EA RID: 4330 RVA: 0x000902BC File Offset: 0x0008E6BC
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
