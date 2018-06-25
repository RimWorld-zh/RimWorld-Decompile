using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Pawn_InteractionsTracker : IExposable
	{
		private Pawn pawn;

		private bool wantsRandomInteract = false;

		private int lastInteractionTime = -9999;

		private const int RandomInteractMTBTicks_Quiet = 22000;

		private const int RandomInteractMTBTicks_Normal = 6600;

		private const int RandomInteractMTBTicks_SuperActive = 550;

		public const int RandomInteractIntervalMin = 320;

		private const int RandomInteractCheckInterval = 60;

		private const int InteractIntervalAbsoluteMin = 120;

		public const int DirectTalkInteractInterval = 320;

		private static List<Pawn> workingList = new List<Pawn>();

		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		private RandomSocialMode CurrentSocialMode
		{
			get
			{
				RandomSocialMode result;
				if (!InteractionUtility.CanInitiateInteraction(this.pawn))
				{
					result = RandomSocialMode.Off;
				}
				else
				{
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
					result = randomSocialMode;
				}
				return result;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
		}

		public void InteractionsTrackerTick()
		{
			RandomSocialMode currentSocialMode = this.CurrentSocialMode;
			if (currentSocialMode == RandomSocialMode.Off)
			{
				this.wantsRandomInteract = false;
			}
			else
			{
				if (currentSocialMode == RandomSocialMode.Quiet)
				{
					this.wantsRandomInteract = false;
				}
				if (!this.wantsRandomInteract)
				{
					if (Find.TickManager.TicksGame > this.lastInteractionTime + 320 && this.pawn.IsHashIntervalTick(60))
					{
						int num = 0;
						if (currentSocialMode != RandomSocialMode.Quiet)
						{
							if (currentSocialMode != RandomSocialMode.Normal)
							{
								if (currentSocialMode == RandomSocialMode.SuperActive)
								{
									num = 550;
								}
							}
							else
							{
								num = 6600;
							}
						}
						else
						{
							num = 22000;
						}
						if (Rand.MTBEventOccurs((float)num, 1f, 60f))
						{
							if (!this.TryInteractRandomly())
							{
								this.wantsRandomInteract = true;
							}
						}
					}
				}
				else if (this.pawn.IsHashIntervalTick(91))
				{
					if (this.TryInteractRandomly())
					{
						this.wantsRandomInteract = false;
					}
				}
			}
		}

		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		public bool CanInteractNowWith(Pawn recipient)
		{
			return recipient.Spawned && InteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient) && InteractionUtility.CanInitiateInteraction(this.pawn) && InteractionUtility.CanReceiveInteraction(recipient);
		}

		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			bool result;
			if (this.pawn == recipient)
			{
				Log.Warning(this.pawn + " tried to interact with self, interaction=" + intDef.defName, false);
				result = false;
			}
			else if (!this.CanInteractNowWith(recipient))
			{
				result = false;
			}
			else if (this.InteractedTooRecentlyToInteract())
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to do interaction ",
					intDef,
					" to ",
					recipient,
					" only ",
					Find.TickManager.TicksGame - this.lastInteractionTime,
					" ticks since last interaction (min is ",
					120,
					")."
				}), false);
				result = false;
			}
			else
			{
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
				string text;
				string label;
				LetterDef letterDef;
				if (!flag)
				{
					intDef.Worker.Interacted(this.pawn, recipient, list, out text, out label, out letterDef);
				}
				else
				{
					text = null;
					label = null;
					letterDef = null;
				}
				MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.Symbol);
				this.lastInteractionTime = Find.TickManager.TicksGame;
				if (flag)
				{
					list.Add(RulePackDefOf.Sentence_SocialFightStarted);
				}
				PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(intDef, this.pawn, recipient, list);
				Find.PlayLog.Add(playLogEntry_Interaction);
				if (letterDef != null)
				{
					string text2 = playLogEntry_Interaction.ToGameStringFromPOV(this.pawn, false);
					if (!text.NullOrEmpty())
					{
						text2 = text2 + "\n\n" + text;
					}
					Find.LetterStack.ReceiveLetter(label, text2, letterDef, this.pawn, null, null);
				}
				result = true;
			}
			return result;
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
			bool result;
			if (this.InteractedTooRecentlyToInteract())
			{
				result = false;
			}
			else if (!InteractionUtility.CanInitiateRandomInteraction(this.pawn))
			{
				result = false;
			}
			else
			{
				List<Pawn> collection = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
				Pawn_InteractionsTracker.workingList.Clear();
				Pawn_InteractionsTracker.workingList.AddRange(collection);
				Pawn_InteractionsTracker.workingList.Shuffle<Pawn>();
				List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
				for (int i = 0; i < Pawn_InteractionsTracker.workingList.Count; i++)
				{
					Pawn p = Pawn_InteractionsTracker.workingList[i];
					if (p != this.pawn && this.CanInteractNowWith(p) && InteractionUtility.CanReceiveRandomInteraction(p) && !this.pawn.HostileTo(p))
					{
						InteractionDef intDef;
						if (allDefsListForReading.TryRandomElementByWeight((InteractionDef x) => x.Worker.RandomSelectionWeight(this.pawn, p), out intDef))
						{
							if (this.TryInteractWith(p, intDef))
							{
								Pawn_InteractionsTracker.workingList.Clear();
								return true;
							}
							Log.Error(this.pawn + " failed to interact with " + p, false);
						}
					}
				}
				Pawn_InteractionsTracker.workingList.Clear();
				result = false;
			}
			return result;
		}

		public bool CheckSocialFightStart(InteractionDef interaction, Pawn initiator)
		{
			bool result;
			if (!DebugSettings.enableRandomMentalStates)
			{
				result = false;
			}
			else if (this.pawn.needs.mood == null || TutorSystem.TutorialMode)
			{
				result = false;
			}
			else if (!InteractionUtility.HasAnySocialFightProvokingThought(this.pawn, initiator))
			{
				result = false;
			}
			else if (DebugSettings.alwaysSocialFight || Rand.Value < this.SocialFightChance(interaction, initiator))
			{
				this.StartSocialFight(initiator);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void StartSocialFight(Pawn otherPawn)
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Thought thought;
				if (!InteractionUtility.TryGetRandomSocialFightProvokingThought(this.pawn, otherPawn, out thought))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn,
						" started a social fight with ",
						otherPawn,
						", but he has no negative opinion thoughts towards ",
						otherPawn,
						"."
					}), false);
				}
				else
				{
					Messages.Message("MessageSocialFight".Translate(new object[]
					{
						this.pawn.LabelShort,
						otherPawn.LabelShort,
						thought.LabelCapSocial
					}), this.pawn, MessageTypeDefOf.ThreatSmall, true);
				}
			}
			MentalStateHandler mentalStateHandler = this.pawn.mindState.mentalStateHandler;
			MentalStateDef socialFighting = MentalStateDefOf.SocialFighting;
			mentalStateHandler.TryStartMentalState(socialFighting, null, false, false, otherPawn, false);
			MentalStateHandler mentalStateHandler2 = otherPawn.mindState.mentalStateHandler;
			socialFighting = MentalStateDefOf.SocialFighting;
			Pawn otherPawn2 = this.pawn;
			mentalStateHandler2.TryStartMentalState(socialFighting, null, false, false, otherPawn2, false);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				this.pawn,
				otherPawn
			});
		}

		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			float result;
			if (!this.pawn.RaceProps.Humanlike || !initiator.RaceProps.Humanlike)
			{
				result = 0f;
			}
			else if (!InteractionUtility.HasAnyVerbForSocialFight(this.pawn) || !InteractionUtility.HasAnyVerbForSocialFight(initiator))
			{
				result = 0f;
			}
			else if (this.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = 0f;
			}
			else if (initiator.Downed || this.pawn.Downed)
			{
				result = 0f;
			}
			else
			{
				float num = interaction.socialFightBaseChance;
				num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
				num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].CurStage != null)
					{
						num *= hediffs[i].CurStage.socialFightChanceFactor;
					}
				}
				float num2 = (float)this.pawn.relations.OpinionOf(initiator);
				if (num2 < 0f)
				{
					num *= GenMath.LerpDouble(-100f, 0f, 4f, 1f, num2);
				}
				else
				{
					num *= GenMath.LerpDouble(0f, 100f, 1f, 0.6f, num2);
				}
				if (this.pawn.RaceProps.Humanlike)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int j = 0; j < allTraits.Count; j++)
					{
						num *= allTraits[j].CurrentData.socialFightChanceFactor;
					}
				}
				int num3 = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - initiator.ageTracker.AgeBiologicalYears);
				if (num3 > 10)
				{
					if (num3 > 50)
					{
						num3 = 50;
					}
					num *= GenMath.LerpDouble(10f, 50f, 1f, 0.25f, (float)num3);
				}
				result = Mathf.Clamp01(num);
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Pawn_InteractionsTracker()
		{
		}

		[CompilerGenerated]
		private sealed class <TryInteractRandomly>c__AnonStorey0
		{
			internal Pawn p;

			internal Pawn_InteractionsTracker $this;

			public <TryInteractRandomly>c__AnonStorey0()
			{
			}

			internal float <>m__0(InteractionDef x)
			{
				return x.Worker.RandomSelectionWeight(this.$this.pawn, this.p);
			}
		}
	}
}
