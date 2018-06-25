using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Archive : IExposable
	{
		private List<IArchivable> archivables = new List<IArchivable>();

		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		public const int MaxNonPinnedArchivables = 200;

		[CompilerGenerated]
		private static Predicate<IArchivable> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<IArchivable> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IArchivable, int> <>f__am$cache2;

		public Archive()
		{
		}

		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

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

		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

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

		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

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

		[CompilerGenerated]
		private static bool <ExposeData>m__0(IArchivable x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(IArchivable x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private static int <Add>m__2(IArchivable x)
		{
			return x.CreatedTicksGame;
		}
	}
}
