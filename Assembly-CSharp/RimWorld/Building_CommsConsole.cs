using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Building_CommsConsole : Building
	{
		private CompPowerTrader powerComp;

		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare)) && this.powerComp.PowerOn;
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		}

		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = new Job(JobDefOf.UseCommsConsole, (Thing)this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			IEnumerable<FloatMenuOption> result;
			if (!myPawn.CanReach((Thing)this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
			{
				FloatMenuOption item = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item);
				result = list;
			}
			else if (base.Spawned && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
				FloatMenuOption item2 = new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item2);
				result = list;
			}
			else if (!this.powerComp.PowerOn)
			{
				FloatMenuOption item3 = new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item3);
				result = list;
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				FloatMenuOption item4 = new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label)), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item4);
				result = list;
			}
			else if (myPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				FloatMenuOption item5 = new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item5);
				result = list;
			}
			else if (!this.CanUseCommsNow)
			{
				Log.Error(myPawn + " could not use comm console for unknown reason.");
				FloatMenuOption item6 = new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(item6);
				result = list;
			}
			else
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				IEnumerable<ICommunicable> enumerable = myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat(Find.FactionManager.AllFactionsInViewOrder.Cast<ICommunicable>());
				foreach (ICommunicable item7 in enumerable)
				{
					ICommunicable localCommTarget = item7;
					string text = "CallOnRadio".Translate(localCommTarget.GetCallLabel());
					Faction faction = localCommTarget as Faction;
					if (faction != null)
					{
						if (!faction.IsPlayer)
						{
							if (Building_CommsConsole.LeaderIsAvailableToTalk(faction))
							{
								goto IL_0365;
							}
							string str = (faction.leader == null) ? "LeaderUnavailableNoLeader".Translate() : "LeaderUnavailable".Translate(faction.leader.LabelShort);
							list2.Add(new FloatMenuOption(text + " (" + str + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						continue;
					}
					goto IL_0365;
					IL_0365:
					Action action = (Action)delegate()
					{
						ICommunicable commTarget2 = localCommTarget;
						if (item7 is TradeShip && !Building_OrbitalTradeBeacon.AllPowered(base.Map).Any())
						{
							Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), (Thing)this, MessageTypeDefOf.RejectInput);
						}
						else
						{
							Job job = new Job(JobDefOf.UseCommsConsole, (Thing)this);
							job.commTarget = commTarget2;
							myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
						}
					};
					list2.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), myPawn, (Thing)this, "ReservedBy"));
				}
				result = list2;
			}
			return result;
		}

		public static bool LeaderIsAvailableToTalk(Faction fac)
		{
			return (byte)((fac.leader != null) ? ((!fac.leader.Spawned || (!fac.leader.Downed && !fac.leader.IsPrisoner && fac.leader.Awake() && !fac.leader.InMentalState)) ? 1 : 0) : 0) != 0;
		}
	}
}
