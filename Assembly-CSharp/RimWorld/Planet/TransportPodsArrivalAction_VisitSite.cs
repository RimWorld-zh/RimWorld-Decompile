using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public class TransportPodsArrivalAction_VisitSite : TransportPodsArrivalAction
	{
		private Site site;

		private PawnsArrivalModeDef arrivalMode;

		public TransportPodsArrivalAction_VisitSite()
		{
		}

		public TransportPodsArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

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

		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.site.HasMap;
		}

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

		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			FloatMenuAcceptanceReport result;
			if (site == null || !site.Spawned || !site.core.def.transportPodsCanLandAndGenerateMap)
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

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal CompLaunchable representative;

			internal Site site;

			internal IEnumerable<IThingHolder> pods;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private TransportPodsArrivalAction_VisitSite.<GetFloatMenuOptions>c__Iterator0.<GetFloatMenuOptions>c__AnonStorey1 $locvar2;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site), () => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop), "DropAtEdge".Translate(), representative, site.Tile).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_177;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						f = enumerator.Current;
						this.$current = f;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(() => TransportPodsArrivalAction_VisitSite.CanVisit(<GetFloatMenuOptions>c__AnonStorey.pods, <GetFloatMenuOptions>c__AnonStorey.site), () => new TransportPodsArrivalAction_VisitSite(<GetFloatMenuOptions>c__AnonStorey.site, PawnsArrivalModeDefOf.CenterDrop), "DropInCenter".Translate(), representative, <GetFloatMenuOptions>c__AnonStorey.site.Tile).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_177:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f2 = enumerator2.Current;
						this.$current = f2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TransportPodsArrivalAction_VisitSite.<GetFloatMenuOptions>c__Iterator0 <GetFloatMenuOptions>c__Iterator = new TransportPodsArrivalAction_VisitSite.<GetFloatMenuOptions>c__Iterator0();
				<GetFloatMenuOptions>c__Iterator.representative = representative;
				<GetFloatMenuOptions>c__Iterator.site = site;
				<GetFloatMenuOptions>c__Iterator.pods = pods;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey1
			{
				internal IEnumerable<IThingHolder> pods;

				internal Site site;

				public <GetFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal FloatMenuAcceptanceReport <>m__0()
				{
					return TransportPodsArrivalAction_VisitSite.CanVisit(this.pods, this.site);
				}

				internal TransportPodsArrivalAction_VisitSite <>m__1()
				{
					return new TransportPodsArrivalAction_VisitSite(this.site, PawnsArrivalModeDefOf.EdgeDrop);
				}

				internal FloatMenuAcceptanceReport <>m__2()
				{
					return TransportPodsArrivalAction_VisitSite.CanVisit(this.pods, this.site);
				}

				internal TransportPodsArrivalAction_VisitSite <>m__3()
				{
					return new TransportPodsArrivalAction_VisitSite(this.site, PawnsArrivalModeDefOf.CenterDrop);
				}
			}
		}
	}
}
