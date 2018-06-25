using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C1 RID: 2241
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x04001B96 RID: 7062
		private int startingAreaCount = -1;

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003346 RID: 13126
		protected abstract Area MyArea { get; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003347 RID: 13127 RVA: 0x001B8EA4 File Offset: 0x001B72A4
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x001B8ED9 File Offset: 0x001B72D9
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x001B8EF3 File Offset: 0x001B72F3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x001B8F0E File Offset: 0x001B730E
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
