using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005E RID: 94
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0001D8B8 File Offset: 0x0001BCB8
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0001D8E8 File Offset: 0x0001BCE8
		protected override void WaitTickAction()
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible();
			Pawn pawn = this.pawn;
			float extraJoyGainFactor = num;
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, this.Grave);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0001D940 File Offset: 0x0001BD40
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				(this.Grave.Corpse == null) ? null : this.Grave.Corpse.InnerPawn
			};
		}
	}
}
