using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class PawnDiedOrDownedThoughtsUtility
	{
		private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();

		private static List<ThoughtDef> tmpAllColonistsThoughts = new List<ThoughtDef>();

		public static void TryGiveThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			try
			{
				if (!PawnGenerator.IsBeingGenerated(victim) && Current.ProgramState == ProgramState.Playing)
				{
					PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
					for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Count; i++)
					{
						PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd[i].Add();
					}
					if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any())
					{
						foreach (Pawn allMapsCaravansAndTravelingTransportPods_Colonist in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Colonists)
						{
							if (allMapsCaravansAndTravelingTransportPods_Colonist != victim)
							{
								for (int j = 0; j < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; j++)
								{
									ThoughtDef def = PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[j];
									allMapsCaravansAndTravelingTransportPods_Colonist.needs.mood.thoughts.memories.TryGainMemory(def, null);
								}
							}
						}
					}
					PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Clear();
					PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Clear();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not give thoughts: " + arg);
			}
		}

		public static void TryGiveThoughts(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			foreach (Pawn item in victims)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(item, default(DamageInfo?), thoughtsKind);
			}
		}

		public static void GetThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtDef> outAllColonistsThoughts)
		{
			outIndividualThoughts.Clear();
			outAllColonistsThoughts.Clear();
			if (victim.RaceProps.Humanlike)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_ForHumanlike(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
			if (victim.relations != null && victim.relations.everSeenByPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_Relations(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
		}

		public static void BuildMoodThoughtsListString(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader)
		{
			PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
			if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any())
			{
				if (!allColonistsThoughtsHeader.NullOrEmpty())
				{
					sb.Append(allColonistsThoughtsHeader);
					sb.AppendLine();
				}
				for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; i++)
				{
					ThoughtDef thoughtDef = PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[i];
					if (sb.Length > 0)
					{
						sb.AppendLine();
					}
					sb.Append("  - " + thoughtDef.stages[0].label.CapitalizeFirst() + " " + Mathf.RoundToInt(thoughtDef.stages[0].baseMoodEffect).ToStringWithSign());
				}
			}
			if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any((Predicate<IndividualThoughtToAdd>)((IndividualThoughtToAdd x) => x.thought.MoodOffset() != 0.0)))
			{
				if (!individualThoughtsHeader.NullOrEmpty())
				{
					sb.Append(individualThoughtsHeader);
				}
				foreach (IGrouping<Pawn, IndividualThoughtToAdd> item in from x in PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd
				where x.thought.MoodOffset() != 0.0
				group x by x.addTo)
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string value = item.Key.KindLabel.CapitalizeFirst() + " " + item.Key.LabelShort;
					sb.Append(value);
					sb.Append(":");
					foreach (IndividualThoughtToAdd item2 in item)
					{
						sb.AppendLine();
						sb.Append("    " + item2.LabelCap);
					}
				}
			}
		}

		public static void BuildMoodThoughtsListString(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader, string victimLabelKey)
		{
			foreach (Pawn item in victims)
			{
				PawnDiedOrDownedThoughtsUtility.GetThoughts(item, default(DamageInfo?), thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
				if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any() || PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any())
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string text = item.KindLabel.CapitalizeFirst() + " " + item.LabelShort;
					if (victimLabelKey.NullOrEmpty())
					{
						sb.Append(text + ":");
					}
					else
					{
						sb.Append(victimLabelKey.Translate(text));
					}
					PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(item, default(DamageInfo?), thoughtsKind, sb, individualThoughtsHeader, allColonistsThoughtsHeader);
				}
			}
		}

		private static void AppendThoughts_ForHumanlike(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtDef> outAllColonistsThoughts)
		{
			bool flag = dinfo.HasValue && dinfo.Value.Def.execution;
			bool flag2 = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
			if (dinfo.HasValue && dinfo.Value.Def.externalViolence && dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn)
			{
				Pawn pawn = (Pawn)dinfo.Value.Instigator;
				if (!pawn.Dead && pawn.needs.mood != null && pawn.story != null && pawn != victim)
				{
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, pawn, null, 1f, 1f));
					}
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.HostileTo(pawn))
					{
						if (victim.Faction != null && PawnUtility.IsFactionLeader(victim) && victim.Faction.HostileTo(pawn.Faction))
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, pawn, victim, 1f, 1f));
						}
						if (victim.kindDef.combatPower > 250.0)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.DefeatedMajorEnemy, pawn, victim, 1f, 1f));
						}
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !flag)
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (allMapsCaravansAndTravelingTransportPod != victim && allMapsCaravansAndTravelingTransportPod.needs.mood != null && (allMapsCaravansAndTravelingTransportPod.MentalStateDef != MentalStateDefOf.SocialFighting || ((MentalState_SocialFighting)allMapsCaravansAndTravelingTransportPod.MentalState).otherPawn != victim))
					{
						if (PawnDiedOrDownedThoughtsUtility.Witnessed(allMapsCaravansAndTravelingTransportPod, victim))
						{
							if (allMapsCaravansAndTravelingTransportPod.Faction == victim.Faction)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
							}
							else if (victim.Faction == null || !victim.Faction.HostileTo(allMapsCaravansAndTravelingTransportPod.Faction))
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
							}
							if (allMapsCaravansAndTravelingTransportPod.relations.FamilyByBlood.Contains(victim))
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
							}
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
						}
						else if (victim.Faction == Faction.OfPlayer && victim.Faction == allMapsCaravansAndTravelingTransportPod.Faction && victim.HostFaction != allMapsCaravansAndTravelingTransportPod.Faction)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowColonistDied, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
						}
						if (flag2 && allMapsCaravansAndTravelingTransportPod.Faction == Faction.OfPlayer && !allMapsCaravansAndTravelingTransportPod.IsPrisoner)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowPrisonerDiedInnocent, allMapsCaravansAndTravelingTransportPod, null, 1f, 1f));
						}
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.IsColonist)
			{
				outAllColonistsThoughts.Add(ThoughtDefOf.ColonistBanished);
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
			{
				if (victim.IsColonist)
				{
					outAllColonistsThoughts.Add(ThoughtDefOf.ColonistBanishedToDie);
				}
				else if (victim.IsPrisonerOfColony)
				{
					outAllColonistsThoughts.Add(ThoughtDefOf.PrisonerBanishedToDie);
				}
			}
		}

		private static void AppendThoughts_Relations(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtDef> outAllColonistsThoughts)
		{
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.RaceProps.Animal)
			{
				List<DirectPawnRelation> directRelations = victim.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (!directRelations[i].otherPawn.Dead && directRelations[i].otherPawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(directRelations[i].otherPawn, victim) && directRelations[i].def == PawnRelationDefOf.Bond)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.BondedAnimalBanished, directRelations[i].otherPawn, victim, 1f, 1f));
					}
				}
			}
			if (thoughtsKind != 0 && thoughtsKind != PawnDiedOrDownedThoughtsKind.BanishedToDie)
				return;
			foreach (Pawn potentiallyRelatedPawn in victim.relations.PotentiallyRelatedPawns)
			{
				if (!potentiallyRelatedPawn.Dead && potentiallyRelatedPawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(potentiallyRelatedPawn, victim))
				{
					PawnRelationDef mostImportantRelation = potentiallyRelatedPawn.GetMostImportantRelation(victim);
					if (mostImportantRelation != null)
					{
						ThoughtDef genderSpecificDiedThought = mostImportantRelation.GetGenderSpecificDiedThought(victim);
						if (genderSpecificDiedThought != null)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, potentiallyRelatedPawn, victim, 1f, 1f));
						}
					}
				}
			}
			if (dinfo.HasValue)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null && pawn != victim)
				{
					foreach (Pawn potentiallyRelatedPawn2 in victim.relations.PotentiallyRelatedPawns)
					{
						if (pawn != potentiallyRelatedPawn2 && !potentiallyRelatedPawn2.Dead && potentiallyRelatedPawn2.needs.mood != null)
						{
							PawnRelationDef mostImportantRelation2 = potentiallyRelatedPawn2.GetMostImportantRelation(victim);
							if (mostImportantRelation2 != null)
							{
								ThoughtDef genderSpecificKilledThought = mostImportantRelation2.GetGenderSpecificKilledThought(victim);
								if (genderSpecificKilledThought != null)
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificKilledThought, potentiallyRelatedPawn2, pawn, 1f, 1f));
								}
							}
							if (potentiallyRelatedPawn2.RaceProps.IsFlesh)
							{
								int num = potentiallyRelatedPawn2.relations.OpinionOf(victim);
								if (num >= 20)
								{
									ThoughtDef killedMyFriend = ThoughtDefOf.KilledMyFriend;
									Pawn addTo = potentiallyRelatedPawn2;
									Pawn otherPawn = pawn;
									float friendDiedThoughtPowerFactor = victim.relations.GetFriendDiedThoughtPowerFactor(num);
									outIndividualThoughts.Add(new IndividualThoughtToAdd(killedMyFriend, addTo, otherPawn, 1f, friendDiedThoughtPowerFactor));
								}
								else if (num <= -20)
								{
									ThoughtDef killedMyFriend = ThoughtDefOf.KilledMyRival;
									Pawn otherPawn = potentiallyRelatedPawn2;
									Pawn addTo = pawn;
									float friendDiedThoughtPowerFactor = victim.relations.GetRivalDiedThoughtPowerFactor(num);
									outIndividualThoughts.Add(new IndividualThoughtToAdd(killedMyFriend, otherPawn, addTo, 1f, friendDiedThoughtPowerFactor));
								}
							}
						}
					}
				}
			}
			if (victim.RaceProps.Humanlike)
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (!allMapsCaravansAndTravelingTransportPod.Dead && allMapsCaravansAndTravelingTransportPod.RaceProps.IsFlesh && allMapsCaravansAndTravelingTransportPod.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(allMapsCaravansAndTravelingTransportPod, victim))
					{
						int num2 = allMapsCaravansAndTravelingTransportPod.relations.OpinionOf(victim);
						if (num2 >= 20)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionDied, allMapsCaravansAndTravelingTransportPod, victim, victim.relations.GetFriendDiedThoughtPowerFactor(num2), 1f));
						}
						else if (num2 <= -20)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionDied, allMapsCaravansAndTravelingTransportPod, victim, victim.relations.GetRivalDiedThoughtPowerFactor(num2), 1f));
						}
					}
				}
			}
		}

		private static bool Witnessed(Pawn p, Pawn victim)
		{
			return (byte)((p.Awake() && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight)) ? ((!victim.IsCaravanMember()) ? ((victim.Spawned && p.Spawned) ? (p.Position.InHorDistOf(victim.Position, 12f) ? (GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, null, 0, 0) ? 1 : 0) : 0) : 0) : ((victim.GetCaravan() == p.GetCaravan()) ? 1 : 0)) : 0) != 0;
		}
	}
}
