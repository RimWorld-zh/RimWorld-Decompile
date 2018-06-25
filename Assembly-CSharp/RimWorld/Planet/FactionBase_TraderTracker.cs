using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FF RID: 1535
	public class FactionBase_TraderTracker : Settlement_TraderTracker
	{
		// Token: 0x06001E8C RID: 7820 RVA: 0x0010BB84 File Offset: 0x00109F84
		public FactionBase_TraderTracker(Settlement factionBase) : base(factionBase)
		{
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x0010BB90 File Offset: 0x00109F90
		public FactionBase FactionBase
		{
			get
			{
				return (FactionBase)this.settlement;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0010BBB0 File Offset: 0x00109FB0
		public override TraderKindDef TraderKind
		{
			get
			{
				FactionBase factionBase = this.FactionBase;
				List<TraderKindDef> baseTraderKinds = factionBase.Faction.def.baseTraderKinds;
				TraderKindDef result;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					result = null;
				}
				else
				{
					int index = Mathf.Abs(factionBase.HashOffset()) % baseTraderKinds.Count;
					result = baseTraderKinds[index];
				}
				return result;
			}
		}
	}
}
