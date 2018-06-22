using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D98 RID: 3480
	public class LoadedObjectDirectory
	{
		// Token: 0x06004DD1 RID: 19921 RVA: 0x0028A45D File Offset: 0x0028885D
		public void Clear()
		{
			this.allObjectsByLoadID.Clear();
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0028A46C File Offset: 0x0028886C
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
				ILoadReferenceable loadReferenceable;
				if (this.allObjectsByLoadID.TryGetValue(text, out loadReferenceable))
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot register ",
						reffable.GetType(),
						" ",
						text2,
						", (id=",
						text,
						" in loaded object directory. Id already used by ",
						loadReferenceable.GetType(),
						" ",
						loadReferenceable.ToString(),
						"."
					}), false);
					return;
				}
			}
			try
			{
				this.allObjectsByLoadID.Add(reffable.GetUniqueLoadID(), reffable);
			}
			catch (Exception ex)
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
				Log.Error(string.Concat(new object[]
				{
					"Exception registering ",
					reffable.GetType(),
					" ",
					text4,
					" in loaded object directory with unique load ID ",
					text3,
					": ",
					ex
				}), false);
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0028A624 File Offset: 0x00288A24
		public T ObjectWithLoadID<T>(string loadID)
		{
			T result;
			if (loadID.NullOrEmpty() || loadID == "null")
			{
				result = default(T);
			}
			else
			{
				ILoadReferenceable loadReferenceable;
				if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
				{
					if (loadReferenceable == null)
					{
						return default(T);
					}
					try
					{
						return (T)((object)loadReferenceable);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception getting object with load id ",
							loadID,
							" of type ",
							typeof(T),
							". What we loaded was ",
							loadReferenceable.ToStringSafe<ILoadReferenceable>(),
							". Exception:\n",
							ex
						}), false);
						return default(T);
					}
				}
				Log.Warning(string.Concat(new object[]
				{
					"Could not resolve reference to object with loadID ",
					loadID,
					" of type ",
					typeof(T),
					". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=",
					Scribe.loader.curParent.ToStringSafe<IExposable>(),
					" curPathRelToParent=",
					Scribe.loader.curPathRelToParent
				}), false);
				result = default(T);
			}
			return result;
		}

		// Token: 0x040033EF RID: 13295
		private Dictionary<string, ILoadReferenceable> allObjectsByLoadID = new Dictionary<string, ILoadReferenceable>();
	}
}
