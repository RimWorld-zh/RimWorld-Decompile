using System;
using System.Collections.Generic;

namespace Verse
{
	public class PostLoadIniter
	{
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();

		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Registered " + s + " for post load init, but current mode is " + Scribe.mode);
			}
			else if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.");
			}
			else if (this.saveablesToPostLoad.Contains(s))
			{
				Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s);
			}
			else
			{
				this.saveablesToPostLoad.Add(s);
			}
		}

		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			HashSet<IExposable>.Enumerator enumerator = this.saveablesToPostLoad.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IExposable current = enumerator.Current;
					try
					{
						Scribe.loader.curParent = current;
						Scribe.loader.curPathRelToParent = (string)null;
						current.ExposeData();
					}
					catch (Exception arg)
					{
						Log.Error("Could not do PostLoadInit: " + arg);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = (string)null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}
	}
}
