using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	public static class DirectXmlCrossRefLoader
	{
		private abstract class WantedRef
		{
			public object wanter;

			public abstract bool TryResolve(FailMode failReportMode);
		}

		private class WantedRefForObject : WantedRef
		{
			public FieldInfo fi;

			public string defName;

			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName)
			{
				base.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
			}

			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName);
					return false;
				}
				Def defSilentFail = GenDefDatabase.GetDefSilentFail(this.fi.FieldType, this.defName);
				if (defSilentFail == null)
				{
					if (failReportMode == FailMode.LogErrors)
					{
						Log.Error("Could not resolve cross-reference: No " + this.fi.FieldType + " named " + this.defName + " found to give to " + base.wanter.GetType() + " " + base.wanter);
					}
					return false;
				}
				SoundDef soundDef = defSilentFail as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					Log.Warning("Could not resolve cross-reference: No " + this.fi.FieldType + " named " + this.defName + " found to give to " + base.wanter.GetType() + " " + base.wanter + " (using undefined sound instead)");
				}
				this.fi.SetValue(base.wanter, defSilentFail);
				return true;
			}
		}

		private class WantedRefForList<T> : WantedRef where T : new()
		{
			private List<string> defNames = new List<string>();

			public WantedRefForList(object wanter)
			{
				base.wanter = wanter;
			}

			public void AddWantedListEntry(string newTargetDefName)
			{
				this.defNames.Add(newTargetDefName);
			}

			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					T val = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], failReportMode);
					if (val != null)
					{
						((List<T>)base.wanter).Add(val);
						this.defNames.RemoveAt(i);
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}
		}

		private class WantedRefForDictionary<K, V> : WantedRef where K : new() where V : new()
		{
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			public WantedRefForDictionary(object wanter)
			{
				base.wanter = wanter;
			}

			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				List<Pair<K, V>> list = new List<Pair<K, V>>();
				List<XmlNode>.Enumerator enumerator = this.wantedDictRefs.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						XmlNode current = enumerator.Current;
						XmlNode xmlNode = current["key"];
						XmlNode xmlNode2 = current["value"];
						K first = (!flag) ? DirectXmlToObject.ObjectFromXml<K>(xmlNode, true) : DirectXmlCrossRefLoader.TryResolveDef<K>(xmlNode.InnerText, failReportMode);
						V second = (!flag2) ? DirectXmlToObject.ObjectFromXml<V>(xmlNode2, true) : DirectXmlCrossRefLoader.TryResolveDef<V>(xmlNode2.InnerText, failReportMode);
						list.Add(new Pair<K, V>(first, second));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				Dictionary<K, V> dictionary = (Dictionary<K, V>)base.wanter;
				dictionary.Clear();
				List<Pair<K, V>>.Enumerator enumerator2 = list.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Pair<K, V> current2 = enumerator2.Current;
						try
						{
							dictionary.Add(current2.First, current2.Second);
						}
						catch
						{
							Log.Error("Failed to load key/value pair: " + current2.First + ", " + current2.Second);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				return true;
			}
		}

		private static List<WantedRef> wantedRefs = new List<WantedRef>();

		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName)
		{
			WantedRefForObject item = new WantedRefForObject(wanter, fi, targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName)
		{
			WantedRefForObject item = new WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName) where T : new()
		{
			WantedRefForList<T> wantedRefForList = null;
			List<WantedRef>.Enumerator enumerator = DirectXmlCrossRefLoader.wantedRefs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WantedRef current = enumerator.Current;
					if (current.wanter == wanterList)
					{
						wantedRefForList = (WantedRefForList<T>)current;
						break;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (wantedRefForList == null)
			{
				wantedRefForList = new WantedRefForList<T>((object)wanterList);
				DirectXmlCrossRefLoader.wantedRefs.Add((WantedRef)wantedRefForList);
			}
			wantedRefForList.AddWantedListEntry(targetDefName);
		}

		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode) where K : new() where V : new()
		{
			WantedRefForDictionary<K, V> wantedRefForDictionary = null;
			List<WantedRef>.Enumerator enumerator = DirectXmlCrossRefLoader.wantedRefs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WantedRef current = enumerator.Current;
					if (current.wanter == wanterDict)
					{
						wantedRefForDictionary = (WantedRefForDictionary<K, V>)current;
						break;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (wantedRefForDictionary == null)
			{
				wantedRefForDictionary = new WantedRefForDictionary<K, V>((object)wanterDict);
				DirectXmlCrossRefLoader.wantedRefs.Add((WantedRef)wantedRefForDictionary);
			}
			wantedRefForDictionary.AddWantedDictEntry(entryNode);
		}

		public static T TryResolveDef<T>(string defName, FailMode failReportMode)
		{
			T val = (T)(object)GenDefDatabase.GetDefSilentFail(typeof(T), defName);
			if (val != null)
			{
				return val;
			}
			if (failReportMode == FailMode.LogErrors)
			{
				Log.Error("Could not resolve cross-reference to " + typeof(T) + " named " + defName);
			}
			return default(T);
		}

		public static void Clear()
		{
			DirectXmlCrossRefLoader.wantedRefs.Clear();
		}

		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			List<WantedRef>.Enumerator enumerator = DirectXmlCrossRefLoader.wantedRefs.ListFullCopy().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WantedRef current = enumerator.Current;
					if (current.TryResolve(failReportMode))
					{
						DirectXmlCrossRefLoader.wantedRefs.Remove(current);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
