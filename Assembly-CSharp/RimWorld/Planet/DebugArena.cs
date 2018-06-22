using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000619 RID: 1561
	public class DebugArena : WorldObjectComp
	{
		// Token: 0x06001FB4 RID: 8116 RVA: 0x00111B34 File Offset: 0x0010FF34
		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00111B5C File Offset: 0x0010FF5C
		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616, false);
			}
			else if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out", false);
				ArenaUtility.ArenaResult obj = default(ArenaUtility.ArenaResult);
				obj.tickDuration = Find.TickManager.TicksGame - this.tickCreated;
				obj.winner = ArenaUtility.ArenaResult.Winner.Other;
				this.callback(obj);
				Find.WorldObjects.Remove(this.parent);
			}
			else
			{
				if (this.tickFightStarted == 0)
				{
					foreach (Pawn pawn3 in this.lhs.Concat(this.rhs))
					{
						if (pawn3.records.GetValue(RecordDefOf.ShotsFired) > 0f || (pawn3.CurJob != null && pawn3.CurJob.def == JobDefOf.AttackMelee && pawn3.Position.DistanceTo(pawn3.CurJob.targetA.Thing.Position) <= 2f))
						{
							this.tickFightStarted = Find.TickManager.TicksGame;
							break;
						}
					}
				}
				if (this.tickFightStarted != 0)
				{
					bool flag = !this.lhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
					bool flag2 = !this.rhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
					if (flag || flag2)
					{
						ArenaUtility.ArenaResult obj2 = default(ArenaUtility.ArenaResult);
						obj2.tickDuration = Find.TickManager.TicksGame - this.tickFightStarted;
						if (flag && !flag2)
						{
							obj2.winner = ArenaUtility.ArenaResult.Winner.Rhs;
						}
						else if (!flag && flag2)
						{
							obj2.winner = ArenaUtility.ArenaResult.Winner.Lhs;
						}
						else
						{
							obj2.winner = ArenaUtility.ArenaResult.Winner.Other;
						}
						this.callback(obj2);
						foreach (Pawn pawn2 in this.lhs.Concat(this.rhs))
						{
							if (!pawn2.Destroyed)
							{
								pawn2.Destroy(DestroyMode.Vanish);
							}
						}
						Find.WorldObjects.Remove(this.parent);
					}
				}
			}
		}

		// Token: 0x0400125D RID: 4701
		public List<Pawn> lhs;

		// Token: 0x0400125E RID: 4702
		public List<Pawn> rhs;

		// Token: 0x0400125F RID: 4703
		public Action<ArenaUtility.ArenaResult> callback;

		// Token: 0x04001260 RID: 4704
		private int tickCreated = 0;

		// Token: 0x04001261 RID: 4705
		private int tickFightStarted = 0;
	}
}
