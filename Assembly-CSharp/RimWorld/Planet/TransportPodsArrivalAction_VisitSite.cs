using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000619 RID: 1561
	public class TransportPodsArrivalAction_VisitSite : TransportPodsArrivalAction
	{
		// Token: 0x06001F6D RID: 8045 RVA: 0x00110447 File Offset: 0x0010E847
		public TransportPodsArrivalAction_VisitSite()
		{
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00110450 File Offset: 0x0010E850
		public TransportPodsArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00110467 File Offset: 0x0010E867
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x00110494 File Offset: 0x0010E894
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.site != null && this.site.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = TransportPodsArrivalAction_VisitSite.CanVisit(pods, this.site);
			}
			return result;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x001104F8 File Offset: 0x0010E8F8
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.site.HasMap;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0011051C File Offset: 0x0010E91C
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.site.Tile, SiteCoreWorker.MapSize, null);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), LetterDefOf.NeutralEvent, true, true);
			}
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x001105D0 File Offset: 0x0010E9D0
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			FloatMenuAcceptanceReport result;
			if (site == null || !site.Spawned || !site.core.transportPodsCanLandAndGenerateMap)
			{
				result = false;
			}
			else if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				result = false;
			}
			else if (site.EnterCooldownBlocksEntering())
			{
				result = FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					site.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0011066C File Offset: 0x0010EA6C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Site site)
		{
			foreach (FloatMenuOption f in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site), () => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop), "DropAtEdge".Translate(), representative, site.Tile))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site), () => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.CenterDrop), "DropInCenter".Translate(), representative, site.Tile))
			{
				yield return f2;
			}
			yield break;
		}

		// Token: 0x0400124F RID: 4687
		private Site site;

		// Token: 0x04001250 RID: 4688
		private PawnsArrivalModeDef arrivalMode;
	}
}
