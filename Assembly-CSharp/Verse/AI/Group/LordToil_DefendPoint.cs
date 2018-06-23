using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009EF RID: 2543
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x04002472 RID: 9330
		private bool allowSatisfyLongNeeds = true;

		// Token: 0x0600392E RID: 14638 RVA: 0x0004F07E File Offset: 0x0004D47E
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0004F0A0 File Offset: 0x0004D4A0
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x0004F0C4 File Offset: 0x0004D4C4
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x0004F0E4 File Offset: 0x0004D4E4
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003932 RID: 14642 RVA: 0x0004F104 File Offset: 0x0004D504
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x0004F120 File Offset: 0x0004D520
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

		// Token: 0x06003934 RID: 14644 RVA: 0x0004F1DE File Offset: 0x0004D5DE
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}
	}
}
