using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BC RID: 956
	public class SymbolResolver_ThingSet : SymbolResolver
	{
		// Token: 0x06001093 RID: 4243 RVA: 0x0008C748 File Offset: 0x0008AB48
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingSetMakerDef thingSetMakerDef = rp.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile;
			ThingSetMakerParams? thingSetMakerParams = rp.thingSetMakerParams;
			ThingSetMakerParams parms;
			if (thingSetMakerParams != null)
			{
				parms = rp.thingSetMakerParams.Value;
			}
			else
			{
				int num = rp.rect.Cells.Count((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null);
				parms = default(ThingSetMakerParams);
				parms.countRange = new IntRange?(new IntRange(num, num));
				parms.techLevel = new TechLevel?((rp.faction == null) ? TechLevel.Undefined : rp.faction.def.techLevel);
			}
			List<Thing> list = thingSetMakerDef.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingToSpawn = list[i];
				BaseGen.symbolStack.Push("thing", resolveParams);
			}
		}
	}
}
