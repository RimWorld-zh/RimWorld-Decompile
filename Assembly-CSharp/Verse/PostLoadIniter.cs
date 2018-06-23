using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D99 RID: 3481
	public class PostLoadIniter
	{
		// Token: 0x040033F0 RID: 13296
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0028A78C File Offset: 0x00288B8C
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

		// Token: 0x06004DD6 RID: 19926 RVA: 0x0028A82C File Offset: 0x00288C2C
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

		// Token: 0x06004DD7 RID: 19927 RVA: 0x0028A914 File Offset: 0x00288D14
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}
	}
}
