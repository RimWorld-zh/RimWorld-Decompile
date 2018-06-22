using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BF RID: 2239
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003342 RID: 13122
		protected abstract Area MyArea { get; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x001B8D64 File Offset: 0x001B7164
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x001B8D99 File Offset: 0x001B7199
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x001B8DB3 File Offset: 0x001B71B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x001B8DCE File Offset: 0x001B71CE
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04001B96 RID: 7062
		private int startingAreaCount = -1;
	}
}
