using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B9 RID: 697
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0006913C File Offset: 0x0006753C
		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040006C7 RID: 1735
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x040006C8 RID: 1736
		public SimpleCurve selectionWeightCurve;

		// Token: 0x040006C9 RID: 1737
		public float pointsFactor = 1f;

		// Token: 0x040006CA RID: 1738
		public TechLevel minTechLevel = TechLevel.Undefined;

		// Token: 0x040006CB RID: 1739
		public bool forQuickMilitaryAid;

		// Token: 0x040006CC RID: 1740
		[MustTranslate]
		public string textEnemy;

		// Token: 0x040006CD RID: 1741
		[MustTranslate]
		public string textFriendly;

		// Token: 0x040006CE RID: 1742
		[Unsaved]
		private PawnsArrivalModeWorker workerInt;
	}
}
