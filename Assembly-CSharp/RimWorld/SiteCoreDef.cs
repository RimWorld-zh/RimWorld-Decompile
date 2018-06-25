using System;

namespace RimWorld
{
	// Token: 0x020002D0 RID: 720
	public class SiteCoreDef : SiteDefBase
	{
		// Token: 0x04000721 RID: 1825
		public bool transportPodsCanLandAndGenerateMap = true;

		// Token: 0x04000722 RID: 1826
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;

		// Token: 0x06000BDE RID: 3038 RVA: 0x00069F1A File Offset: 0x0006831A
		public SiteCoreDef()
		{
			this.workerClass = typeof(SiteCoreWorker);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00069F48 File Offset: 0x00068348
		public new SiteCoreWorker Worker
		{
			get
			{
				return (SiteCoreWorker)base.Worker;
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00069F68 File Offset: 0x00068368
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00069F98 File Offset: 0x00068398
		protected override SiteWorkerBase CreateWorker()
		{
			return (SiteCoreWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
