using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompMannable : ThingComp
	{
		private int lastManTick = -1;

		private Pawn lastManPawn;

		public CompMannable()
		{
		}

		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (!pawn.RaceProps.ToolUser)
			{
				yield break;
			}
			if (!pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
			{
				yield break;
			}
			if (this.Props.manWorkType != WorkTags.None && pawn.story != null && pawn.story.WorkTagIsDisabled(this.Props.manWorkType))
			{
				if (this.Props.manWorkType == WorkTags.Violent)
				{
					yield return new FloatMenuOption("CannotManThing".Translate(new object[]
					{
						this.parent.LabelShort
					}) + " (" + "IsIncapableOfViolenceLower".Translate(new object[]
					{
						pawn.LabelShort
					}) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				yield break;
			}
			FloatMenuOption opt = new FloatMenuOption("OrderManThing".Translate(new object[]
			{
				this.parent.LabelShort
			}), delegate()
			{
				Job job = new Job(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield return opt;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <CompFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Pawn pawn;

			internal FloatMenuOption <opt>__0;

			internal CompMannable $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private CompMannable.<CompFloatMenuOptions>c__Iterator0.<CompFloatMenuOptions>c__AnonStorey1 $locvar0;

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
					if (pawn.RaceProps.ToolUser)
					{
						if (pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
						{
							if (base.Props.manWorkType != WorkTags.None && pawn.story != null && pawn.story.WorkTagIsDisabled(base.Props.manWorkType))
							{
								if (base.Props.manWorkType != WorkTags.Violent)
								{
									break;
								}
								this.$current = new FloatMenuOption("CannotManThing".Translate(new object[]
								{
									this.parent.LabelShort
								}) + " (" + "IsIncapableOfViolenceLower".Translate(new object[]
								{
									pawn.LabelShort
								}) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
							}
							else
							{
								opt = new FloatMenuOption("OrderManThing".Translate(new object[]
								{
									this.parent.LabelShort
								}), delegate()
								{
									Job job = new Job(JobDefOf.ManTurret, this.parent);
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.Default, null, null, 0f, null, null);
								this.$current = opt;
								if (!this.$disposing)
								{
									this.$PC = 2;
								}
							}
							return true;
						}
					}
					break;
				case 2u:
					this.$PC = -1;
					break;
				}
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
				CompMannable.<CompFloatMenuOptions>c__Iterator0 <CompFloatMenuOptions>c__Iterator = new CompMannable.<CompFloatMenuOptions>c__Iterator0();
				<CompFloatMenuOptions>c__Iterator.$this = this;
				<CompFloatMenuOptions>c__Iterator.pawn = pawn;
				return <CompFloatMenuOptions>c__Iterator;
			}

			private sealed class <CompFloatMenuOptions>c__AnonStorey1
			{
				internal Pawn pawn;

				internal CompMannable.<CompFloatMenuOptions>c__Iterator0 <>f__ref$0;

				public <CompFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Job job = new Job(JobDefOf.ManTurret, this.<>f__ref$0.$this.parent);
					this.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}
			}
		}
	}
}
