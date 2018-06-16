using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CB RID: 459
	public class ThinkNode_ConditionalShouldFollowMaster : ThinkNode_Conditional
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x0005621C File Offset: 0x0005461C
		protected override bool Satisfied(Pawn pawn)
		{
			return ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00056238 File Offset: 0x00054638
		public static bool ShouldFollowMaster(Pawn pawn)
		{
			bool result;
			if (!pawn.Spawned || pawn.playerSettings == null)
			{
				result = false;
			}
			else
			{
				Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
				if (respectedMaster == null)
				{
					result = false;
				}
				else
				{
					if (respectedMaster.Spawned)
					{
						if (pawn.playerSettings.followDrafted && respectedMaster.Drafted && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return true;
						}
						if (pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
					else
					{
						Pawn carriedBy = respectedMaster.CarriedBy;
						if (carriedBy != null && carriedBy.HostileTo(respectedMaster) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}
	}
}
