using System;

namespace Verse
{
	// Token: 0x02000BD7 RID: 3031
	public abstract class Entity
	{
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06004208 RID: 16904
		public abstract string LabelCap { get; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06004209 RID: 16905
		public abstract string Label { get; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x00125474 File Offset: 0x00123874
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x00125490 File Offset: 0x00123890
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x001254AC File Offset: 0x001238AC
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x0600420D RID: 16909
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x0600420E RID: 16910
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x0600420F RID: 16911 RVA: 0x001254CC File Offset: 0x001238CC
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x001254D4 File Offset: 0x001238D4
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x001254DC File Offset: 0x001238DC
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x001254E4 File Offset: 0x001238E4
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
