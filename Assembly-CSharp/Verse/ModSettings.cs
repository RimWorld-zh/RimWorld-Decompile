using System;

namespace Verse
{
	// Token: 0x02000CC9 RID: 3273
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x00260F80 File Offset: 0x0025F380
		// (set) Token: 0x06004869 RID: 18537 RVA: 0x00260F9A File Offset: 0x0025F39A
		public Mod Mod { get; internal set; }

		// Token: 0x0600486A RID: 18538 RVA: 0x00260FA3 File Offset: 0x0025F3A3
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00260FA6 File Offset: 0x0025F3A6
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
