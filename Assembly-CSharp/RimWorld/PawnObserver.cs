using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000509 RID: 1289
	public class PawnObserver
	{
		// Token: 0x06001723 RID: 5923 RVA: 0x000CBA7C File Offset: 0x000C9E7C
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = Rand.Range(0, 4);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000CBA9C File Offset: 0x000C9E9C
		public void ObserverInterval()
		{
			if (this.pawn.Spawned)
			{
				this.intervalsUntilObserve--;
				if (this.intervalsUntilObserve <= 0)
				{
					this.ObserveSurroundingThings();
					this.intervalsUntilObserve = 4 + Rand.RangeInclusive(-1, 1);
				}
			}
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x000CBAF0 File Offset: 0x000C9EF0
		private void ObserveSurroundingThings()
		{
			if (this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				Map map = this.pawn.Map;
				int num = 0;
				while ((float)num < 100f)
				{
					IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(map))
					{
						if (GenSight.LineOfSight(intVec, this.pawn.Position, map, true, null, 0, 0))
						{
							List<Thing> thingList = intVec.GetThingList(map);
							for (int i = 0; i < thingList.Count; i++)
							{
								IThoughtGiver thoughtGiver = thingList[i] as IThoughtGiver;
								if (thoughtGiver != null)
								{
									Thought_Memory thought_Memory = thoughtGiver.GiveObservedThought();
									if (thought_Memory != null)
									{
										this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
									}
								}
							}
						}
					}
					num++;
				}
			}
		}

		// Token: 0x04000DB8 RID: 3512
		private Pawn pawn;

		// Token: 0x04000DB9 RID: 3513
		private int intervalsUntilObserve;

		// Token: 0x04000DBA RID: 3514
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04000DBB RID: 3515
		private const float SampleNumCells = 100f;
	}
}
