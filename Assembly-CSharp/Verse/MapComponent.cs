using System;

namespace Verse
{
	// Token: 0x02000C34 RID: 3124
	public abstract class MapComponent : IExposable
	{
		// Token: 0x04002F19 RID: 12057
		public Map map;

		// Token: 0x060044ED RID: 17645 RVA: 0x00152351 File Offset: 0x00150751
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00152361 File Offset: 0x00150761
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x00152364 File Offset: 0x00150764
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x00152367 File Offset: 0x00150767
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0015236A File Offset: 0x0015076A
		public virtual void ExposeData()
		{
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x0015236D File Offset: 0x0015076D
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00152370 File Offset: 0x00150770
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00152373 File Offset: 0x00150773
		public virtual void MapRemoved()
		{
		}
	}
}
