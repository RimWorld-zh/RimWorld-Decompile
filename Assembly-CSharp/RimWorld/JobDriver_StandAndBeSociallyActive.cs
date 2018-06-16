using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200004F RID: 79
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06000273 RID: 627 RVA: 0x00019F14 File Offset: 0x00018314
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00019F2C File Offset: 0x0001832C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = this.FindClosePawn();
					if (pawn != null)
					{
						this.pawn.rotationTracker.FaceCell(pawn.Position);
					}
					this.pawn.GainComfortFromCellIfPossible();
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00019F58 File Offset: 0x00018358
		private Pawn FindClosePawn()
		{
			IntVec3 position = this.pawn.Position;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map))
				{
					Thing thing = intVec.GetThingList(base.Map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != this.pawn)
					{
						if (GenSight.LineOfSight(position, intVec, base.Map, false, null, 0, 0))
						{
							return (Pawn)thing;
						}
					}
				}
			}
			return null;
		}
	}
}
