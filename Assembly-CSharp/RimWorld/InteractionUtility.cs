using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class InteractionUtility
	{
		public const float MaxInteractRange = 6f;

		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		public static bool CanInitiateInteraction(Pawn pawn)
		{
			return (byte)((pawn.interactions != null) ? (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) ? (pawn.Awake() ? ((!pawn.IsBurning()) ? 1 : 0) : 0) : 0) : 0) != 0;
		}

		public static bool CanReceiveInteraction(Pawn pawn)
		{
			return (byte)(pawn.Awake() ? ((!pawn.IsBurning()) ? 1 : 0) : 0) != 0;
		}

		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return (byte)(InteractionUtility.CanInitiateInteraction(p) ? ((p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState) ? ((p.Faction != null) ? 1 : 0) : 0) : 0) != 0;
		}

		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return (byte)(InteractionUtility.CanReceiveInteraction(p) ? ((p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState) ? 1 : 0) : 0) != 0;
		}

		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		public static bool HasAnyVerbForSocialFight(Pawn p)
		{
			bool result;
			if (p.Dead)
			{
				result = false;
			}
			else
			{
				List<Verb> allVerbs = p.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i] is Verb_MeleeAttack && allVerbs[i].IsStillUsableBy(p))
						goto IL_004a;
				}
				result = false;
			}
			goto IL_0069;
			IL_0069:
			return result;
			IL_004a:
			result = true;
			goto IL_0069;
		}

		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			bool result;
			if (p.Dead)
			{
				verb = null;
				result = false;
			}
			else
			{
				List<Verb> allVerbs = p.verbTracker.AllVerbs;
				result = (from x in allVerbs
				where x is Verb_MeleeAttack && x.IsStillUsableBy(p)
				select x).TryRandomElementByWeight<Verb>((Func<Verb, float>)((Verb x) => x.verbProps.AdjustedMeleeDamageAmount(x, p, null)), out verb);
			}
			return result;
		}

		public static bool HasAnySocialFightProvokingThought(Pawn pawn, Pawn otherPawn)
		{
			Thought thought = default(Thought);
			return InteractionUtility.TryGetRandomSocialFightProvokingThought(pawn, otherPawn, out thought);
		}

		public static bool TryGetRandomSocialFightProvokingThought(Pawn pawn, Pawn otherPawn, out Thought thought)
		{
			bool result;
			if (pawn.needs.mood == null)
			{
				thought = null;
				result = false;
			}
			else
			{
				pawn.needs.mood.thoughts.GetSocialThoughts(otherPawn, InteractionUtility.tmpSocialThoughts);
				ISocialThought socialThought = default(ISocialThought);
				bool flag = InteractionUtility.tmpSocialThoughts.Where((Func<ISocialThought, bool>)delegate(ISocialThought x)
				{
					ThoughtDef def = ((Thought)x).def;
					return def != ThoughtDefOf.HadAngeringFight && def != ThoughtDefOf.HadCatharticFight && x.OpinionOffset() < 0.0;
				}).TryRandomElementByWeight<ISocialThought>((Func<ISocialThought, float>)((ISocialThought x) => (float)(0.0 - x.OpinionOffset())), out socialThought);
				InteractionUtility.tmpSocialThoughts.Clear();
				thought = (Thought)socialThought;
				result = flag;
			}
			return result;
		}
	}
}
