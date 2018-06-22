using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FD RID: 1533
	public class FactionBase_TraderTracker : Settlement_TraderTracker
	{
		// Token: 0x06001E89 RID: 7817 RVA: 0x0010B7CC File Offset: 0x00109BCC
		public FactionBase_TraderTracker(Settlement factionBase) : base(factionBase)
		{
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0010B7D8 File Offset: 0x00109BD8
		public FactionBase FactionBase
		{
			get
			{
				return (FactionBase)this.settlement;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x0010B7F8 File Offset: 0x00109BF8
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
