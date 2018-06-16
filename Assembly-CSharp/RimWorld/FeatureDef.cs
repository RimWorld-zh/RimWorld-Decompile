using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000294 RID: 660
	public class FeatureDef : Def
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x000656D4 File Offset: 0x00063AD4
		public FeatureWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (FeatureWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040005DB RID: 1499
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x040005DC RID: 1500
		public float order;

		// Token: 0x040005DD RID: 1501
		public int minSize = 50;

		// Token: 0x040005DE RID: 1502
		public int maxSize = int.MaxValue;

		// Token: 0x040005DF RID: 1503
		public bool canTouchWorldEdge = true;

		// Token: 0x040005E0 RID: 1504
		public RulePackDef nameMaker;

		// Token: 0x040005E1 RID: 1505
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x040005E2 RID: 1506
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x040005E3 RID: 1507
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x040005E4 RID: 1508
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x040005E5 RID: 1509
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x040005E6 RID: 1510
		public int minRootGroupsInCluster = 3;

		// Token: 0x040005E7 RID: 1511
		public int minRootGroupSize = 10;

		// Token: 0x040005E8 RID: 1512
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x040005E9 RID: 1513
		public int maxPassageWidth = 3;

		// Token: 0x040005EA RID: 1514
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x040005EB RID: 1515
		[Unsaved]
		private FeatureWorker workerInt;
	}
}
