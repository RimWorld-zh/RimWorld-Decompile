using System;

namespace Verse
{
	// Token: 0x02000BD3 RID: 3027
	public abstract class Entity
	{
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600420C RID: 16908
		public abstract string LabelCap { get; }

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x0600420D RID: 16909
		public abstract string Label { get; }

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x00125624 File Offset: 0x00123A24
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x00125640 File Offset: 0x00123A40
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x0012565C File Offset: 0x00123A5C
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06004211 RID: 16913
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06004212 RID: 16914
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06004213 RID: 16915 RVA: 0x0012567C File Offset: 0x00123A7C
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00125684 File Offset: 0x00123A84
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0012568C File Offset: 0x00123A8C
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00125694 File Offset: 0x00123A94
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
