using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000507 RID: 1287
	public class PawnObserver
	{
		// Token: 0x04000DB5 RID: 3509
		private Pawn pawn;

		// Token: 0x04000DB6 RID: 3510
		private int intervalsUntilObserve;

		// Token: 0x04000DB7 RID: 3511
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04000DB8 RID: 3512
		private const float SampleNumCells = 100f;

		// Token: 0x0600171F RID: 5919 RVA: 0x000CBC18 File Offset: 0x000CA018
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = Rand.Range(0, 4);
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000CBC38 File Offset: 0x000CA038
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

		// Token: 0x06001721 RID: 5921 RVA: 0x000CBC8C File Offset: 0x000CA08C
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
	}
}
