using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class PawnDiedOrDownedThoughtsUtility
	{
		private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();

		private static List<ThoughtDef> tmpAllColonistsThoughts = new List<ThoughtDef>();

		[CompilerGenerated]
		private static Predicate<IndividualThoughtToAdd> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IndividualThoughtToAdd, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IndividualThoughtToAdd, Pawn> <>f__am$cache2;

		public static void TryGiveThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			try
			{
				if (!PawnGenerator.IsBeingGenerated(victim))
				{
					if (Current.ProgramState == ProgramState.Playing)
					{
						PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
						for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Count; i++)
						{
							PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd[i].Add();
						}
						if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtDef>())
						{
							foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
							{
								if (pawn != victim)
								{
									for (int j = 0; j < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; j++)
									{
										ThoughtDef def = PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[j];
										pawn.needs.mood.thoughts.memories.TryGainMemory(def, null);
									}
								}
							}
						}
						PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Clear();
						PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Clear();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not give thoughts: " + arg, false);
			}
		}

		public static void TryGiveThoughts(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			foreach (Pawn victim in victims)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(victim, null, thoughtsKind);
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
			if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtDef>())
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
			if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any((IndividualThoughtToAdd x) => x.thought.MoodOffset() != 0f))
			{
				if (!individualThoughtsHeader.NullOrEmpty())
				{
					sb.Append(individualThoughtsHeader);
				}
				foreach (IGrouping<Pawn, IndividualThoughtToAdd> grouping in from x in PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd
				where x.thought.MoodOffset() != 0f
				group x by x.addTo)
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string value = grouping.Key.KindLabel.CapitalizeFirst() + " " + grouping.Key.LabelShort;
					sb.Append(value);
					sb.Append(":");
					foreach (IndividualThoughtToAdd individualThoughtToAdd in grouping)
					{
						sb.AppendLine();
						sb.Append("    " + individualThoughtToAdd.LabelCap);
					}
				}
			}
		}

		public static void BuildMoodThoughtsListString(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader, string victimLabelKey)
		{
			foreach (Pawn pawn in victims)
			{
				PawnDiedOrDownedThoughtsUtility.GetThoughts(pawn, null, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
				if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any<IndividualThoughtToAdd>() || PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtDef>())
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string text = pawn.KindLabel.CapitalizeFirst() + " " + pawn.LabelShort;
					if (victimLabelKey.NullOrEmpty())
					{
						sb.Append(text + ":");
					}
					else
					{
						sb.Append(victimLabelKey.Translate(new object[]
						{
							text
						}));
					}
					PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(pawn, null, thoughtsKind, sb, individualThoughtsHeader, allColonistsThoughtsHeader);
				}
			}
		}

		private static void AppendThoughts_ForHumanlike(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtDef> outAllColonistsThoughts)
		{
			bool flag = dinfo != null && dinfo.Value.Def.execution;
			bool flag2 = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
			bool flag3 = dinfo != null && dinfo.Value.Def.ExternalViolenceFor(victim) && dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn;
			if (flag3)
			{
				Pawn pawn = (Pawn)dinfo.Value.Instigator;
				if (!pawn.Dead && pawn.needs.mood != null && pawn.story != null && pawn != victim)
				{
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, pawn, null, 1f, 1f));
					}
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.HostileTo(pawn) && victim.Faction != null && PawnUtility.IsFactionLeader(victim) && victim.Faction.HostileTo(pawn.Faction))
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, pawn, victim, 1f, 1f));
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !flag)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (pawn2 != victim && pawn2.needs.mood != null)
					{
						if (pawn2.MentalStateDef != MentalStateDefOf.SocialFighting || ((MentalState_SocialFighting)pawn2.MentalState).otherPawn != victim)
						{
							if (PawnDiedOrDownedThoughtsUtility.Witnessed(pawn2, victim))
							{
								if (pawn2.Faction == victim.Faction)
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, pawn2, null, 1f, 1f));
								}
								else if (victim.Faction == null || !victim.Faction.HostileTo(pawn2.Faction))
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, pawn2, null, 1f, 1f));
								}
								if (pawn2.relations.FamilyByBlood.Contains(victim))
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, pawn2, null, 1f, 1f));
								}
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, pawn2, null, 1f, 1f));
							}
							else if (victim.Faction == Faction.OfPlayer && victim.Faction == pawn2.Faction && victim.HostFaction != pawn2.Faction)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowColonistDied, pawn2, victim, 1f, 1f));
							}
							if (flag2 && pawn2.Faction == Faction.OfPlayer && !pawn2.IsPrisoner)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowPrisonerDiedInnocent, pawn2, victim, 1f, 1f));
							}
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
					if (!directRelations[i].otherPawn.Dead && directRelations[i].otherPawn.needs.mood != null)
					{
						if (PawnUtility.ShouldGetThoughtAbout(directRelations[i].otherPawn, victim))
						{
							if (directRelations[i].def == PawnRelationDefOf.Bond)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.BondedAnimalBanished, directRelations[i].otherPawn, victim, 1f, 1f));
							}
						}
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died || thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
			{
				foreach (Pawn pawn in victim.relations.PotentiallyRelatedPawns)
				{
					if (!pawn.Dead && pawn.needs.mood != null)
					{
						if (PawnUtility.ShouldGetThoughtAbout(pawn, victim))
						{
							PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(victim);
							if (mostImportantRelation != null)
							{
								ThoughtDef genderSpecificDiedThought = mostImportantRelation.GetGenderSpecificDiedThought(victim);
								if (genderSpecificDiedThought != null)
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, pawn, victim, 1f, 1f));
								}
							}
						}
					}
				}
				if (dinfo != null)
				{
					Pawn pawn2 = dinfo.Value.Instigator as Pawn;
					if (pawn2 != null && pawn2 != victim)
					{
						foreach (Pawn pawn3 in victim.relations.PotentiallyRelatedPawns)
						{
							if (pawn2 != pawn3 && !pawn3.Dead && pawn3.needs.mood != null)
							{
								PawnRelationDef mostImportantRelation2 = pawn3.GetMostImportantRelation(victim);
								if (mostImportantRelation2 != null)
								{
									ThoughtDef genderSpecificKilledThought = mostImportantRelation2.GetGenderSpecificKilledThought(victim);
									if (genderSpecificKilledThought != null)
									{
										outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificKilledThought, pawn3, pawn2, 1f, 1f));
									}
								}
								if (pawn3.RaceProps.IsFlesh)
								{
									int num = pawn3.relations.OpinionOf(victim);
									if (num >= 20)
									{
										ThoughtDef thoughtDef = ThoughtDefOf.KilledMyFriend;
										Pawn pawn4 = pawn3;
										Pawn pawn5 = pawn2;
										float opinionOffsetFactor = victim.relations.GetFriendDiedThoughtPowerFactor(num);
										outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, pawn4, pawn5, 1f, opinionOffsetFactor));
									}
									else if (num <= -20)
									{
										ThoughtDef thoughtDef = ThoughtDefOf.KilledMyRival;
										Pawn pawn5 = pawn3;
										Pawn pawn4 = pawn2;
										float opinionOffsetFactor = victim.relations.GetRivalDiedThoughtPowerFactor(num);
										outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, pawn5, pawn4, 1f, opinionOffsetFactor));
									}
								}
							}
						}
					}
				}
				if (victim.RaceProps.Humanlike)
				{
					foreach (Pawn pawn6 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
					{
						if (!pawn6.Dead && pawn6.RaceProps.IsFlesh && pawn6.needs.mood != null)
						{
							if (PawnUtility.ShouldGetThoughtAbout(pawn6, victim))
							{
								int num2 = pawn6.relations.OpinionOf(victim);
								if (num2 >= 20)
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionDied, pawn6, victim, victim.relations.GetFriendDiedThoughtPowerFactor(num2), 1f));
								}
								else if (num2 <= -20)
								{
									outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionDied, pawn6, victim, victim.relations.GetRivalDiedThoughtPowerFactor(num2), 1f));
								}
							}
						}
					}
				}
			}
		}

		private static bool Witnessed(Pawn p, Pawn victim)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			if (victim.IsCaravanMember())
			{
				return victim.GetCaravan() == p.GetCaravan();
			}
			return victim.Spawned && p.Spawned && p.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, null, 0, 0);
		}

		public static void RemoveDiedThoughts(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs.mood != null && pawn2 != pawn)
				{
					MemoryThoughtHandler memories = pawn2.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowColonistDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowPrisonerDiedInnocent, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionDied, pawn);
					List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						ThoughtDef genderSpecificDiedThought = allDefsListForReading[i].GetGenderSpecificDiedThought(pawn);
						if (genderSpecificDiedThought != null)
						{
							memories.RemoveMemoriesOfDefWhereOtherPawnIs(genderSpecificDiedThought, pawn);
						}
					}
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnDiedOrDownedThoughtsUtility()
		{
		}

		[CompilerGenerated]
		private static bool <BuildMoodThoughtsListString>m__0(IndividualThoughtToAdd x)
		{
			return x.thought.MoodOffset() != 0f;
		}

		[CompilerGenerated]
		private static bool <BuildMoodThoughtsListString>m__1(IndividualThoughtToAdd x)
		{
			return x.thought.MoodOffset() != 0f;
		}

		[CompilerGenerated]
		private static Pawn <BuildMoodThoughtsListString>m__2(IndividualThoughtToAdd x)
		{
			return x.addTo;
		}
	}
}
