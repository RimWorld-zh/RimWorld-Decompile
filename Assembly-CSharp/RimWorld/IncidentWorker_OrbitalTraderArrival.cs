using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		private const int MaxShips = 5;

		[CompilerGenerated]
		private static Func<TraderKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<TraderKindDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Building> <>f__am$cache2;

		public IncidentWorker_OrbitalTraderArrival()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return map.passingShipManager.passingShips.Count < 5;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (map.passingShipManager.passingShips.Count >= 5)
			{
				return false;
			}
			TraderKindDef def;
			if ((from x in DefDatabase<TraderKindDef>.AllDefs
			where x.orbital
			select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out def))
			{
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
				return true;
			}
			throw new InvalidOperationException();
		}

		[CompilerGenerated]
		private static bool <TryExecuteWorker>m__0(TraderKindDef x)
		{
			return x.orbital;
		}

		[CompilerGenerated]
		private static float <TryExecuteWorker>m__1(TraderKindDef traderDef)
		{
			return traderDef.CalculatedCommonality;
		}

		[CompilerGenerated]
		private static bool <TryExecuteWorker>m__2(Building b)
		{
			return b.def.IsCommsConsole && b.GetComp<CompPowerTrader>().PowerOn;
		}
	}
}
