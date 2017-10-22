using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class RelationsUtility
	{
		public static bool PawnsKnowEachOther(Pawn p1, Pawn p2)
		{
			return (byte)((p1.Faction != null && p1.Faction == p2.Faction) ? 1 : ((p1.RaceProps.IsFlesh && p1.relations.DirectRelations.Find((Predicate<DirectPawnRelation>)((DirectPawnRelation x) => x.otherPawn == p2)) != null) ? 1 : ((p2.RaceProps.IsFlesh && p2.relations.DirectRelations.Find((Predicate<DirectPawnRelation>)((DirectPawnRelation x) => x.otherPawn == p1)) != null) ? 1 : (RelationsUtility.HasAnySocialMemoryWith(p1, p2) ? 1 : (RelationsUtility.HasAnySocialMemoryWith(p2, p1) ? 1 : 0))))) != 0;
		}

		public static bool IsDisfigured(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					if (hediffs[num].Part != null && hediffs[num].Part.def.beautyRelated && (hediffs[num] is Hediff_MissingPart || hediffs[num] is Hediff_Injury))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool TryDevelopBondRelation(Pawn humanlike, Pawn animal, float baseChance)
		{
			bool result;
			if (!animal.RaceProps.Animal)
			{
				result = false;
			}
			else if (animal.RaceProps.TrainableIntelligence.intelligenceOrder < TrainableIntelligenceDefOf.Intermediate.intelligenceOrder)
			{
				result = false;
			}
			else if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				result = false;
			}
			else if (animal.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Predicate<Pawn>)((Pawn x) => x.Spawned)) != null)
			{
				result = false;
			}
			else
			{
				int num = 0;
				List<DirectPawnRelation> directRelations = animal.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (directRelations[i].def == PawnRelationDefOf.Bond && !directRelations[i].otherPawn.Dead)
					{
						num++;
					}
				}
				int num2 = 0;
				List<DirectPawnRelation> directRelations2 = humanlike.relations.DirectRelations;
				for (int j = 0; j < directRelations2.Count; j++)
				{
					if (directRelations2[j].def == PawnRelationDefOf.Bond && !directRelations2[j].otherPawn.Dead)
					{
						num2++;
					}
				}
				if (num > 0)
				{
					baseChance *= Mathf.Pow(0.2f, (float)num);
				}
				if (num2 > 0)
				{
					baseChance *= Mathf.Pow(0.55f, (float)num2);
				}
				if (Rand.Value < baseChance)
				{
					if (!humanlike.story.traits.HasTrait(TraitDefOf.Psychopath))
					{
						humanlike.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
					}
					if (humanlike.Faction == Faction.OfPlayer || animal.Faction == Faction.OfPlayer)
					{
						TaleRecorder.RecordTale(TaleDefOf.BondedWithAnimal, humanlike, animal);
					}
					bool flag = false;
					string text = (string)null;
					if (animal.Name == null || animal.Name.Numerical)
					{
						flag = true;
						text = ((animal.Name != null) ? animal.Name.ToStringFull : animal.LabelIndefinite());
						animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, (string)null);
					}
					if (PawnUtility.ShouldSendNotificationAbout(humanlike) || PawnUtility.ShouldSendNotificationAbout(animal))
					{
						string text2 = (!flag) ? "MessageNewBondRelation".Translate(humanlike.LabelShort, animal.LabelShort).CapitalizeFirst() : "MessageNewBondRelationNewName".Translate(humanlike.LabelShort, text, animal.Name.ToStringFull).AdjustedFor(animal).CapitalizeFirst();
						Messages.Message(text2, (Thing)humanlike, MessageTypeDefOf.PositiveEvent);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public static string LabelWithBondInfo(Pawn humanlike, Pawn animal)
		{
			string text = humanlike.LabelShort;
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				text = text + " " + "BondBrackets".Translate();
			}
			return text;
		}

		private static bool HasAnySocialMemoryWith(Pawn p, Pawn otherPawn)
		{
			bool result;
			if (!p.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (p.Dead)
			{
				result = false;
			}
			else
			{
				List<Thought_Memory> memories = p.needs.mood.thoughts.memories.Memories;
				for (int i = 0; i < memories.Count; i++)
				{
					Thought_MemorySocial thought_MemorySocial = memories[i] as Thought_MemorySocial;
					if (thought_MemorySocial != null && thought_MemorySocial.OtherPawn() == otherPawn)
						goto IL_007c;
				}
				result = false;
			}
			goto IL_009b;
			IL_007c:
			result = true;
			goto IL_009b;
			IL_009b:
			return result;
		}
	}
}
