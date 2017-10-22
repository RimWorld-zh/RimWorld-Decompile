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
				if (this.allObjectsByLoadID.TryGetValue(text, out loadReferenceable))
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
			T result;
			if (loadID.NullOrEmpty() || loadID == "null")
			{
				result = default(T);
			}
			else
			{
				ILoadReferenceable loadReferenceable = default(ILoadReferenceable);
				if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
				{
					if (loadReferenceable == null)
					{
						result = default(T);
						goto IL_0131;
					}
					try
					{
						return (T)loadReferenceable;
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting object with load id " + loadID + " of type " + typeof(T) + ". What we loaded was " + loadReferenceable.ToStringSafe<ILoadReferenceable>() + ". Exception:\n" + ex);
						return default(T);
					}
				}
				Log.Warning("Could not resolve reference to object with loadID " + loadID + " of type " + typeof(T) + ". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent);
				result = default(T);
			}
			goto IL_0131;
			IL_0131:
			return result;
		}
	}
}
