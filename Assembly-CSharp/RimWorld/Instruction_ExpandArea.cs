using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003349 RID: 13129
		protected abstract Area MyArea { get; }

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x001B8B7C File Offset: 0x001B6F7C
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x001B8BB1 File Offset: 0x001B6FB1
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x001B8BCB File Offset: 0x001B6FCB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x001B8BE6 File Offset: 0x001B6FE6
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
