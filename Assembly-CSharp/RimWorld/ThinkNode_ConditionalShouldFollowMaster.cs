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
			if (pawn.playerSettings == null)
			{
				return false;
			}
			Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
			if (respectedMaster == null)
			{
				return false;
			}
			Pawn carriedBy = respectedMaster.CarriedBy;
			if (!respectedMaster.Spawned && carriedBy == null)
			{
				return false;
			}
			if (carriedBy != null && carriedBy.HostileTo(respectedMaster))
			{
				return true;
			}
			if (pawn.playerSettings.followDrafted && respectedMaster.Drafted)
			{
				return true;
			}
			if (pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork)
			{
				return true;
			}
			return false;
		}
	}
}
