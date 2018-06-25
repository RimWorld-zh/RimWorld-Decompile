using System;
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

		private StoryDanger dangerRatingInt = StoryDanger.None;

		private int lastUpdateTick = -10000;

		private int lastColonistHarmedTick = -10000;

		private const int UpdateInterval = 101;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IAttackTarget, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache2;

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
			float num = (from x in this.map.attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x)
			select x).Sum((IAttackTarget t) => (!(t is Pawn)) ? 0f : ((Pawn)t).kindDef.combatPower);
			StoryDanger result;
			if (num == 0f)
			{
				result = StoryDanger.None;
			}
			else
			{
				int num2 = (from p in this.map.mapPawns.FreeColonistsSpawned
				where !p.Downed
				select p).Count<Pawn>();
				if (num < 150f && num <= (float)num2 * 18f)
				{
					result = StoryDanger.Low;
				}
				else if (num > 400f)
				{
					result = StoryDanger.High;
				}
				else if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
				{
					result = StoryDanger.High;
				}
				else
				{
					foreach (Lord lord in this.map.lordManager.lords)
					{
						if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
						{
							return StoryDanger.High;
						}
					}
					result = StoryDanger.Low;
				}
			}
			return result;
		}

		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		[CompilerGenerated]
		private static bool <CalculateDangerRating>m__0(IAttackTarget x)
		{
			return GenHostility.IsActiveThreatToPlayer(x);
		}

		[CompilerGenerated]
		private static float <CalculateDangerRating>m__1(IAttackTarget t)
		{
			return (!(t is Pawn)) ? 0f : ((Pawn)t).kindDef.combatPower;
		}

		[CompilerGenerated]
		private static bool <CalculateDangerRating>m__2(Pawn p)
		{
			return !p.Downed;
		}
	}
}
