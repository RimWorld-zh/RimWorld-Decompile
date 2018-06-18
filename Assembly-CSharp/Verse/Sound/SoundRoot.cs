using System;

namespace Verse.Sound
{
	// Token: 0x02000DB7 RID: 3511
	public class SoundRoot
	{
		// Token: 0x06004E68 RID: 20072 RVA: 0x0028F1D8 File Offset: 0x0028D5D8
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0028F202 File Offset: 0x0028D602
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04003433 RID: 13363
		public AudioSourcePool sourcePool;

		// Token: 0x04003434 RID: 13364
		public SampleOneShotManager oneShotManager;

		// Token: 0x04003435 RID: 13365
		public SustainerManager sustainerManager;
	}
}
