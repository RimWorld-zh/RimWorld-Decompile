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
			_003CGetFloatMenuOptions_003Ec__Iterator0 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			if (site.HasMap)
				yield break;
			string label2;
			string label = label2 = ((!site.KnownDanger) ? "VisitSite".Translate(site.Label) : "AttackSite".Translate(site.Label));
			Action action = (Action)delegate()
			{
				caravan.pather.StartPath(site.Tile, new CaravanArrivalAction_VisitSite(site), true);
			};
			Site revalidateWorldClickTarget = site;
			yield return new FloatMenuOption(label2, action, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
			/*Error: Unable to find new state assignment for yield return*/;
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
			Map orGenerateMap;
			Map map = orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, SiteCoreWorker.MapSize, null);
			CaravanEnterMode enterMode = CaravanEnterMode.Edge;
			bool draftColonists = flag;
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, enterMode, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
			Messages.Message("MessageCaravanArrivedAtDestination".Translate(caravan.Label).CapitalizeFirst(), (Thing)t, MessageTypeDefOf.TaskCompletion);
		}
	}
}
