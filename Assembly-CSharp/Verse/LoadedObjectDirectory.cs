using System;
using System.Collections.Generic;

namespace Verse
{
	public class LoadedObjectDirectory
	{
		private Dictionary<string, ILoadReferenceable> allObjectsByLoadID = new Dictionary<string, ILoadReferenceable>();

		public void Clear()
		{
			this.allObjectsByLoadID.Clear();
		}

		public void RegisterLoaded(ILoadReferenceable reffable)
		{
			if (Prefs.DevMode)
			{
				string text = "[excepted]";
				try
				{
					text = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text2 = "[excepted]";
				try
				{
					text2 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				ILoadReferenceable loadReferenceable = default(ILoadReferenceable);
				if (this.allObjectsByLoadID.TryGetValue(reffable.GetUniqueLoadID(), out loadReferenceable))
				{
					Log.Error("Cannot register " + reffable.GetType() + " " + text2 + ", (id=" + text + " in loaded object directory. Id already used by " + loadReferenceable.GetType() + " " + loadReferenceable.ToString() + ".");
					return;
				}
			}
			try
			{
				this.allObjectsByLoadID.Add(reffable.GetUniqueLoadID(), reffable);
			}
			catch (Exception ex5)
			{
				string text3 = "[excepted]";
				try
				{
					text3 = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text4 = "[excepted]";
				try
				{
					text4 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				Log.Error("Exception registering " + reffable.GetType() + " " + text4 + " in loaded object directory with unique load ID " + text3 + ": " + ex5);
			}
		}

		public T ObjectWithLoadID<T>(string loadID)
		{
			if (!loadID.NullOrEmpty() && !(loadID == "null"))
			{
				ILoadReferenceable loadReferenceable = default(ILoadReferenceable);
				if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
				{
					if (loadReferenceable == null)
					{
						return default(T);
					}
					try
					{
						return (T)loadReferenceable;
						IL_0055:;
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting object with load id " + loadID + " of type " + typeof(T) + ". What we loaded was " + loadReferenceable.ToStringSafe<ILoadReferenceable>() + ". Exception:\n" + ex);
						return default(T);
						IL_00ba:;
					}
				}
				Log.Warning("Could not resolve reference to object with loadID " + loadID + " of type " + typeof(T) + ". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent);
				return default(T);
			}
			return default(T);
		}
	}
}
