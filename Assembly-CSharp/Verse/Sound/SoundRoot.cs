using System;

namespace Verse.Sound
{
	// Token: 0x02000DB4 RID: 3508
	public class SoundRoot
	{
		// Token: 0x0400343E RID: 13374
		public AudioSourcePool sourcePool;

		// Token: 0x0400343F RID: 13375
		public SampleOneShotManager oneShotManager;

		// Token: 0x04003440 RID: 13376
		public SustainerManager sustainerManager;

		// Token: 0x06004E7D RID: 20093 RVA: 0x00290788 File Offset: 0x0028EB88
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x002907B2 File Offset: 0x0028EBB2
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}
	}
}
