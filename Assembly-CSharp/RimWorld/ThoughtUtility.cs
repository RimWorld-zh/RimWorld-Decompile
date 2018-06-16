using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D0 RID: 2512
	public static class ThoughtUtility
	{
		// Token: 0x0600383F RID: 14399 RVA: 0x001DF790 File Offset: 0x001DDB90
		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList<ThoughtDef>();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList<ThoughtDef>();
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x001DF800 File Offset: 0x001DDC00
		public static void GiveThoughtsForPawnExecuted(Pawn victim, PawnExecutionKind kind)
		{
			if (victim.RaceProps.Humanlike)
			{
				int forcedStage = 1;
				if (victim.guilt.IsGuilty)
				{
					forcedStage = 0;
				}
				else if (kind != PawnExecutionKind.GenericHumane)
				{
					if (kind != PawnExecutionKind.GenericBrutal)
					{
						if (kind == PawnExecutionKind.OrganHarvesting)
						{
							forcedStage = 3;
						}
					}
					else
					{
						forcedStage = 2;
					}
				}
				else
				{
					forcedStage = 1;
				}
				ThoughtDef def;
				if (victim.IsColonist)
				{
					def = ThoughtDefOf.KnowColonistExecuted;
				}
				else
				{
					def = ThoughtDefOf.KnowGuestExecuted;
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
				}
			}
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x001DF8F4 File Offset: 0x001DDCF4
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
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
				{
					if (pawn == victim)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null);
					}
					else if (thoughtDef != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
					}
				}
			}
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x001DF9DC File Offset: 0x001DDDDC
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
				if (num == 0f)
				{
					result = false;
				}
				else
				{
					Rand.PushState();
					Rand.Seed = pawn.thingIDNumber * 31 + (int)(def.index * 139);
					bool flag = Rand.Value < num;
					Rand.PopState();
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x001DFAA0 File Offset: 0x001DDEA0
		public static bool IsThoughtNullifiedByOwnTales(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingOwnTales != null)
			{
				for (int i = 0; i < def.nullifyingOwnTales.Count; i++)
				{
					if (Find.TaleManager.GetLatestTale(def.nullifyingOwnTales[i], pawn) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x001DFB04 File Offset: 0x001DDF04
		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			if (pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Thought_Memory thought) => thought.MoodOffset() > 0f);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			}
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001DFBA0 File Offset: 0x001DDFA0
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
				if (!def.requiredTraits.NullOrEmpty<TraitDef>())
				{
					bool flag = false;
					for (int j = 0; j < def.requiredTraits.Count; j++)
					{
						if (pawn.story.traits.HasTrait(def.requiredTraits[j]))
						{
							if (!def.RequiresSpecificTraitsDegree || def.requiredTraitsDegree == pawn.story.traits.DegreeOfTrait(def.requiredTraits[j]))
							{
								flag = true;
								break;
							}
						}
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

		// Token: 0x040023FF RID: 9215
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		// Token: 0x04002400 RID: 9216
		public static List<ThoughtDef> situationalNonSocialThoughtDefs;
	}
}
