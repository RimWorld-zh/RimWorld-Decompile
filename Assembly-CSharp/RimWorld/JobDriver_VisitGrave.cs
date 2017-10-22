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
				return (Building_Grave)base.job.GetTarget(TargetIndex.A).Thing;
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
				Pawn pawn = base.pawn;
				float extraJoyGainFactor = num;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor);
			};
		}

		public override object[] TaleParameters()
		{
			return new object[2]
			{
				base.pawn,
				(this.Grave.Corpse == null) ? null : this.Grave.Corpse.InnerPawn
			};
		}
	}
}
