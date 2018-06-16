using System;

namespace Verse
{
	// Token: 0x02000C38 RID: 3128
	public abstract class MapComponent : IExposable
	{
		// Token: 0x060044E6 RID: 17638 RVA: 0x00152135 File Offset: 0x00150535
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00152145 File Offset: 0x00150545
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00152148 File Offset: 0x00150548
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x0015214B File Offset: 0x0015054B
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0015214E File Offset: 0x0015054E
		public virtual void ExposeData()
		{
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x00152151 File Offset: 0x00150551
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x00152154 File Offset: 0x00150554
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x00152157 File Offset: 0x00150557
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04002F11 RID: 12049
		public Map map;
	}
}
