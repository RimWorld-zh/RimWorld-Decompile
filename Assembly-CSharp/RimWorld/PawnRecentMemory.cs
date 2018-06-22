using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000507 RID: 1287
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x0600171F RID: 5919 RVA: 0x000CBC54 File Offset: 0x000CA054
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x000CBC7C File Offset: 0x000CA07C
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x000CBCA4 File Offset: 0x000CA0A4
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000CBCCA File Offset: 0x000CA0CA
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000CBCFC File Offset: 0x000CA0FC
		public void RecentMemoryInterval()
		{
			if (this.pawn.Spawned)
			{
				if (this.pawn.Map.glowGrid.PsychGlowAt(this.pawn.Position) != PsychGlow.Dark)
				{
					this.lastLightTick = Find.TickManager.TicksGame;
				}
				if (this.Outdoors())
				{
					this.lastOutdoorTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000CBD70 File Offset: 0x000CA170
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x000CBDA1 File Offset: 0x000CA1A1
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04000DB9 RID: 3513
		private Pawn pawn;

		// Token: 0x04000DBA RID: 3514
		private int lastLightTick = 999999;

		// Token: 0x04000DBB RID: 3515
		private int lastOutdoorTick = 999999;
	}
}
