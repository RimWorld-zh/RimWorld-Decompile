using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050B RID: 1291
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x06001728 RID: 5928 RVA: 0x000CBC5C File Offset: 0x000CA05C
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x000CBC84 File Offset: 0x000CA084
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x000CBCAC File Offset: 0x000CA0AC
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x000CBCD2 File Offset: 0x000CA0D2
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000CBD04 File Offset: 0x000CA104
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

		// Token: 0x0600172D RID: 5933 RVA: 0x000CBD78 File Offset: 0x000CA178
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000CBDA9 File Offset: 0x000CA1A9
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04000DBC RID: 3516
		private Pawn pawn;

		// Token: 0x04000DBD RID: 3517
		private int lastLightTick = 999999;

		// Token: 0x04000DBE RID: 3518
		private int lastOutdoorTick = 999999;
	}
}
