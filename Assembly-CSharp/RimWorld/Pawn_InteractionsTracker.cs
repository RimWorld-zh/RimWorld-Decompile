using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Pawn_InteractionsTracker : IExposable
	{
		private Pawn pawn;

		private bool wantsRandomInteract;

		private int lastInteractionTime = -9999;

		private const int RandomInteractMTBTicks_Quiet = 22000;

		private const int RandomInteractMTBTicks_Normal = 6600;

		private const int RandomInteractMTBTicks_SuperActive = 550;

		public const int RandomInteractIntervalMin = 320;

		private const int RandomInteractCheckInterval = 60;

		private const int InteractIntervalAbsoluteMin = 120;

		public const int DirectTalkInteractInterval = 320;

		private static List<Pawn> workingList = new List<Pawn>();

		private RandomSocialMode CurrentSocialMode
		{
			get
			{
				if (!InteractionUtility.CanInitiateInteraction(this.pawn))
				{
					return RandomSocialMode.Off;
				}
				RandomSocialMode randomSocialMode = RandomSocialMode.Normal;
				JobDriver curDriver = this.pawn.jobs.curDriver;
				if (curDriver != null)
				{
					randomSocialMode = curDriver.DesiredSocialMode();
				}
				PawnDuty duty = this.pawn.mindState.duty;
				if (duty != null && duty.def.socialModeMax < randomSocialMode)
				{
					randomSocialMode = duty.def.socialModeMax;
				}
				if (this.pawn.Drafted && randomSocialMode > RandomSocialMode.Quiet)
				{
					randomSocialMode = RandomSocialMode.Quiet;
				}
				if (this.pawn.InMentalState && randomSocialMode > this.pawn.MentalState.SocialModeMax())
				{
					randomSocialMode = this.pawn.MentalState.SocialModeMax();
				}
				return randomSocialMode;
			}
		}

		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
		}

		public void InteractionsTrackerTick()
		{
			RandomSocialMode currentSocialMode = this.CurrentSocialMode;
			switch (currentSocialMode)
			{
			case RandomSocialMode.Off:
				this.wantsRandomInteract = false;
				return;
			case RandomSocialMode.Quiet:
				this.wantsRandomInteract = false;
				break;
			}
			if (!this.wantsRandomInteract)
			{
				if (Find.TickManager.TicksGame > this.lastInteractionTime + 320 && this.pawn.IsHashIntervalTick(60))
				{
					int num = 0;
					switch (currentSocialMode)
					{
					case RandomSocialMode.Quiet:
						num = 22000;
						break;
					case RandomSocialMode.Normal:
						num = 6600;
						break;
					case RandomSocialMode.SuperActive:
						num = 550;
						break;
					}
					if (Rand.MTBEventOccurs((float)num, 1f, 60f) && !this.TryInteractRandomly())
					{
						this.wantsRandomInteract = true;
					}
				}
			}
			else if (this.pawn.IsHashIntervalTick(91) && this.TryInteractRandomly())
			{
				this.wantsRandomInteract = false;
			}
		}

		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		public bool CanInteractNowWith(Pawn recipient)
		{
			if (!recipient.Spawned)
			{
				return false;
			}
			if (!InteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient))
			{
				return false;
			}
			if (InteractionUtility.CanInitiateInteraction(this.pawn) && InteractionUtility.CanReceiveInteraction(recipient))
			{
				return true;
			}
			return false;
		}

		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			if (this.pawn == recipient)
			{
				Log.Warning(this.pawn + " tried to interact with self, interaction=" + intDef.defName);
				return false;
			}
			if (!this.CanInteractNowWith(recipient))
			{
				return false;
			}
			if (this.InteractedTooRecentlyToInteract())
			{
				Log.Error(this.pawn + " tried to do interaction " + intDef + " to " + recipient + " only " + (Find.TickManager.TicksGame - this.lastInteractionTime) + " ticks since last interaction (min is " + 120 + ").");
				return false;
			}
			List<RulePackDef> list = new List<RulePackDef>();
			if (intDef.initiatorThought != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(this.pawn, recipient, intDef.initiatorThought);
			}
			if (intDef.recipientThought != null && recipient.needs.mood != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(recipient, this.pawn, intDef.recipientThought);
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				this.pawn.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false);
			}
			if (intDef.recipientXpGainSkill != null && recipient.RaceProps.Humanlike)
			{
				recipient.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false);
			}
			bool flag = false;
			if (recipient.RaceProps.Humanlike)
			{
				flag = recipient.interactions.CheckSocialFightStart(intDef, this.pawn);
			}
			if (!flag)
			{
				intDef.Worker.Interacted(this.pawn, recipient, list);
			}
			MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.Symbol);
			this.lastInteractionTime = Find.TickManager.TicksGame;
			if (flag)
			{
				list.Add(RulePackDefOf.Sentence_SocialFightStarted);
			}
			Find.PlayLog.Add(new PlayLogEntry_Interaction(intDef, this.pawn, recipient, list));
			return true;
		}

		private static void AddInteractionThought(Pawn pawn, Pawn otherPawn, ThoughtDef thoughtDef)
		{
			float statValue = otherPawn.GetStatValue(StatDefOf.SocialImpact, true);
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			thought_Memory.moodPowerFactor = statValue;
			Thought_MemorySocial thought_MemorySocial = thought_Memory as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= statValue;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, otherPawn);
		}

		private bool TryInteractRandomly()
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!InteractionUtility.CanInitiateRandomInteraction(this.pawn))
			{
				return false;
			}
			List<Pawn> collection = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
			Pawn_InteractionsTracker.workingList.Clear();
			Pawn_InteractionsTracker.workingList.AddRange(collection);
			Pawn_InteractionsTracker.workingList.Shuffle();
			List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
			for (int i = 0; i < Pawn_InteractionsTracker.workingList.Count; i++)
			{
				Pawn p = Pawn_InteractionsTracker.workingList[i];
				InteractionDef intDef = default(InteractionDef);
				if (p != this.pawn && this.CanInteractNowWith(p) && InteractionUtility.CanReceiveRandomInteraction(p) && !this.pawn.HostileTo(p) && ((IEnumerable<InteractionDef>)allDefsListForReading).TryRandomElementByWeight<InteractionDef>((Func<InteractionDef, float>)((InteractionDef x) => x.Worker.RandomSelectionWeight(this.pawn, p)), out intDef))
				{
					if (this.TryInteractWith(p, intDef))
					{
						return true;
					}
					Log.Error(this.pawn + " failed to interact with " + p);
				}
			}
			return false;
		}

		public bool CheckSocialFightStart(InteractionDef interaction, Pawn initiator)
		{
			if (this.pawn.needs.mood != null && !TutorSystem.TutorialMode)
			{
				if (!InteractionUtility.HasAnySocialFightProvokingThought(this.pawn, initiator))
				{
					return false;
				}
				if (!DebugSettings.alwaysSocialFight && !(Rand.Value < this.SocialFightChance(interaction, initiator)))
				{
					return false;
				}
				this.StartSocialFight(initiator);
				return true;
			}
			return false;
		}

		public void StartSocialFight(Pawn otherPawn)
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Thought thought = default(Thought);
				if (!InteractionUtility.TryGetRandomSocialFightProvokingThought(this.pawn, otherPawn, out thought))
				{
					Log.Warning("Pawn " + this.pawn + " started a social fight with " + otherPawn + ", but he has no negative opinion thoughts towards " + otherPawn + ".");
				}
				else
				{
					Messages.Message("MessageSocialFight".Translate(this.pawn.LabelShort, otherPawn.LabelShort, thought.LabelCapSocial), this.pawn, MessageTypeDefOf.ThreatSmall);
				}
			}
			MentalStateHandler mentalStateHandler = this.pawn.mindState.mentalStateHandler;
			MentalStateDef socialFighting = MentalStateDefOf.SocialFighting;
			mentalStateHandler.TryStartMentalState(socialFighting, null, false, false, otherPawn);
			MentalStateHandler mentalStateHandler2 = otherPawn.mindState.mentalStateHandler;
			socialFighting = MentalStateDefOf.SocialFighting;
			Pawn otherPawn2 = this.pawn;
			mentalStateHandler2.TryStartMentalState(socialFighting, null, false, false, otherPawn2);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, this.pawn, otherPawn);
		}

		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			if (this.pawn.RaceProps.Humanlike && initiator.RaceProps.Humanlike)
			{
				if (InteractionUtility.HasAnyVerbForSocialFight(this.pawn) && InteractionUtility.HasAnyVerbForSocialFight(initiator))
				{
					if (this.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						return 0f;
					}
					if (!initiator.Downed && !this.pawn.Downed)
					{
						float socialFightBaseChance = interaction.socialFightBaseChance;
						socialFightBaseChance *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
						socialFightBaseChance *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
						List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
						for (int i = 0; i < hediffs.Count; i++)
						{
							if (hediffs[i].CurStage != null)
							{
								socialFightBaseChance *= hediffs[i].CurStage.socialFightChanceFactor;
							}
						}
						float num = (float)this.pawn.relations.OpinionOf(initiator);
						socialFightBaseChance = ((!(num < 0.0)) ? (socialFightBaseChance * GenMath.LerpDouble(0f, 100f, 1f, 0.6f, num)) : (socialFightBaseChance * GenMath.LerpDouble(-100f, 0f, 4f, 1f, num)));
						if (this.pawn.RaceProps.Humanlike)
						{
							List<Trait> allTraits = this.pawn.story.traits.allTraits;
							for (int j = 0; j < allTraits.Count; j++)
							{
								socialFightBaseChance *= allTraits[j].CurrentData.socialFightChanceFactor;
							}
						}
						int num2 = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - initiator.ageTracker.AgeBiologicalYears);
						if (num2 > 10)
						{
							if (num2 > 50)
							{
								num2 = 50;
							}
							socialFightBaseChance *= GenMath.LerpDouble(10f, 50f, 1f, 0.25f, (float)num2);
						}
						return Mathf.Clamp01(socialFightBaseChance);
					}
					return 0f;
				}
				return 0f;
			}
			return 0f;
		}
	}
}
