using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public class DebugArena : WorldObjectComp
	{
		public List<Pawn> lhs;

		public List<Pawn> rhs;

		public Action<ArenaUtility.ArenaResult> callback;

		private int tickCreated = 0;

		private int tickFightStarted = 0;

		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616);
			}
			else
			{
				if (this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000)
				{
					goto IL_0078;
				}
				if (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000)
					goto IL_0078;
				if (this.tickFightStarted == 0)
				{
					foreach (Pawn item in this.lhs.Concat(this.rhs))
					{
						if (!(item.records.GetValue(RecordDefOf.ShotsFired) > 0.0) && (item.CurJob == null || item.CurJob.def != JobDefOf.AttackMelee || !(item.Position.DistanceTo(item.CurJob.targetA.Thing.Position) <= 2.0)))
						{
							continue;
						}
						this.tickFightStarted = Find.TickManager.TicksGame;
						break;
					}
				}
				if (this.tickFightStarted != 0)
				{
					bool flag = !this.lhs.Any((Predicate<Pawn>)((Pawn pawn) => !pawn.Dead && !pawn.Downed));
					bool flag2 = !this.rhs.Any((Predicate<Pawn>)((Pawn pawn) => !pawn.Dead && !pawn.Downed));
					if (!flag && !flag2)
						return;
					ArenaUtility.ArenaResult obj = new ArenaUtility.ArenaResult
					{
						tickDuration = Find.TickManager.TicksGame - this.tickFightStarted
					};
					if (flag && !flag2)
					{
						obj.winner = ArenaUtility.ArenaResult.Winner.Rhs;
					}
					else if (!flag && flag2)
					{
						obj.winner = ArenaUtility.ArenaResult.Winner.Lhs;
					}
					else
					{
						obj.winner = ArenaUtility.ArenaResult.Winner.Other;
					}
					this.callback(obj);
					foreach (Pawn item2 in this.lhs.Concat(this.rhs))
					{
						if (!item2.Destroyed)
						{
							item2.Destroy(DestroyMode.Vanish);
						}
					}
					Find.WorldObjects.Remove(base.parent);
				}
			}
			return;
			IL_0078:
			Log.Message("Fight timed out");
			ArenaUtility.ArenaResult obj2 = new ArenaUtility.ArenaResult
			{
				tickDuration = Find.TickManager.TicksGame - this.tickCreated,
				winner = ArenaUtility.ArenaResult.Winner.Other
			};
			this.callback(obj2);
			Find.WorldObjects.Remove(base.parent);
		}
	}
}
