using System;

namespace RimWorld
{
	// Token: 0x020002CE RID: 718
	public class SiteCoreDef : SiteDefBase
	{
		// Token: 0x06000BDB RID: 3035 RVA: 0x00069DCE File Offset: 0x000681CE
		public SiteCoreDef()
		{
			this.workerClass = typeof(SiteCoreWorker);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00069DFC File Offset: 0x000681FC
		public new SiteCoreWorker Worker
		{
			get
			{
				return (SiteCoreWorker)base.Worker;
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00069E1C File Offset: 0x0006821C
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00069E4C File Offset: 0x0006824C
		protected override SiteWorkerBase CreateWorker()
		{
			return (SiteCoreWorker)Activator.CreateInstance(this.workerClass);
		}

		// Token: 0x0400071F RID: 1823
		public bool transportPodsCanLandAndGenerateMap = true;

		// Token: 0x04000720 RID: 1824
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;
	}
}
