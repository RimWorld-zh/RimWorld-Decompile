using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000385 RID: 901
	public class DangerWatcher
	{
		// Token: 0x0400098C RID: 2444
		private Map map;

		// Token: 0x0400098D RID: 2445
		private StoryDanger dangerRatingInt = StoryDanger.None;

		// Token: 0x0400098E RID: 2446
		private int lastUpdateTick = -10000;

		// Token: 0x0400098F RID: 2447
		private int lastColonistHarmedTick = -10000;

		// Token: 0x04000990 RID: 2448
		private const int UpdateInterval = 101;

		// Token: 0x06000F94 RID: 3988 RVA: 0x00083A5C File Offset: 0x00081E5C
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00083A8C File Offset: 0x00081E8C
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

		// Token: 0x06000F96 RID: 3990 RVA: 0x00083AE0 File Offset: 0x00081EE0
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

		// Token: 0x06000F97 RID: 3991 RVA: 0x00083C78 File Offset: 0x00082078
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}
	}
}
