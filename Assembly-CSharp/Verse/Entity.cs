using System;

namespace Verse
{
	// Token: 0x02000BD6 RID: 3030
	public abstract class Entity
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600420F RID: 16911
		public abstract string LabelCap { get; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06004210 RID: 16912
		public abstract string Label { get; }

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x001259DC File Offset: 0x00123DDC
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x001259F8 File Offset: 0x00123DF8
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x00125A14 File Offset: 0x00123E14
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06004214 RID: 16916
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06004215 RID: 16917
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06004216 RID: 16918 RVA: 0x00125A34 File Offset: 0x00123E34
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00125A3C File Offset: 0x00123E3C
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x00125A44 File Offset: 0x00123E44
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00125A4C File Offset: 0x00123E4C
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
