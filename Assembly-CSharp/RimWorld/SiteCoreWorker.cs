using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class SiteCoreWorker : SiteCoreOrPartWorkerBase
	{
		public static readonly IntVec3 MapSize = new IntVec3(120, 1, 120);

		public SiteCoreWorker()
		{
		}

		public SiteCoreDef Def
		{
			get
			{
				return (SiteCoreDef)this.def;
			}
		}

		public virtual void SiteCoreWorkerTick(Site site)
		{
		}

		public virtual void VisitAction(Caravan caravan, Site site)
		{
			this.Enter(caravan, site);
		}

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

		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative, Site site)
		{
			foreach (FloatMenuOption f in TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods, site))
			{
				yield return f;
			}
			yield break;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static SiteCoreWorker()
		{
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Site site;

			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__1;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

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
					if (site.HasMap)
					{
						goto IL_CC;
					}
					enumerator = CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, site).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
				IL_CC:
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
				SiteCoreWorker.<GetFloatMenuOptions>c__Iterator0 <GetFloatMenuOptions>c__Iterator = new SiteCoreWorker.<GetFloatMenuOptions>c__Iterator0();
				<GetFloatMenuOptions>c__Iterator.site = site;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator1 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal CompLaunchable representative;

			internal IEnumerable<IThingHolder> pods;

			internal Site site;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__1;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator1()
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
					enumerator = TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods, site).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
				SiteCoreWorker.<GetTransportPodsFloatMenuOptions>c__Iterator1 <GetTransportPodsFloatMenuOptions>c__Iterator = new SiteCoreWorker.<GetTransportPodsFloatMenuOptions>c__Iterator1();
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.site = site;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Enter>c__AnonStorey2
		{
			internal Caravan caravan;

			internal Site site;

			internal SiteCoreWorker $this;

			public <Enter>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.$this.DoEnter(this.caravan, this.site);
			}
		}
	}
}
