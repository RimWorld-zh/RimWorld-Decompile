using System;

namespace Verse
{
	// Token: 0x02000CC7 RID: 3271
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x00260EA4 File Offset: 0x0025F2A4
		// (set) Token: 0x06004866 RID: 18534 RVA: 0x00260EBE File Offset: 0x0025F2BE
		public Mod Mod { get; internal set; }

		// Token: 0x06004867 RID: 18535 RVA: 0x00260EC7 File Offset: 0x0025F2C7
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x00260ECA File Offset: 0x0025F2CA
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
