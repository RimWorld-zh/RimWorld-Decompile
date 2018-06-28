using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Settlement_TraderTracker : SettlementBase_TraderTracker
	{
		public Settlement_TraderTracker(SettlementBase settlement) : base(settlement)
		{
		}

		public Settlement Settlement
		{
			get
			{
				return (Settlement)this.settlement;
			}
		}

		public override TraderKindDef TraderKind
		{
			get
			{
				Settlement settlement = this.Settlement;
				List<TraderKindDef> baseTraderKinds = settlement.Faction.def.baseTraderKinds;
				TraderKindDef result;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					result = null;
				}
				else
				{
					int index = Mathf.Abs(settlement.HashOffset()) % baseTraderKinds.Count;
					result = baseTraderKinds[index];
				}
				return result;
			}
		}
	}
}
