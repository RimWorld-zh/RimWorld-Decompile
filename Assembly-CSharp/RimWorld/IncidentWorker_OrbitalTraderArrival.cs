using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000334 RID: 820
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		// Token: 0x06000E07 RID: 3591 RVA: 0x00077A34 File Offset: 0x00075E34
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				result = (map.passingShipManager.passingShips.Count < 5);
			}
			return result;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00077A88 File Offset: 0x00075E88
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			if (map.passingShipManager.passingShips.Count >= 5)
			{
				result = false;
			}
			else
			{
				TraderKindDef def;
				if (!(from x in DefDatabase<TraderKindDef>.AllDefs
				where x.orbital
				select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out def))
				{
					throw new InvalidOperationException();
				}
				TradeShip tradeShip = new TradeShip(def);
				if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && b.GetComp<CompPowerTrader>().PowerOn))
				{
					Find.LetterStack.ReceiveLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(new object[]
					{
						tradeShip.name,
						tradeShip.def.label
					}), LetterDefOf.PositiveEvent, null);
				}
				map.passingShipManager.AddShip(tradeShip);
				tradeShip.GenerateThings();
				result = true;
			}
			return result;
		}

		// Token: 0x040008DB RID: 2267
		private const int MaxShips = 5;
	}
}
