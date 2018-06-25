using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D1 RID: 721
	public class SiteCoreWorker : SiteWorkerBase
	{
		// Token: 0x04000723 RID: 1827
		public static readonly IntVec3 MapSize = new IntVec3(120, 1, 120);

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0006A028 File Offset: 0x00068428
		public SiteCoreDef Def
		{
			get
			{
				return (SiteCoreDef)this.def;
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0006A048 File Offset: 0x00068448
		public virtual void SiteCoreWorkerTick(Site site)
		{
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0006A04B File Offset: 0x0006844B
		public virtual void VisitAction(Caravan caravan, Site site)
		{
			this.Enter(caravan, site);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0006A058 File Offset: 0x00068458
		public IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			if (!site.HasMap)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, site))
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0006A08C File Offset: 0x0006848C
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative, Site site)
		{
			foreach (FloatMenuOption f in TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods, site))
			{
				yield return f;
			}
			yield break;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0006A0C4 File Offset: 0x000684C4
		protected void Enter(Caravan caravan, Site site)
		{
			if (!site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoEnter(caravan, site);
				}, "GeneratingMapForNewEncounter", false, null);
			}
			else
			{
				this.DoEnter(caravan, site);
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0006A12C File Offset: 0x0006852C
		private void DoEnter(Caravan caravan, Site site)
		{
			Pawn t = caravan.PawnsListForReading[0];
			bool flag = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag2 = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, SiteCoreWorker.MapSize, null);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
			Messages.Message("MessageCaravanArrivedAtDestination".Translate(new object[]
			{
				caravan.Label
			}).CapitalizeFirst(), t, MessageTypeDefOf.TaskCompletion, true);
			if (flag2)
			{
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsSite".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), LetterDefOf.NeutralEvent, true, true);
			}
			Map map = orGenerateMap;
			CaravanEnterMode enterMode = CaravanEnterMode.Edge;
			bool draftColonists = flag;
			CaravanEnterMapUtility.Enter(caravan, map, enterMode, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
		}
	}
}
