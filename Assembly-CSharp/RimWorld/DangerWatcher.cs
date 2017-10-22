using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class DangerWatcher
	{
		private const int UpdateInterval = 101;

		private const int ColonistHarmedDangerSeconds = 15;

		private Map map;

		private StoryDanger dangerRatingInt;

		private int lastUpdateTick = -10000;

		private int lastColonistHarmedTick = -10000;

		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					int num = this.map.attackTargetsCache.TargetsHostileToColony.Count((Func<IAttackTarget, bool>)((IAttackTarget x) => !x.ThreatDisabled()));
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
							List<Lord>.Enumerator enumerator = this.map.lordManager.lords.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									Lord current = enumerator.Current;
									if (current.CurLordToil is LordToil_AssaultColony)
									{
										this.dangerRatingInt = StoryDanger.High;
										break;
									}
								}
							}
							finally
							{
								((IDisposable)(object)enumerator).Dispose();
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
