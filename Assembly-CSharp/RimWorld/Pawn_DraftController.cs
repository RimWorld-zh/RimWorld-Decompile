using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004E8 RID: 1256
	public class Pawn_DraftController : IExposable
	{
		// Token: 0x06001661 RID: 5729 RVA: 0x000C66F7 File Offset: 0x000C4AF7
		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x000C6724 File Offset: 0x000C4B24
		// (set) Token: 0x06001663 RID: 5731 RVA: 0x000C6740 File Offset: 0x000C4B40
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

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x000C68C8 File Offset: 0x000C4CC8
		// (set) Token: 0x06001665 RID: 5733 RVA: 0x000C68E4 File Offset: 0x000C4CE4
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

		// Token: 0x06001666 RID: 5734 RVA: 0x000C6930 File Offset: 0x000C4D30
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000C6981 File Offset: 0x000C4D81
		public void DraftControllerTick()
		{
			this.autoUndrafter.AutoUndraftTick();
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000C6990 File Offset: 0x000C4D90
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

		// Token: 0x06001669 RID: 5737 RVA: 0x000C69BA File Offset: 0x000C4DBA
		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		// Token: 0x04000D0C RID: 3340
		public Pawn pawn;

		// Token: 0x04000D0D RID: 3341
		private bool draftedInt = false;

		// Token: 0x04000D0E RID: 3342
		private bool fireAtWillInt = true;

		// Token: 0x04000D0F RID: 3343
		private AutoUndrafter autoUndrafter;
	}
}
