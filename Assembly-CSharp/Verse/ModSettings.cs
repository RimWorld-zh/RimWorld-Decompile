using System;

namespace Verse
{
	// Token: 0x02000CCA RID: 3274
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x0025FA8C File Offset: 0x0025DE8C
		// (set) Token: 0x06004855 RID: 18517 RVA: 0x0025FAA6 File Offset: 0x0025DEA6
		public Mod Mod { get; internal set; }

		// Token: 0x06004856 RID: 18518 RVA: 0x0025FAAF File Offset: 0x0025DEAF
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x0025FAB2 File Offset: 0x0025DEB2
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
