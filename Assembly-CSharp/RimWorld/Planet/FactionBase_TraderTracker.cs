using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000601 RID: 1537
	public class FactionBase_TraderTracker : Settlement_TraderTracker
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x0010B784 File Offset: 0x00109B84
		public FactionBase_TraderTracker(Settlement factionBase) : base(factionBase)
		{
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0010B790 File Offset: 0x00109B90
		public FactionBase FactionBase
		{
			get
			{
				return (FactionBase)this.settlement;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x0010B7B0 File Offset: 0x00109BB0
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
