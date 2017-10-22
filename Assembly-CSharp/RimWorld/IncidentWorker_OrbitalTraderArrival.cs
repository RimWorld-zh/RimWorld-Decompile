using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (map.passingShipManager.passingShips.Count >= 5)
			{
				return false;
			}
			TraderKindDef def = default(TraderKindDef);
			if ((from x in DefDatabase<TraderKindDef>.AllDefs
			where x.orbital
			select x).TryRandomElementByWeight<TraderKindDef>((Func<TraderKindDef, float>)((TraderKindDef traderDef) => traderDef.commonality), out def))
			{
				TradeShip tradeShip = new TradeShip(def);
				if (map.listerBuildings.allBuildingsColonist.Any((Predicate<Building>)((Building b) => b.def.IsCommsConsole && b.GetComp<CompPowerTrader>().PowerOn)))
				{
					Find.LetterStack.ReceiveLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(tradeShip.name, tradeShip.def.label), LetterDefOf.Good, (string)null);
				}
				map.passingShipManager.AddShip(tradeShip);
				tradeShip.GenerateThings();
				return true;
			}
			throw new InvalidOperationException();
		}
	}
}
