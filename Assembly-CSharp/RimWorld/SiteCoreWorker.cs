using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SiteCoreWorker
	{
		public SiteCoreDef def;

		public static readonly IntVec3 MapSize = new IntVec3(120, 1, 120);

		public virtual void SiteCoreWorkerTick(Site site)
		{
		}

		public virtual void PostMapGenerate(Map map)
		{
		}

		public virtual void VisitAction(Caravan caravan, Site site)
		{
			this.Enter(caravan, site);
		}

		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		public IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			if (!site.HasMap)
			{
				string label = (!site.KnownDanger) ? "VisitSite".Translate(site.Label) : "AttackSite".Translate(site.Label);
				yield return new FloatMenuOption(label, (Action)delegate
				{
					((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_009a: stateMachine*/).caravan.pather.StartPath(((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_009a: stateMachine*/).site.Tile, new CaravanArrivalAction_VisitSite(((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_009a: stateMachine*/).site), true);
				}, MenuOptionPriority.Default, null, null, 0f, null, site);
				if (Prefs.DevMode)
				{
					yield return new FloatMenuOption(label + " (Dev: instantly)", (Action)delegate
					{
						((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_00e8: stateMachine*/).caravan.Tile = ((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_00e8: stateMachine*/).site.Tile;
						((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_00e8: stateMachine*/).caravan.pather.StopDead();
						new CaravanArrivalAction_VisitSite(((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_00e8: stateMachine*/).site).Arrived(((_003CGetFloatMenuOptions_003Ec__Iterator95)/*Error near IL_00e8: stateMachine*/).caravan);
					}, MenuOptionPriority.Default, null, null, 0f, null, site);
				}
			}
		}

		protected void Enter(Caravan caravan, Site site)
		{
			if (!site.HasMap)
			{
				LongEventHandler.QueueLongEvent((Action)delegate()
				{
					this.DoEnter(caravan, site);
				}, "GeneratingMapForNewEncounter", false, null);
			}
			else
			{
				this.DoEnter(caravan, site);
			}
		}

		private void DoEnter(Caravan caravan, Site site)
		{
			Pawn t = caravan.PawnsListForReading[0];
			bool flag = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, SiteCoreWorker.MapSize, null);
			bool draftColonists = flag;
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
			Messages.Message("MessageCaravanArrivedAtDestination".Translate(caravan.Label).CapitalizeFirst(), (Thing)t, MessageSound.Benefit);
		}
	}
}
