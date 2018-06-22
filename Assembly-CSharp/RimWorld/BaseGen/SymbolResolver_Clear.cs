using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A6 RID: 934
	public class SymbolResolver_Clear : SymbolResolver
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x000889B8 File Offset: 0x00086DB8
		public override void Resolve(ResolveParams rp)
		{
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				if (rp.clearEdificeOnly != null && rp.clearEdificeOnly.Value)
				{
					Building edifice = iterator.Current.GetEdifice(BaseGen.globalSettings.map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
				else if (rp.clearFillageOnly != null && rp.clearFillageOnly.Value)
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(iterator.Current.GetThingList(BaseGen.globalSettings.map));
					for (int i = 0; i < SymbolResolver_Clear.tmpThingsToDestroy.Count; i++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[i].def.destroyable && SymbolResolver_Clear.tmpThingsToDestroy[i].def.Fillage != FillCategory.None)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				else
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(iterator.Current.GetThingList(BaseGen.globalSettings.map));
					for (int j = 0; j < SymbolResolver_Clear.tmpThingsToDestroy.Count; j++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[j].def.destroyable)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
				if (rp.clearRoof != null && rp.clearRoof.Value)
				{
					BaseGen.globalSettings.map.roofGrid.SetRoof(iterator.Current, null);
				}
				iterator.MoveNext();
			}
		}

		// Token: 0x04000A10 RID: 2576
		private static List<Thing> tmpThingsToDestroy = new List<Thing>();
	}
}
