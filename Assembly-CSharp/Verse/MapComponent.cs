using System;

namespace Verse
{
	// Token: 0x02000C36 RID: 3126
	public abstract class MapComponent : IExposable
	{
		// Token: 0x04002F19 RID: 12057
		public Map map;

		// Token: 0x060044F0 RID: 17648 RVA: 0x001524A1 File Offset: 0x001508A1
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x001524B1 File Offset: 0x001508B1
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x001524B4 File Offset: 0x001508B4
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x001524B7 File Offset: 0x001508B7
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x001524BA File Offset: 0x001508BA
		public virtual void ExposeData()
		{
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x001524BD File Offset: 0x001508BD
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001524C0 File Offset: 0x001508C0
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x001524C3 File Offset: 0x001508C3
		public virtual void MapRemoved()
		{
		}
	}
}
