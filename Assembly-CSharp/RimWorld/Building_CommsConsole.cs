using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A1 RID: 1697
	public class Building_CommsConsole : Building
	{
		// Token: 0x04001416 RID: 5142
		private CompPowerTrader powerComp;

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0013635C File Offset: 0x0013475C
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare)) && this.powerComp.PowerOn;
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x001363A8 File Offset: 0x001347A8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x001363D8 File Offset: 0x001347D8
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = new Job(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00136418 File Offset: 0x00134818
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			FloatMenuOption result;
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
			{
				result = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (base.Spawned && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
				result = new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!this.powerComp.PowerOn)
			{
				result = new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				result = new FloatMenuOption("CannotUseReason".Translate(new object[]
				{
					"IncapableOfCapacity".Translate(new object[]
					{
						PawnCapacityDefOf.Talking.label
					})
				}), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (myPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				result = new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(new object[]
				{
					SkillDefOf.Social.LabelCap
				}), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!this.CanUseCommsNow)
			{
				Log.Error(myPawn + " could not use comm console for unknown reason.", false);
				result = new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x001365C0 File Offset: 0x001349C0
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat(Find.FactionManager.AllFactionsVisibleInViewOrder.Cast<ICommunicable>());
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x00136600 File Offset: 0x00134A00
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			FloatMenuOption failureReason = this.GetFailureReason(myPawn);
			if (failureReason != null)
			{
				yield return failureReason;
				yield break;
			}
			foreach (ICommunicable commTarget in this.GetCommTargets(myPawn))
			{
				FloatMenuOption option = commTarget.CommFloatMenuOption(this, myPawn);
				if (option != null)
				{
					yield return option;
				}
			}
			yield break;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x00136634 File Offset: 0x00134A34
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = new Job(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}
	}
}
