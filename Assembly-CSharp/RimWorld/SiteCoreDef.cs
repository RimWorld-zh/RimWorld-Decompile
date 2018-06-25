using System;

namespace RimWorld
{
	// Token: 0x020002D0 RID: 720
	public class SiteCoreDef : SiteDefBase
	{
		// Token: 0x0400071F RID: 1823
		public bool transportPodsCanLandAndGenerateMap = true;

		// Token: 0x04000720 RID: 1824
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;

		// Token: 0x06000BDF RID: 3039 RVA: 0x00069F1E File Offset: 0x0006831E
		public SiteCoreDef()
		{
			this.workerClass = typeof(SiteCoreWorker);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00069F4C File Offset: 0x0006834C
		public new SiteCoreWorker Worker
		{
			get
			{
				return (SiteCoreWorker)base.Worker;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00069F6C File Offset: 0x0006836C
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00069F9C File Offset: 0x0006839C
		protected override SiteWorkerBase CreateWorker()
		{
			return (SiteCoreWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
