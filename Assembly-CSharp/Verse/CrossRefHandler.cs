using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D94 RID: 3476
	public class CrossRefHandler
	{
		// Token: 0x040033E3 RID: 13283
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x040033E4 RID: 13284
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x040033E5 RID: 13285
		private List<IExposable> crossReferencingExposables = new List<IExposable>();

		// Token: 0x06004DBE RID: 19902 RVA: 0x00289A40 File Offset: 0x00287E40
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

		// Token: 0x06004DBF RID: 19903 RVA: 0x00289ADC File Offset: 0x00287EDC
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

		// Token: 0x06004DC0 RID: 19904 RVA: 0x00289C74 File Offset: 0x00288074
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x00289CA8 File Offset: 0x002880A8
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00289CF4 File Offset: 0x002880F4
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

		// Token: 0x06004DC3 RID: 19907 RVA: 0x00289D58 File Offset: 0x00288158
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x00289DA3 File Offset: 0x002881A3
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
	}
}
