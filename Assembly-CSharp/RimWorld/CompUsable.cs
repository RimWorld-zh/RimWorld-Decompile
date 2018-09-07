using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompUsable : ThingComp
	{
		[CompilerGenerated]
		private static Func<CompUseEffect, float> <>f__am$cache0;

		public CompUsable()
		{
		}

		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		protected virtual string FloatMenuOptionLabel
		{
			get
			{
				return this.Props.useLabel;
			}
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			string failReason;
			if (!this.CanBeUsedBy(myPawn, out failReason))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + ((failReason == null) ? string.Empty : (" (" + failReason + ")")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				FloatMenuOption useopt = new FloatMenuOption(this.FloatMenuOptionLabel, delegate()
				{
					if (myPawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
						{
							if (compUseEffect.SelectedUseOption(myPawn))
							{
								return;
							}
						}
						this.TryStartUseJob(myPawn);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return useopt;
			}
			yield break;
		}

		public void TryStartUseJob(Pawn user)
		{
			if (!user.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return;
			}
			string text;
			if (!this.CanBeUsedBy(user, out text))
			{
				return;
			}
			Job job = new Job(this.Props.useJob, this.parent);
			user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		public void UsedBy(Pawn p)
		{
			string text;
			if (!this.CanBeUsedBy(p, out text))
			{
				return;
			}
			foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					compUseEffect.DoEffect(p);
				}
				catch (Exception arg)
				{
					Log.Error("Error in CompUseEffect: " + arg, false);
				}
			}
		}

		private bool CanBeUsedBy(Pawn p, out string failReason)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
				if (compUseEffect != null && !compUseEffect.CanBeUsedBy(p, out failReason))
				{
					return false;
				}
			}
			failReason = null;
			return true;
		}

		[CompilerGenerated]
		private static float <UsedBy>m__0(CompUseEffect x)
		{
			return x.OrderPriority;
		}

		[CompilerGenerated]
		private sealed class <CompFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Pawn myPawn;

			internal string <failReason>__0;

			internal FloatMenuOption <useopt>__1;

			internal CompUsable $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private CompUsable.<CompFloatMenuOptions>c__Iterator0.<CompFloatMenuOptions>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <CompFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (!base.CanBeUsedBy(myPawn, out failReason))
					{
						this.$current = new FloatMenuOption(this.FloatMenuOptionLabel + ((failReason == null) ? string.Empty : (" (" + failReason + ")")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
					}
					else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						this.$current = new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
					}
					else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
					{
						this.$current = new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
					}
					else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						this.$current = new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
					}
					else
					{
						useopt = new FloatMenuOption(this.FloatMenuOptionLabel, delegate()
						{
							if (myPawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
							{
								foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
								{
									if (compUseEffect.SelectedUseOption(myPawn))
									{
										return;
									}
								}
								this.TryStartUseJob(myPawn);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
						this.$current = useopt;
						if (!this.$disposing)
						{
							this.$PC = 5;
						}
					}
					return true;
				case 1u:
					break;
				case 2u:
					break;
				case 3u:
					break;
				case 4u:
					break;
				case 5u:
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
				CompUsable.<CompFloatMenuOptions>c__Iterator0 <CompFloatMenuOptions>c__Iterator = new CompUsable.<CompFloatMenuOptions>c__Iterator0();
				<CompFloatMenuOptions>c__Iterator.$this = this;
				<CompFloatMenuOptions>c__Iterator.myPawn = myPawn;
				return <CompFloatMenuOptions>c__Iterator;
			}

			private sealed class <CompFloatMenuOptions>c__AnonStorey1
			{
				internal Pawn myPawn;

				internal CompUsable.<CompFloatMenuOptions>c__Iterator0 <>f__ref$0;

				public <CompFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					if (this.myPawn.CanReserveAndReach(this.<>f__ref$0.$this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						foreach (CompUseEffect compUseEffect in this.<>f__ref$0.$this.parent.GetComps<CompUseEffect>())
						{
							if (compUseEffect.SelectedUseOption(this.myPawn))
							{
								return;
							}
						}
						this.<>f__ref$0.$this.TryStartUseJob(this.myPawn);
					}
				}
			}
		}
	}
}
