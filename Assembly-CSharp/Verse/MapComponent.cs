using System;

namespace Verse
{
	// Token: 0x02000C37 RID: 3127
	public abstract class MapComponent : IExposable
	{
		// Token: 0x060044E4 RID: 17636 RVA: 0x001521AD File Offset: 0x001505AD
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x001521BD File Offset: 0x001505BD
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x001521C0 File Offset: 0x001505C0
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x001521C3 File Offset: 0x001505C3
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x001521C6 File Offset: 0x001505C6
		public virtual void ExposeData()
		{
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x001521C9 File Offset: 0x001505C9
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x001521CC File Offset: 0x001505CC
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x001521CF File Offset: 0x001505CF
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04002F0F RID: 12047
		public Map map;
	}
}
