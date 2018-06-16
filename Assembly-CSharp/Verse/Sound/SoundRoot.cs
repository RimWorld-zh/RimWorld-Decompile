using System;

namespace Verse.Sound
{
	// Token: 0x02000DB8 RID: 3512
	public class SoundRoot
	{
		// Token: 0x06004E6A RID: 20074 RVA: 0x0028F1F8 File Offset: 0x0028D5F8
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x0028F222 File Offset: 0x0028D622
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04003435 RID: 13365
		public AudioSourcePool sourcePool;

		// Token: 0x04003436 RID: 13366
		public SampleOneShotManager oneShotManager;

		// Token: 0x04003437 RID: 13367
		public SustainerManager sustainerManager;
	}
}
