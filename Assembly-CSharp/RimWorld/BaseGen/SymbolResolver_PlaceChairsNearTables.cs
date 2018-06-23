using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CE RID: 974
	public class SymbolResolver_PlaceChairsNearTables : SymbolResolver
	{
		// Token: 0x04000A3C RID: 2620
		private static List<Thing> tables = new List<Thing>();

		// Token: 0x060010CC RID: 4300 RVA: 0x0008F28C File Offset: 0x0008D68C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_PlaceChairsNearTables.tables.Clear();
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				List<Thing> thingList = iterator.Current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.IsTable && !SymbolResolver_PlaceChairsNearTables.tables.Contains(thingList[i]))
					{
						SymbolResolver_PlaceChairsNearTables.tables.Add(thingList[i]);
					}
				}
				iterator.MoveNext();
			}
			for (int j = 0; j < SymbolResolver_PlaceChairsNearTables.tables.Count; j++)
			{
				CellRect cellRect = SymbolResolver_PlaceChairsNearTables.tables[j].OccupiedRect().ExpandedBy(1);
				bool flag = false;
				foreach (IntVec3 c in cellRect.EdgeCells.InRandomOrder(null))
				{
					if (!cellRect.IsCorner(c) && rp.rect.Contains(c))
					{
						if (c.Standable(map) && c.GetEdifice(map) == null)
						{
							if (!flag || !Rand.Bool)
							{
								Rot4 value;
								if (c.x == cellRect.minX)
								{
									value = Rot4.East;
								}
								else if (c.x == cellRect.maxX)
								{
									value = Rot4.West;
								}
								else if (c.z == cellRect.minZ)
								{
									value = Rot4.North;
								}
								else
								{
									value = Rot4.South;
								}
								ResolveParams resolveParams = rp;
								resolveParams.rect = CellRect.SingleCell(c);
								resolveParams.singleThingDef = ThingDefOf.DiningChair;
								resolveParams.singleThingStuff = (rp.singleThingStuff ?? ThingDefOf.WoodLog);
								resolveParams.thingRot = new Rot4?(value);
								BaseGen.symbolStack.Push("thing", resolveParams);
								flag = true;
							}
						}
					}
				}
			}
			SymbolResolver_PlaceChairsNearTables.tables.Clear();
		}
	}
}
