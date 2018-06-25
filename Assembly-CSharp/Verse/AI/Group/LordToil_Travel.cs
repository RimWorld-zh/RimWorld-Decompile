using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F9 RID: 2553
	public class LordToil_Travel : LordToil
	{
		// Token: 0x0400247C RID: 9340
		public Danger maxDanger = Danger.Unspecified;

		// Token: 0x0600394F RID: 14671 RVA: 0x000501C7 File Offset: 0x0004E5C7
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x000501F0 File Offset: 0x0004E5F0
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x00050210 File Offset: 0x0004E610
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x00050230 File Offset: 0x0004E630
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x00050248 File Offset: 0x0004E648
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x00050264 File Offset: 0x0004E664
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.TravelOrLeave, data.dest, -1f);
				pawnDuty.maxDanger = this.maxDanger;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x000502E0 File Offset: 0x0004E6E0
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("TravelArrived");
				}
			}
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0005039C File Offset: 0x0004E79C
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x000503C1 File Offset: 0x0004E7C1
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}
	}
}
