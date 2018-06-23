using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C0 RID: 960
	public class SymbolResolver_ChargeBatteries : SymbolResolver
	{
		// Token: 0x04000A2D RID: 2605
		private static List<CompPowerBattery> batteries = new List<CompPowerBattery>();

		// Token: 0x0600109D RID: 4253 RVA: 0x0008CDD4 File Offset: 0x0008B1D4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_ChargeBatteries.batteries.Clear();
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				List<Thing> thingList = iterator.Current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					CompPowerBattery compPowerBattery = thingList[i].TryGetComp<CompPowerBattery>();
					if (compPowerBattery != null && !SymbolResolver_ChargeBatteries.batteries.Contains(compPowerBattery))
					{
						SymbolResolver_ChargeBatteries.batteries.Add(compPowerBattery);
					}
				}
				iterator.MoveNext();
			}
			for (int j = 0; j < SymbolResolver_ChargeBatteries.batteries.Count; j++)
			{
				float num = Rand.Range(0.1f, 0.3f);
				SymbolResolver_ChargeBatteries.batteries[j].SetStoredEnergyPct(Mathf.Min(SymbolResolver_ChargeBatteries.batteries[j].StoredEnergyPct + num, 1f));
			}
			SymbolResolver_ChargeBatteries.batteries.Clear();
		}
	}
}
