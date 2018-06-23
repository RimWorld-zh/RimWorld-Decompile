using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BC RID: 700
	public class RaidStrategyDef : Def
	{
		// Token: 0x040006D0 RID: 1744
		public Type workerClass;

		// Token: 0x040006D1 RID: 1745
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x040006D2 RID: 1746
		public float minPawns = 1f;

		// Token: 0x040006D3 RID: 1747
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x040006D4 RID: 1748
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x040006D5 RID: 1749
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x040006D6 RID: 1750
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x040006D7 RID: 1751
		public float pointsFactor = 1f;

		// Token: 0x040006D8 RID: 1752
		public bool pawnsCanBringFood = false;

		// Token: 0x040006D9 RID: 1753
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x040006DA RID: 1754
		private RaidStrategyWorker workerInt;

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0006931C File Offset: 0x0006771C
		public RaidStrategyWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidStrategyWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
