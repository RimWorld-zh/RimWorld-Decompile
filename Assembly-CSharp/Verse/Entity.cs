using System;

namespace Verse
{
	// Token: 0x02000BD7 RID: 3031
	public abstract class Entity
	{
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600420A RID: 16906
		public abstract string LabelCap { get; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600420B RID: 16907
		public abstract string Label { get; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x001254EC File Offset: 0x001238EC
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x00125508 File Offset: 0x00123908
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x00125524 File Offset: 0x00123924
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x0600420F RID: 16911
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06004210 RID: 16912
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06004211 RID: 16913 RVA: 0x00125544 File Offset: 0x00123944
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x0012554C File Offset: 0x0012394C
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00125554 File Offset: 0x00123954
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0012555C File Offset: 0x0012395C
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
