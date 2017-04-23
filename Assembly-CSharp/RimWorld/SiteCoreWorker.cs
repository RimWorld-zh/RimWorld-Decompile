using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			SiteCoreWorker.<GetFloatMenuOptions>c__Iterator94 <GetFloatMenuOptions>c__Iterator = new SiteCoreWorker.<GetFloatMenuOptions>c__Iterator94();
			<GetFloatMenuOptions>c__Iterator.site = site;
			<GetFloatMenuOptions>c__Iterator.caravan = caravan;
			<GetFloatMenuOptions>c__Iterator.<$>site = site;
			<GetFloatMenuOptions>c__Iterator.<$>caravan = caravan;
			SiteCoreWorker.<GetFloatMenuOptions>c__Iterator94 expr_23 = <GetFloatMenuOptions>c__Iterator;
			expr_23.$PC = -2;
			return expr_23;
		}

		protected void Enter(Caravan caravan, Site site)
		{
			if (!site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate
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
			Messages.Message("MessageCaravanArrivedAtDestination".Translate(new object[]
			{
				caravan.Label
			}).CapitalizeFirst(), t, MessageSound.Benefit);
		}
	}
}
