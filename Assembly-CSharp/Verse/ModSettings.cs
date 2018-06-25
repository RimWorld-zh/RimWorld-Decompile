using System;

namespace Verse
{
	// Token: 0x02000CCA RID: 3274
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x00261260 File Offset: 0x0025F660
		// (set) Token: 0x06004869 RID: 18537 RVA: 0x0026127A File Offset: 0x0025F67A
		public Mod Mod { get; internal set; }

		// Token: 0x0600486A RID: 18538 RVA: 0x00261283 File Offset: 0x0025F683
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00261286 File Offset: 0x0025F686
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
