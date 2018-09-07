using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class DangerWatcher
	{
		private Map map;

		private StoryDanger dangerRatingInt;

		private int lastUpdateTick = -10000;

		private int lastColonistHarmedTick = -10000;

		private const int UpdateInterval = 101;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<IAttackTarget, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					this.dangerRatingInt = this.CalculateDangerRating();
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		private StoryDanger CalculateDangerRating()
		{
			IEnumerable<IAttackTarget> targetsHostileToColony = this.map.attackTargetsCache.TargetsHostileToColony;
			if (DangerWatcher.<>f__mg$cache0 == null)
			{
				DangerWatcher.<>f__mg$cache0 = new Func<IAttackTarget, bool>(GenHostility.IsActiveThreatToPlayer);
			}
			float num = targetsHostileToColony.Where(DangerWatcher.<>f__mg$cache0).Sum((IAttackTarget t) => (!(t is Pawn)) ? 0f : ((Pawn)t).kindDef.combatPower);
			if (num == 0f)
			{
				return StoryDanger.None;
			}
			int num2 = (from p in this.map.mapPawns.FreeColonistsSpawned
			where !p.Downed
			select p).Count<Pawn>();
			if (num < 150f && num <= (float)num2 * 18f)
			{
				return StoryDanger.Low;
			}
			if (num > 400f)
			{
				return StoryDanger.High;
			}
			if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
			{
				return StoryDanger.High;
			}
			foreach (Lord lord in this.map.lordManager.lords)
			{
				if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
				{
					return StoryDanger.High;
				}
			}
			return StoryDanger.Low;
		}

		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		[CompilerGenerated]
		private static float <CalculateDangerRating>m__0(IAttackTarget t)
		{
			return (!(t is Pawn)) ? 0f : ((Pawn)t).kindDef.combatPower;
		}

		[CompilerGenerated]
		private static bool <CalculateDangerRating>m__1(Pawn p)
		{
			return !p.Downed;
		}
	}
}
