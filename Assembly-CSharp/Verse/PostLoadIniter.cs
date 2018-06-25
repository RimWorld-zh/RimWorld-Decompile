using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D9B RID: 3483
	public class PostLoadIniter
	{
		// Token: 0x040033F0 RID: 13296
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0028A8B8 File Offset: 0x00288CB8
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

		// Token: 0x06004DDA RID: 19930 RVA: 0x0028A958 File Offset: 0x00288D58
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

		// Token: 0x06004DDB RID: 19931 RVA: 0x0028AA40 File Offset: 0x00288E40
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}
	}
}
