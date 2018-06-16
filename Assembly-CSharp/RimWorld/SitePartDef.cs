using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D2 RID: 722
	public class SitePartDef : SiteDefBase
	{
		// Token: 0x06000BF4 RID: 3060 RVA: 0x0006A495 File Offset: 0x00068895
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x0006A4B0 File Offset: 0x000688B0
		public new SitePartWorker Worker
		{
			get
			{
				return (SitePartWorker)base.Worker;
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0006A4D0 File Offset: 0x000688D0
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0006A500 File Offset: 0x00068900
		protected override SiteWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}

		// Token: 0x04000734 RID: 1844
		[MustTranslate]
		public string descriptionDialogue;
	}
}
