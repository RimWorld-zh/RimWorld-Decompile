using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A3 RID: 419
	public class LordToil_Party : LordToil
	{
		// Token: 0x040003AA RID: 938
		private IntVec3 spot;

		// Token: 0x040003AB RID: 939
		private int ticksPerPartyPulse = 600;

		// Token: 0x040003AC RID: 940
		private const int DefaultTicksPerPartyPulse = 600;

		// Token: 0x060008AE RID: 2222 RVA: 0x00051EFA File Offset: 0x000502FA
		public LordToil_Party(IntVec3 spot, int ticksPerPartyPulse = 600)
		{
			this.spot = spot;
			this.ticksPerPartyPulse = ticksPerPartyPulse;
			this.data = new LordToilData_Party();
			this.Data.ticksToNextPulse = ticksPerPartyPulse;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00051F34 File Offset: 0x00050334
		private LordToilData_Party Data
		{
			get
			{
				return (LordToilData_Party)this.data;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00051F54 File Offset: 0x00050354
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return DutyDefOf.Party.hook;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00051F74 File Offset: 0x00050374
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Party, this.spot, -1f);
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00051FDC File Offset: 0x000503DC
		public override void LordToilTick()
		{
			if (--this.Data.ticksToNextPulse <= 0)
			{
				this.Data.ticksToNextPulse = this.ticksPerPartyPulse;
				List<Pawn> ownedPawns = this.lord.ownedPawns;
				for (int i = 0; i < ownedPawns.Count; i++)
				{
					if (PartyUtility.InPartyArea(ownedPawns[i].Position, this.spot, base.Map))
					{
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedParty, null);
						LordJob_Joinable_Party lordJob_Joinable_Party = this.lord.LordJob as LordJob_Joinable_Party;
						if (lordJob_Joinable_Party != null)
						{
							TaleRecorder.RecordTale(TaleDefOf.AttendedParty, new object[]
							{
								ownedPawns[i],
								lordJob_Joinable_Party.Organizer
							});
						}
					}
				}
			}
		}
	}
}
