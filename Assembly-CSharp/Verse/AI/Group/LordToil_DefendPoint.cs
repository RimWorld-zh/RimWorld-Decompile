using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F3 RID: 2547
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x06003934 RID: 14644 RVA: 0x0004F092 File Offset: 0x0004D492
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x0004F0B4 File Offset: 0x0004D4B4
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x0004F0D8 File Offset: 0x0004D4D8
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x0004F0F8 File Offset: 0x0004D4F8
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003938 RID: 14648 RVA: 0x0004F118 File Offset: 0x0004D518
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0004F134 File Offset: 0x0004D534
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.focusSecond = data.defendPoint;
				this.lord.ownedPawns[i].mindState.duty.radius = data.defendRadius;
			}
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x0004F1F2 File Offset: 0x0004D5F2
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x04002477 RID: 9335
		private bool allowSatisfyLongNeeds = true;
	}
}
