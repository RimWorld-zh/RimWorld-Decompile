using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000DA9 RID: 3497
	public static class Scribe_References
	{
		// Token: 0x06004E21 RID: 20001 RVA: 0x0028E2F4 File Offset: 0x0028C6F4
		public static void Look<T>(ref T refee, string label, bool saveDestroyedThings = false) where T : ILoadReferenceable
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (refee == null)
				{
					Scribe.saver.WriteElement(label, "null");
				}
				else
				{
					Thing thing = refee as Thing;
					if (thing != null)
					{
						if (Scribe_References.CheckSaveReferenceToDestroyedThing(thing, label, saveDestroyedThings))
						{
							return;
						}
					}
					if (UnityData.isDebugBuild)
					{
						if (thing != null)
						{
							if (!thing.def.HasThingIDNumber)
							{
								Log.Error("Trying to cross-reference save Thing which lacks ID number: " + refee, false);
								Scribe.saver.WriteElement(label, "null");
								return;
							}
							if (thing.IsSaveCompressible())
							{
								Log.Error("Trying to save a reference to a thing that will be compressed away: " + refee, false);
								Scribe.saver.WriteElement(label, "null");
								return;
							}
						}
					}
					string uniqueLoadID = refee.GetUniqueLoadID();
					Scribe.saver.WriteElement(label, uniqueLoadID);
					Scribe.saver.loadIDsErrorsChecker.RegisterReferenced(refee, label);
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (Scribe.loader.curParent != null && Scribe.loader.curParent.GetType().IsValueType)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Trying to load reference of an object of type ",
						typeof(T),
						" with label ",
						label,
						", but our current node is a value type. The reference won't be loaded properly. curParent=",
						Scribe.loader.curParent
					}), false);
				}
				XmlNode xmlNode = Scribe.loader.curXmlParent[label];
				string targetLoadID;
				if (xmlNode != null)
				{
					targetLoadID = xmlNode.InnerText;
				}
				else
				{
					targetLoadID = null;
				}
				Scribe.loader.crossRefs.loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(T), label);
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				refee = Scribe.loader.crossRefs.TakeResolvedRef<T>(label);
			}
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0028E514 File Offset: 0x0028C914
		public static void Look<T>(ref WeakReference<T> refee, string label, bool saveDestroyedThings = false) where T : class, ILoadReferenceable
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				T t = (refee == null) ? ((T)((object)null)) : refee.Target;
				Scribe_References.Look<T>(ref t, label, saveDestroyedThings);
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				T t2 = (T)((object)null);
				Scribe_References.Look<T>(ref t2, label, saveDestroyedThings);
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				T t3 = (T)((object)null);
				Scribe_References.Look<T>(ref t3, label, saveDestroyedThings);
				if (t3 != null)
				{
					refee = new WeakReference<T>(t3);
				}
			}
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0028E5AC File Offset: 0x0028C9AC
		public static bool CheckSaveReferenceToDestroyedThing(Thing th, string label, bool saveDestroyedThings)
		{
			bool result;
			if (!th.Destroyed)
			{
				result = false;
			}
			else if (!saveDestroyedThings)
			{
				Scribe.saver.WriteElement(label, "null");
				result = true;
			}
			else if (th.Discarded)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Trying to save reference to a discarded thing ",
					th,
					" with saveDestroyedThings=true. This means that it's not deep-saved anywhere and is no longer managed by anything in the code, so saving its reference will always fail. , label=",
					label
				}), false);
				Scribe.saver.WriteElement(label, "null");
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
