using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_ThingSet : SymbolResolver
	{
		public SymbolResolver_ThingSet()
		{
		}

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

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey0
		{
			internal Map map;

			public <Resolve>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && x.GetFirstItem(this.map) == null;
			}
		}
	}
}
