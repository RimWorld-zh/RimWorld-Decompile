using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanArrivalActionUtility
	{
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, int pathDestination, WorldObject revalidateWorldClickTarget) where T : CaravanArrivalAction
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
					Action action = delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
						if (floatMenuAcceptanceReport.Accepted)
						{
							caravan.pather.StartPath(pathDestination, arrivalActionGetter(), true, true);
						}
						else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, false);
						}
					};
					yield return new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
					if (Prefs.DevMode)
					{
						string label2 = label + " (Dev: instantly)";
						action = delegate()
						{
							FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
							if (floatMenuAcceptanceReport.Accepted)
							{
								caravan.Tile = pathDestination;
								caravan.pather.StopDead();
								T t = arrivalActionGetter();
								t.Arrived(caravan);
							}
							else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, false);
							}
						};
						yield return new FloatMenuOption(label2, action, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
					}
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0<T> : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption> where T : CaravanArrivalAction
		{
			internal Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

			internal FloatMenuAcceptanceReport <rep>__0;

			internal string label;

			internal WorldObject revalidateWorldClickTarget;

			internal Caravan caravan;

			internal int pathDestination;

			internal Func<T> arrivalActionGetter;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private CaravanArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T>.<GetFloatMenuOptions>c__AnonStorey1 $locvar0;

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
							return true;
						}
						string text = label;
						Action action = delegate()
						{
							FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
							if (floatMenuAcceptanceReport.Accepted)
							{
								caravan.pather.StartPath(pathDestination, arrivalActionGetter(), true, true);
							}
							else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, false);
							}
						};
						WorldObject worldObject = revalidateWorldClickTarget;
						this.$current = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, worldObject);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					if (Prefs.DevMode)
					{
						string text = label + " (Dev: instantly)";
						Action action = delegate()
						{
							FloatMenuAcceptanceReport floatMenuAcceptanceReport = <GetFloatMenuOptions>c__AnonStorey.acceptanceReportGetter();
							if (floatMenuAcceptanceReport.Accepted)
							{
								<GetFloatMenuOptions>c__AnonStorey.caravan.Tile = <GetFloatMenuOptions>c__AnonStorey.pathDestination;
								<GetFloatMenuOptions>c__AnonStorey.caravan.pather.StopDead();
								T t = <GetFloatMenuOptions>c__AnonStorey.arrivalActionGetter();
								t.Arrived(<GetFloatMenuOptions>c__AnonStorey.caravan);
							}
							else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(<GetFloatMenuOptions>c__AnonStorey.pathDestination), MessageTypeDefOf.RejectInput, false);
							}
						};
						WorldObject worldObject = revalidateWorldClickTarget;
						this.$current = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, worldObject);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					break;
				case 3u:
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
				CaravanArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T> <GetFloatMenuOptions>c__Iterator = new CaravanArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T>();
				<GetFloatMenuOptions>c__Iterator.acceptanceReportGetter = acceptanceReportGetter;
				<GetFloatMenuOptions>c__Iterator.label = label;
				<GetFloatMenuOptions>c__Iterator.revalidateWorldClickTarget = revalidateWorldClickTarget;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				<GetFloatMenuOptions>c__Iterator.pathDestination = pathDestination;
				<GetFloatMenuOptions>c__Iterator.arrivalActionGetter = arrivalActionGetter;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey1
			{
				internal Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

				internal Caravan caravan;

				internal int pathDestination;

				internal Func<T> arrivalActionGetter;

				internal CaravanArrivalActionUtility.<GetFloatMenuOptions>c__Iterator0<T> <>f__ref$0;

				public <GetFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport = this.acceptanceReportGetter();
					if (floatMenuAcceptanceReport.Accepted)
					{
						this.caravan.pather.StartPath(this.pathDestination, this.arrivalActionGetter(), true, true);
					}
					else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(this.pathDestination), MessageTypeDefOf.RejectInput, false);
					}
				}

				internal void <>m__1()
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport = this.acceptanceReportGetter();
					if (floatMenuAcceptanceReport.Accepted)
					{
						this.caravan.Tile = this.pathDestination;
						this.caravan.pather.StopDead();
						T t = this.arrivalActionGetter();
						t.Arrived(this.caravan);
					}
					else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(this.pathDestination), MessageTypeDefOf.RejectInput, false);
					}
				}
			}
		}
	}
}
