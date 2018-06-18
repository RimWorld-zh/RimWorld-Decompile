using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D9C RID: 3484
	public class PostLoadIniter
	{
		// Token: 0x06004DC0 RID: 19904 RVA: 0x002891DC File Offset: 0x002875DC
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for post load init, but current mode is ",
					Scribe.mode
				}), false);
			}
			else if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.", false);
			}
			else if (this.saveablesToPostLoad.Contains(s))
			{
				Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s, false);
			}
			else
			{
				this.saveablesToPostLoad.Add(s);
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0028927C File Offset: 0x0028767C
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not do PostLoadInit on ",
						exposable.ToStringSafe<IExposable>(),
						": ",
						ex
					}), false);
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00289364 File Offset: 0x00287764
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x040033E5 RID: 13285
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
