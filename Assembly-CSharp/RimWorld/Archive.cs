using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F5 RID: 757
	public class Archive : IExposable
	{
		// Token: 0x04000832 RID: 2098
		private List<IArchivable> archivables = new List<IArchivable>();

		// Token: 0x04000833 RID: 2099
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		// Token: 0x04000834 RID: 2100
		public const int MaxNonPinnedArchivables = 200;

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0006F2F8 File Offset: 0x0006D6F8
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0006F314 File Offset: 0x0006D714
		public void ExposeData()
		{
			Scribe_Collections.Look<IArchivable>(ref this.archivables, "archivables", LookMode.Deep, new object[0]);
			Scribe_Collections.Look<IArchivable>(ref this.pinnedArchivables, "pinnedArchivables", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.archivables.RemoveAll((IArchivable x) => x == null);
				this.pinnedArchivables.RemoveWhere((IArchivable x) => x == null);
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0006F3AC File Offset: 0x0006D7AC
		public bool Add(IArchivable archivable)
		{
			bool result;
			if (archivable == null)
			{
				Log.Error("Tried to add null archivable.", false);
				result = false;
			}
			else if (this.Contains(archivable))
			{
				result = false;
			}
			else
			{
				this.archivables.Add(archivable);
				this.archivables.SortBy((IArchivable x) => x.CreatedTicksGame);
				this.CheckCullArchivables();
				result = true;
			}
			return result;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0006F428 File Offset: 0x0006D828
		public bool Remove(IArchivable archivable)
		{
			bool result;
			if (!this.Contains(archivable))
			{
				result = false;
			}
			else
			{
				this.archivables.Remove(archivable);
				this.pinnedArchivables.Remove(archivable);
				result = true;
			}
			return result;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0006F46C File Offset: 0x0006D86C
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0006F48D File Offset: 0x0006D88D
		public void Pin(IArchivable archivable)
		{
			if (this.Contains(archivable))
			{
				if (!this.IsPinned(archivable))
				{
					this.pinnedArchivables.Add(archivable);
				}
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0006F4BF File Offset: 0x0006D8BF
		public void Unpin(IArchivable archivable)
		{
			if (this.Contains(archivable))
			{
				if (this.IsPinned(archivable))
				{
					this.pinnedArchivables.Remove(archivable);
				}
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0006F4F4 File Offset: 0x0006D8F4
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0006F518 File Offset: 0x0006D918
		private void CheckCullArchivables()
		{
			int num = 0;
			for (int i = 0; i < this.archivables.Count; i++)
			{
				if (!this.IsPinned(this.archivables[i]) && this.archivables[i].CanCullArchivedNow)
				{
					num++;
				}
			}
			int num2 = num - 200;
			for (int j = 0; j < this.archivables.Count; j++)
			{
				if (num2 <= 0)
				{
					break;
				}
				if (!this.IsPinned(this.archivables[j]) && this.archivables[j].CanCullArchivedNow)
				{
					if (this.Remove(this.archivables[j]))
					{
						num2--;
						j--;
					}
				}
			}
		}
	}
}
