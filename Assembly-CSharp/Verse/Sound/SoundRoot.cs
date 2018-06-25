using System;

namespace Verse.Sound
{
	// Token: 0x02000DB6 RID: 3510
	public class SoundRoot
	{
		// Token: 0x0400343E RID: 13374
		public AudioSourcePool sourcePool;

		// Token: 0x0400343F RID: 13375
		public SampleOneShotManager oneShotManager;

		// Token: 0x04003440 RID: 13376
		public SustainerManager sustainerManager;

		// Token: 0x06004E81 RID: 20097 RVA: 0x002908B4 File Offset: 0x0028ECB4
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x002908DE File Offset: 0x0028ECDE
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}
	}
}
