using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BB RID: 699
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x040006C8 RID: 1736
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x040006C9 RID: 1737
		public SimpleCurve selectionWeightCurve;

		// Token: 0x040006CA RID: 1738
		public float pointsFactor = 1f;

		// Token: 0x040006CB RID: 1739
		public TechLevel minTechLevel = TechLevel.Undefined;

		// Token: 0x040006CC RID: 1740
		public bool forQuickMilitaryAid;

		// Token: 0x040006CD RID: 1741
		[MustTranslate]
		public string textEnemy;

		// Token: 0x040006CE RID: 1742
		[MustTranslate]
		public string textFriendly;

		// Token: 0x040006CF RID: 1743
		[Unsaved]
		private PawnsArrivalModeWorker workerInt;

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x000692F0 File Offset: 0x000676F0
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
	}
}
