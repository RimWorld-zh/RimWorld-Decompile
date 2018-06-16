using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000383 RID: 899
	public class DangerWatcher
	{
		// Token: 0x06000F91 RID: 3985 RVA: 0x00083710 File Offset: 0x00081B10
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00083740 File Offset: 0x00081B40
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

		// Token: 0x06000F93 RID: 3987 RVA: 0x00083794 File Offset: 0x00081B94
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

		// Token: 0x06000F94 RID: 3988 RVA: 0x0008392C File Offset: 0x00081D2C
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04000987 RID: 2439
		private Map map;

		// Token: 0x04000988 RID: 2440
		private StoryDanger dangerRatingInt = StoryDanger.None;

		// Token: 0x04000989 RID: 2441
		private int lastUpdateTick = -10000;

		// Token: 0x0400098A RID: 2442
		private int lastColonistHarmedTick = -10000;

		// Token: 0x0400098B RID: 2443
		private const int UpdateInterval = 101;
	}
}
