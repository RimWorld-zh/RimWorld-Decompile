using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000529 RID: 1321
	public static class InteractionUtility
	{
		// Token: 0x04000E6A RID: 3690
		public const float MaxInteractRange = 6f;

		// Token: 0x04000E6B RID: 3691
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		// Token: 0x0600182D RID: 6189 RVA: 0x000D380C File Offset: 0x000D1C0C
		public static bool CanInitiateInteraction(Pawn pawn)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning();
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x000D387C File Offset: 0x000D1C7C
		public static bool CanReceiveInteraction(Pawn pawn)
		{
			return pawn.Awake() && !pawn.IsBurning();
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000D38B8 File Offset: 0x000D1CB8
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanInitiateInteraction(p) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && p.Faction != null;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x000D3920 File Offset: 0x000D1D20
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanReceiveInteraction(p) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x000D3978 File Offset: 0x000D1D78
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000D39A4 File Offset: 0x000D1DA4
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000D39DC File Offset: 0x000D1DDC
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
					if (allVerbs[i].IsMeleeAttack && allVerbs[i].IsStillUsableBy(p))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000D3A54 File Offset: 0x000D1E54
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
				where x.IsMeleeAttack && x.IsStillUsableBy(p)
				select x).TryRandomElementByWeight((Verb x) => x.verbProps.AdjustedMeleeDamageAmount(x, p, null), out verb);
			}
			return result;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000D3AC8 File Offset: 0x000D1EC8
		public static bool HasAnySocialFightProvokingThought(Pawn pawn, Pawn otherPawn)
		{
			Thought thought;
			return InteractionUtility.TryGetRandomSocialFightProvokingThought(pawn, otherPawn, out thought);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000D3AE8 File Offset: 0x000D1EE8
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
				ISocialThought socialThought;
				bool flag = InteractionUtility.tmpSocialThoughts.Where(delegate(ISocialThought x)
				{
					ThoughtDef def = ((Thought)x).def;
					return def != ThoughtDefOf.HadAngeringFight && def != ThoughtDefOf.HadCatharticFight && x.OpinionOffset() < 0f;
				}).TryRandomElementByWeight((ISocialThought x) => -x.OpinionOffset(), out socialThought);
				InteractionUtility.tmpSocialThoughts.Clear();
				thought = (Thought)socialThought;
				result = flag;
			}
			return result;
		}
	}
}
