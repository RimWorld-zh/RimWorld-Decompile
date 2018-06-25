using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200012A RID: 298
	public class ItemAvailability
	{
		// Token: 0x04000311 RID: 785
		private Map map;

		// Token: 0x04000312 RID: 786
		private Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();

		// Token: 0x06000625 RID: 1573 RVA: 0x000413D0 File Offset: 0x0003F7D0
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000413EB File Offset: 0x0003F7EB
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000413FC File Offset: 0x0003F7FC
		public bool ThingsAvailableAnywhere(ThingDefCountClass need, Pawn pawn)
		{
			int key = Gen.HashCombine<Faction>(need.GetHashCode(), pawn.Faction);
			bool flag;
			if (!this.cachedResults.TryGetValue(key, out flag))
			{
				List<Thing> list = this.map.listerThings.ThingsOfDef(need.thingDef);
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn))
					{
						num += list[i].stackCount;
						if (num >= need.count)
						{
							break;
						}
					}
				}
				flag = (num >= need.count);
				this.cachedResults.Add(key, flag);
			}
			return flag;
		}
	}
}
