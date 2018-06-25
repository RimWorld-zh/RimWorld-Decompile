using System;

namespace Verse
{
	// Token: 0x02000C37 RID: 3127
	public abstract class MapComponent : IExposable
	{
		// Token: 0x04002F20 RID: 12064
		public Map map;

		// Token: 0x060044F0 RID: 17648 RVA: 0x00152701 File Offset: 0x00150B01
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x00152711 File Offset: 0x00150B11
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x00152714 File Offset: 0x00150B14
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00152717 File Offset: 0x00150B17
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x0015271A File Offset: 0x00150B1A
		public virtual void ExposeData()
		{
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0015271D File Offset: 0x00150B1D
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x00152720 File Offset: 0x00150B20
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00152723 File Offset: 0x00150B23
		public virtual void MapRemoved()
		{
		}
	}
}
