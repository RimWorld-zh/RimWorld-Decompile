using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ThoughtUtility
	{
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		public static List<ThoughtDef> situationalNonSocialThoughtDefs;

		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList();
		}

		public static void GiveThoughtsForPawnExecuted(Pawn victim, PawnExecutionKind kind)
		{
			if (victim.RaceProps.Humanlike)
			{
				int forcedStage = 1;
				if (victim.guilt.IsGuilty)
				{
					forcedStage = 0;
				}
				else
				{
					switch (kind)
					{
					case PawnExecutionKind.GenericHumane:
					{
						forcedStage = 1;
						break;
					}
					case PawnExecutionKind.GenericBrutal:
					{
						forcedStage = 2;
						break;
					}
					case PawnExecutionKind.OrganHarvesting:
					{
						forcedStage = 3;
						break;
					}
					}
				}
				ThoughtDef def = (!victim.IsColonist) ? ThoughtDefOf.KnowGuestExecuted : ThoughtDefOf.KnowColonistExecuted;
				foreach (Pawn item in from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods
				where x.IsColonist || x.IsPrisonerOfColony
				select x)
				{
					item.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
				}
			}
		}

		public static void GiveThoughtsForPawnOrganHarvested(Pawn victim)
		{
			if (victim.RaceProps.Humanlike)
			{
				ThoughtDef thoughtDef = null;
				if (victim.IsColonist)
				{
					thoughtDef = ThoughtDefOf.KnowColonistOrganHarvested;
				}
				else if (victim.HostFaction == Faction.OfPlayer)
				{
					thoughtDef = ThoughtDefOf.KnowGuestOrganHarvested;
				}
				foreach (Pawn item in from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods
				where x.IsColonist || x.IsPrisonerOfColony
				select x)
				{
					if (item == victim)
					{
						item.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null);
					}
					else if (thoughtDef != null)
					{
						item.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
					}
				}
			}
		}

		public static bool IsSituationalThoughtNullifiedByHediffs(ThoughtDef def, Pawn pawn)
		{
			bool result;
			if (def.IsMemory)
			{
				result = false;
			}
			else
			{
				float num = 0f;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null && curStage.pctConditionalThoughtsNullified > num)
					{
						num = curStage.pctConditionalThoughtsNullified;
					}
				}
				if (num == 0.0)
				{
					result = false;
				}
				else
				{
					Rand.PushState();
					Rand.Seed = pawn.thingIDNumber * 31 + def.index * 139;
					bool flag = Rand.Value < num;
					Rand.PopState();
					result = flag;
				}
			}
			return result;
		}

		public static bool IsThoughtNullifiedByOwnTales(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingOwnTales != null)
			{
				for (int i = 0; i < def.nullifyingOwnTales.Count; i++)
				{
					if (Find.TaleManager.GetLatestTale(def.nullifyingOwnTales[i], pawn) != null)
						goto IL_0031;
				}
			}
			bool result = false;
			goto IL_0056;
			IL_0031:
			result = true;
			goto IL_0056;
			IL_0056:
			return result;
		}

		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			if (pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Func<Thought_Memory, bool>)((Thought_Memory thought) => thought.MoodOffset() > 0.0));
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Func<Thought_Memory, bool>)((Thought_Memory thought) => thought.MoodOffset() > 0.0));
			}
		}

		public static bool CanGetThought(Pawn pawn, ThoughtDef def)
		{
			try
			{
				if (!def.validWhileDespawned && !pawn.Spawned && !def.IsMemory)
				{
					return false;
				}
				if (def.nullifyingTraits != null)
				{
					for (int i = 0; i < def.nullifyingTraits.Count; i++)
					{
						if (pawn.story.traits.HasTrait(def.nullifyingTraits[i]))
						{
							return false;
						}
					}
				}
				if (!def.requiredTraits.NullOrEmpty())
				{
					bool flag = false;
					int num = 0;
					while (num < def.requiredTraits.Count)
					{
						if (!pawn.story.traits.HasTrait(def.requiredTraits[num]) || (def.RequiresSpecificTraitsDegree && def.requiredTraitsDegree != pawn.story.traits.DegreeOfTrait(def.requiredTraits[num])))
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (!flag)
					{
						return false;
					}
				}
				if (def.nullifiedIfNotColonist && !pawn.IsColonist)
				{
					return false;
				}
				if (ThoughtUtility.IsSituationalThoughtNullifiedByHediffs(def, pawn))
				{
					return false;
				}
				if (ThoughtUtility.IsThoughtNullifiedByOwnTales(def, pawn))
				{
					return false;
				}
			}
			finally
			{
			}
			return true;
		}
	}
}
