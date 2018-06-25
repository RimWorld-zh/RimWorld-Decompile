using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000199 RID: 409
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x0400038D RID: 909
		private int sapperThingID;

		// Token: 0x0600086D RID: 2157 RVA: 0x000503CC File Offset: 0x0004E7CC
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000503E0 File Offset: 0x0004E7E0
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00050400 File Offset: 0x0004E800
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00050420 File Offset: 0x0004E820
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00050438 File Offset: 0x0004E838
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00050454 File Offset: 0x0004E854
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, leader, 10f);
				}
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00050534 File Offset: 0x0004E934
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.guilt.Notify_Guilty();
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00050588 File Offset: 0x0004E988
		private Pawn GetLeader()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (!this.lord.ownedPawns[i].Downed && this.IsSapper(this.lord.ownedPawns[i]))
				{
					return this.lord.ownedPawns[i];
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				if (!this.lord.ownedPawns[j].Downed)
				{
					return this.lord.ownedPawns[j];
				}
			}
			return null;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00050664 File Offset: 0x0004EA64
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}
	}
}
