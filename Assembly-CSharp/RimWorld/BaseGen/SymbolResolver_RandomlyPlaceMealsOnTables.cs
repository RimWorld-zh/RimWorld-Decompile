using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D2 RID: 978
	public class SymbolResolver_RandomlyPlaceMealsOnTables : SymbolResolver
	{
		// Token: 0x060010D8 RID: 4312 RVA: 0x0008F860 File Offset: 0x0008DC60
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef singleThingDef = (rp.faction != null && rp.faction.def.techLevel.IsNeolithicOrWorse()) ? ThingDefOf.Pemmican : ThingDefOf.MealSimple;
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				List<Thing> thingList = iterator.Current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.IsTable && Rand.Chance(0.15f))
					{
						ResolveParams resolveParams = rp;
						resolveParams.rect = CellRect.SingleCell(iterator.Current);
						resolveParams.singleThingDef = singleThingDef;
						BaseGen.symbolStack.Push("thing", resolveParams);
					}
				}
				iterator.MoveNext();
			}
		}
	}
}
