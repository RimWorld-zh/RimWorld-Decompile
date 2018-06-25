using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class FactionBase_TraderTracker : Settlement_TraderTracker
	{
		public FactionBase_TraderTracker(Settlement factionBase) : base(factionBase)
		{
		}

		public FactionBase FactionBase
		{
			get
			{
				return (FactionBase)this.settlement;
			}
		}

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
