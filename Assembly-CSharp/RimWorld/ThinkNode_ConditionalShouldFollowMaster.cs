using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalShouldFollowMaster : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			return ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		public static bool ShouldFollowMaster(Pawn pawn)
		{
			bool result;
			if (pawn.playerSettings == null)
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
					Pawn carriedBy = respectedMaster.CarriedBy;
					result = ((byte)((respectedMaster.Spawned || carriedBy != null) ? ((carriedBy != null && carriedBy.HostileTo(respectedMaster)) ? 1 : ((pawn.playerSettings.followDrafted && respectedMaster.Drafted) ? 1 : ((pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork) ? 1 : 0))) : 0) != 0);
				}
			}
			return result;
		}
	}
}
