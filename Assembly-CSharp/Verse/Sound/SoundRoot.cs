using System;

namespace Verse.Sound
{
	// Token: 0x02000DB7 RID: 3511
	public class SoundRoot
	{
		// Token: 0x04003445 RID: 13381
		public AudioSourcePool sourcePool;

		// Token: 0x04003446 RID: 13382
		public SampleOneShotManager oneShotManager;

		// Token: 0x04003447 RID: 13383
		public SustainerManager sustainerManager;

		// Token: 0x06004E81 RID: 20097 RVA: 0x00290B94 File Offset: 0x0028EF94
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00290BBE File Offset: 0x0028EFBE
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}
	}
}
