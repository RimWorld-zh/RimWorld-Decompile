using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D1 RID: 977
	public class SymbolResolver_Refuel : SymbolResolver
	{
		// Token: 0x04000A40 RID: 2624
		private static List<CompRefuelable> refuelables = new List<CompRefuelable>();

		// Token: 0x060010D6 RID: 4310 RVA: 0x0008F814 File Offset: 0x0008DC14
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_Refuel.refuelables.Clear();
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				List<Thing> thingList = iterator.Current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					CompRefuelable compRefuelable = thingList[i].TryGetComp<CompRefuelable>();
					if (compRefuelable != null && !SymbolResolver_Refuel.refuelables.Contains(compRefuelable))
					{
						SymbolResolver_Refuel.refuelables.Add(compRefuelable);
					}
				}
				iterator.MoveNext();
			}
			for (int j = 0; j < SymbolResolver_Refuel.refuelables.Count; j++)
			{
				float fuelCapacity = SymbolResolver_Refuel.refuelables[j].Props.fuelCapacity;
				float amount = Rand.Range(fuelCapacity / 2f, fuelCapacity);
				SymbolResolver_Refuel.refuelables[j].Refuel(amount);
			}
			SymbolResolver_Refuel.refuelables.Clear();
		}
	}
}
