using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override Action GetWaitTickAction()
		{
			return (Action)delegate
			{
				float num = this.Grave.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
				Room room = base.pawn.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
				}
				base.pawn.GainComfortFromCellIfPossible();
				float extraJoyGainFactor = num;
				JoyUtility.JoyTickCheckEnd(base.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor);
			};
		}
	}
}
