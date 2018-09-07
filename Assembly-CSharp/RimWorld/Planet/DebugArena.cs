using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class DebugArena : WorldObjectComp
	{
		public List<Pawn> lhs;

		public List<Pawn> rhs;

		public Action<ArenaUtility.ArenaResult> callback;

		private int tickCreated;

		private int tickFightStarted;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache1;

		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616, false);
				return;
			}
			if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out", false);
				ArenaUtility.ArenaResult obj = default(ArenaUtility.ArenaResult);
				obj.tickDuration = Find.TickManager.TicksGame - this.tickCreated;
				obj.winner = ArenaUtility.ArenaResult.Winner.Other;
				this.callback(obj);
				Find.WorldObjects.Remove(this.parent);
				return;
			}
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

		[CompilerGenerated]
		private static bool <CompTick>m__0(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Downed && pawn.Spawned;
		}

		[CompilerGenerated]
		private static bool <CompTick>m__1(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Downed && pawn.Spawned;
		}
	}
}
