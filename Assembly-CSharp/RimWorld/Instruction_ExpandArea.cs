using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003347 RID: 13127
		protected abstract Area MyArea { get; }

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x001B8AB4 File Offset: 0x001B6EB4
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x001B8AE9 File Offset: 0x001B6EE9
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x001B8B03 File Offset: 0x001B6F03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x001B8B1E File Offset: 0x001B6F1E
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04001B98 RID: 7064
		private int startingAreaCount = -1;
	}
}
