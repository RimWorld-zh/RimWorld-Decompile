using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200019D RID: 413
	public class LordToil_Stage : LordToil
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x00051802 File Offset: 0x0004FC02
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00051824 File Offset: 0x0004FC24
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00051844 File Offset: 0x0004FC44
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00051864 File Offset: 0x0004FC64
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0005187C File Offset: 0x0004FC7C
		public override void UpdateAllDuties()
		{
			LordToilData_Stage data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.stagingPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
