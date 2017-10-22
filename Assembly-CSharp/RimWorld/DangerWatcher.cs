using System;
using System.Linq;
using UnityEngine;
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

		private const int ColonistHarmedDangerSeconds = 15;

		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					int num = this.map.attackTargetsCache.TargetsHostileToColony.Count((Func<IAttackTarget, bool>)((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x)));
					if (num == 0)
					{
						this.dangerRatingInt = StoryDanger.None;
					}
					else if (num <= Mathf.CeilToInt((float)((float)this.map.mapPawns.FreeColonistsSpawnedCount * 0.5)))
					{
						this.dangerRatingInt = StoryDanger.Low;
					}
					else
					{
						this.dangerRatingInt = StoryDanger.Low;
						if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
						{
							this.dangerRatingInt = StoryDanger.High;
						}
						else
						{
							foreach (Lord lord in this.map.lordManager.lords)
							{
								if (lord.CurLordToil is LordToil_AssaultColony)
								{
									this.dangerRatingInt = StoryDanger.High;
									break;
								}
							}
						}
					}
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}
	}
}
