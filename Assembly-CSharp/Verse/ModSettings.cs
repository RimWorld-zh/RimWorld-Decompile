using System;

namespace Verse
{
	// Token: 0x02000CCB RID: 3275
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x0025FAB4 File Offset: 0x0025DEB4
		// (set) Token: 0x06004857 RID: 18519 RVA: 0x0025FACE File Offset: 0x0025DECE
		public Mod Mod { get; internal set; }

		// Token: 0x06004858 RID: 18520 RVA: 0x0025FAD7 File Offset: 0x0025DED7
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0025FADA File Offset: 0x0025DEDA
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
