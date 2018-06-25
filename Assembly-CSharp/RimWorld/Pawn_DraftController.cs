using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Pawn_DraftController : IExposable
	{
		public Pawn pawn;

		private bool draftedInt = false;

		private bool fireAtWillInt = true;

		private AutoUndrafter autoUndrafter;

		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		public bool Drafted
		{
			get
			{
				return this.draftedInt;
			}
			set
			{
				if (value != this.draftedInt)
				{
					this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
					this.fireAtWillInt = true;
					this.draftedInt = value;
					if (!value && this.pawn.Spawned)
					{
						this.pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this.pawn);
					}
					this.pawn.jobs.ClearQueuedJobs();
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
					if (this.draftedInt)
					{
						Lord lord = this.pawn.GetLord();
						if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
						{
							lord.Notify_PawnLost(this.pawn, PawnLostCondition.Drafted);
						}
						this.autoUndrafter.Notify_Drafted();
					}
					else if (this.pawn.playerSettings != null)
					{
						this.pawn.playerSettings.animalsReleased = false;
					}
					foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
					{
						pawn.jobs.Notify_MasterDraftedOrUndrafted();
					}
				}
			}
		}

		public bool FireAtWill
		{
			get
			{
				return this.fireAtWillInt;
			}
			set
			{
				this.fireAtWillInt = value;
				if (!this.fireAtWillInt)
				{
					if (this.pawn.stances.curStance is Stance_Warmup)
					{
						this.pawn.stances.CancelBusyStanceSoft();
					}
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		public void DraftControllerTick()
		{
			this.autoUndrafter.AutoUndraftTick();
		}

		internal IEnumerable<Gizmo> GetGizmos()
		{
			Command_Toggle draft = new Command_Toggle();
			draft.hotKey = KeyBindingDefOf.Command_ColonistDraft;
			draft.isActive = (() => this.Drafted);
			draft.toggleAction = delegate()
			{
				this.Drafted = !this.Drafted;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
				if (this.Drafted)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
				}
			};
			draft.defaultDesc = "CommandToggleDraftDesc".Translate();
			draft.icon = TexCommand.Draft;
			draft.turnOnSound = SoundDefOf.DraftOn;
			draft.turnOffSound = SoundDefOf.DraftOff;
			if (!this.Drafted)
			{
				draft.defaultLabel = "CommandDraftLabel".Translate();
			}
			if (this.pawn.Downed)
			{
				draft.Disable("IsIncapped".Translate(new object[]
				{
					this.pawn.LabelShort
				}));
			}
			if (!this.Drafted)
			{
				draft.tutorTag = "Draft";
			}
			else
			{
				draft.tutorTag = "Undraft";
			}
			yield return draft;
			if (this.Drafted && this.pawn.equipment.Primary != null && this.pawn.equipment.Primary.def.IsRangedWeapon)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Misc6,
					isActive = (() => this.FireAtWill),
					toggleAction = delegate()
					{
						this.FireAtWill = !this.FireAtWill;
					},
					icon = TexCommand.FireAtWill,
					defaultLabel = "CommandFireAtWillLabel".Translate(),
					defaultDesc = "CommandFireAtWillDesc".Translate(),
					tutorTag = "FireAtWillToggle"
				};
			}
			yield break;
		}

		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Toggle <draft>__1;

			internal Command_Toggle <toggleFireAtWill>__2;

			internal Pawn_DraftController $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					draft = new Command_Toggle();
					draft.hotKey = KeyBindingDefOf.Command_ColonistDraft;
					draft.isActive = (() => base.Drafted);
					draft.toggleAction = delegate()
					{
						base.Drafted = !base.Drafted;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
						if (base.Drafted)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
						}
					};
					draft.defaultDesc = "CommandToggleDraftDesc".Translate();
					draft.icon = TexCommand.Draft;
					draft.turnOnSound = SoundDefOf.DraftOn;
					draft.turnOffSound = SoundDefOf.DraftOff;
					if (!base.Drafted)
					{
						draft.defaultLabel = "CommandDraftLabel".Translate();
					}
					if (this.pawn.Downed)
					{
						draft.Disable("IsIncapped".Translate(new object[]
						{
							this.pawn.LabelShort
						}));
					}
					if (!base.Drafted)
					{
						draft.tutorTag = "Draft";
					}
					else
					{
						draft.tutorTag = "Undraft";
					}
					this.$current = draft;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (base.Drafted && this.pawn.equipment.Primary != null && this.pawn.equipment.Primary.def.IsRangedWeapon)
					{
						Command_Toggle toggleFireAtWill = new Command_Toggle();
						toggleFireAtWill.hotKey = KeyBindingDefOf.Misc6;
						toggleFireAtWill.isActive = (() => base.FireAtWill);
						toggleFireAtWill.toggleAction = delegate()
						{
							base.FireAtWill = !base.FireAtWill;
						};
						toggleFireAtWill.icon = TexCommand.FireAtWill;
						toggleFireAtWill.defaultLabel = "CommandFireAtWillLabel".Translate();
						toggleFireAtWill.defaultDesc = "CommandFireAtWillDesc".Translate();
						toggleFireAtWill.tutorTag = "FireAtWillToggle";
						this.$current = toggleFireAtWill;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn_DraftController.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Pawn_DraftController.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.Drafted;
			}

			internal void <>m__1()
			{
				base.Drafted = !base.Drafted;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
				if (base.Drafted)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
				}
			}

			internal bool <>m__2()
			{
				return base.FireAtWill;
			}

			internal void <>m__3()
			{
				base.FireAtWill = !base.FireAtWill;
			}
		}
	}
}
