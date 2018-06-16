using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D98 RID: 3480
	public class CrossRefHandler
	{
		// Token: 0x06004DAB RID: 19883 RVA: 0x002884B0 File Offset: 0x002868B0
		public void RegisterForCrossRefResolve(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for cross ref resolve, but current mode is ",
					Scribe.mode
				}), false);
			}
			else if (s != null)
			{
				if (DebugViewSettings.logMapLoad)
				{
					LogSimple.Message("RegisterForCrossRefResolve " + ((s == null) ? "null" : s.GetType().ToString()));
				}
				this.crossReferencingExposables.Add(s);
			}
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0028854C File Offset: 0x0028694C
		public void ResolveAllCrossReferences()
		{
			Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Register the saveables all so we can find them later");
			}
			foreach (IExposable exposable in this.crossReferencingExposables)
			{
				ILoadReferenceable loadReferenceable = exposable as ILoadReferenceable;
				if (loadReferenceable != null)
				{
					if (DebugViewSettings.logMapLoad)
					{
						LogSimple.Message("RegisterLoaded " + loadReferenceable.GetType());
					}
					this.loadedObjectDirectory.RegisterLoaded(loadReferenceable);
				}
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Fill all cross-references to the saveables");
			}
			foreach (IExposable exposable2 in this.crossReferencingExposables)
			{
				if (DebugViewSettings.logMapLoad)
				{
					LogSimple.Message("ResolvingCrossRefs ExposeData " + exposable2.GetType());
				}
				try
				{
					Scribe.loader.curParent = exposable2;
					Scribe.loader.curPathRelToParent = null;
					exposable2.ExposeData();
				}
				catch (Exception arg)
				{
					Log.Error("Could not resolve cross refs: " + arg, false);
				}
			}
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
			this.Clear(true);
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x002886E4 File Offset: 0x00286AE4
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x00288718 File Offset: 0x00286B18
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x00288764 File Offset: 0x00286B64
		public List<T> TakeResolvedRefList<T>(string pathRelToParent, IExposable parent)
		{
			List<string> list = this.loadIDs.TakeList(pathRelToParent, parent);
			List<T> list2 = new List<T>();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(this.loadedObjectDirectory.ObjectWithLoadID<T>(list[i]));
				}
			}
			return list2;
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x002887C8 File Offset: 0x00286BC8
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x00288813 File Offset: 0x00286C13
		public void Clear(bool errorIfNotEmpty)
		{
			if (errorIfNotEmpty)
			{
				this.loadIDs.ConfirmClear();
			}
			else
			{
				this.loadIDs.Clear();
			}
			this.crossReferencingExposables.Clear();
			this.loadedObjectDirectory.Clear();
		}

		// Token: 0x040033DA RID: 13274
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x040033DB RID: 13275
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x040033DC RID: 13276
		private List<IExposable> crossReferencingExposables = new List<IExposable>();
	}
}
