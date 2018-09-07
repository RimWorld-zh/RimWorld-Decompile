using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public static class TransportPodsArrivalActionUtility
	{
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, CompLaunchable representative, int destinationTile) where T : TransportPodsArrivalAction
		{
			FloatMenuAcceptanceReport rep = acceptanceReportGetter();
			if (rep.Accepted || !rep.FailReason.NullOrEmpty() || !rep.FailMessage.NullOrEmpty())
			{
				if (!rep.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + rep.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					yield return new FloatMenuOption(label, delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
						if (floatMenuAcceptanceReport.Accepted)
						{
							representative.TryLaunch(destinationTile, arrivalActionGetter());
						}
						else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
			}
			yield break;
		}

		public static bool AnyNonDownedColonist(IEnumerable<IThingHolder> pods)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool AnyPotentialCaravanOwner(IEnumerable<IThingHolder> pods, Faction faction)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && CaravanUtility.IsOwner(pawn, faction))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static Thing GetLookTarget(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner directlyHeldThings = pods[i].GetDirectlyHeldThings();
				for (int j = 0; j < directlyHeldThings.Count; j++)
				{
					Pawn pawn = directlyHeldThings[j] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						return pawn;
					}
				}
			}
			for (int k = 0; k < pods.Count; k++)
			{
				Thing thing = pods[k].GetDirectlyHeldThings().FirstOrDefault<Thing>();
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		public static void DropTravelingTransportPods(List<ActiveDropPodInfo> dropPods, IntVec3 near, Map map)
		{
			TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(dropPods);
			for (int i = 0; i < dropPods.Count; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(near, map, out c, false, true);
				DropPodUtility.MakeDropPodAt(c, map, dropPods[i]);
			}
		}

		public static void RemovePawnsFromWorldPawns(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0<T> : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption> where T : TransportPodsArrivalAction
		{
			internal Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

			internal FloatMenuAcceptanceReport <rep>__0;

			internal string label;

			internal CompLaunchable representative;

			internal int destinationTile;

			internal Func<T> arrivalActionGetter;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private TransportPodsArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T>.<GetFloatMenuOptions>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					rep = acceptanceReportGetter();
					if (rep.Accepted || !rep.FailReason.NullOrEmpty() || !rep.FailMessage.NullOrEmpty())
					{
						if (!rep.FailReason.NullOrEmpty())
						{
							this.$current = new FloatMenuOption(label + " (" + rep.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
						}
						else
						{
							this.$current = new FloatMenuOption(label, delegate()
							{
								FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
								if (floatMenuAcceptanceReport.Accepted)
								{
									representative.TryLaunch(destinationTile, arrivalActionGetter());
								}
								else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
								{
									Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
								}
							}, MenuOptionPriority.Default, null, null, 0f, null, null);
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					break;
				default:
					return false;
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
				this.$disposing = true;
				this.$PC = -1;
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
				TransportPodsArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T> <GetFloatMenuOptions>c__Iterator = new TransportPodsArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T>();
				<GetFloatMenuOptions>c__Iterator.acceptanceReportGetter = acceptanceReportGetter;
				<GetFloatMenuOptions>c__Iterator.label = label;
				<GetFloatMenuOptions>c__Iterator.representative = representative;
				<GetFloatMenuOptions>c__Iterator.destinationTile = destinationTile;
				<GetFloatMenuOptions>c__Iterator.arrivalActionGetter = arrivalActionGetter;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey1
			{
				internal Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

				internal CompLaunchable representative;

				internal int destinationTile;

				internal Func<T> arrivalActionGetter;

				internal TransportPodsArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T> <>f__ref$0;

				public <GetFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport = this.acceptanceReportGetter();
					if (floatMenuAcceptanceReport.Accepted)
					{
						this.representative.TryLaunch(this.destinationTile, this.arrivalActionGetter());
					}
					else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.RejectInput, false);
					}
				}
			}
		}
	}
}
