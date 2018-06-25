using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D7B RID: 3451
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x0400339D RID: 13213
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004D59 RID: 19801 RVA: 0x00284AC8 File Offset: 0x00282EC8
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x00284AEC File Offset: 0x00282EEC
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName)
		{
			DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x00284B10 File Offset: 0x00282F10
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName)
		{
			DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x00284B40 File Offset: 0x00282F40
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null) where T : new()
		{
			DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList = null;
			foreach (DirectXmlCrossRefLoader.WantedRef wantedRef in DirectXmlCrossRefLoader.wantedRefs)
			{
				if (wantedRef.wanter == wanterList)
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
					break;
				}
			}
			if (wantedRefForList == null)
			{
				wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
				DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
			}
			wantedRefForList.AddWantedListEntry(targetDefName);
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x00284BD8 File Offset: 0x00282FD8
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null) where K : new() where V : new()
		{
			DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary = null;
			foreach (DirectXmlCrossRefLoader.WantedRef wantedRef in DirectXmlCrossRefLoader.wantedRefs)
			{
				if (wantedRef.wanter == wanterDict)
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
					break;
				}
			}
			if (wantedRefForDictionary == null)
			{
				wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
				DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
			}
			wantedRefForDictionary.AddWantedDictEntry(entryNode);
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x00284C70 File Offset: 0x00283070
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
			T result;
			if (t != null)
			{
				result = t;
			}
			else
			{
				if (failReportMode == FailMode.LogErrors)
				{
					string text = string.Concat(new object[]
					{
						"Could not resolve cross-reference to ",
						typeof(T),
						" named ",
						defName
					});
					if (debugWanterInfo != null)
					{
						text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
					}
					Log.Error(text, false);
				}
				result = default(T);
			}
			return result;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x00284D11 File Offset: 0x00283111
		public static void Clear()
		{
			DirectXmlCrossRefLoader.wantedRefs.Clear();
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00284D20 File Offset: 0x00283120
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			foreach (DirectXmlCrossRefLoader.WantedRef wantedRef in DirectXmlCrossRefLoader.wantedRefs.ListFullCopy<DirectXmlCrossRefLoader.WantedRef>())
			{
				if (wantedRef.TryResolve(failReportMode))
				{
					DirectXmlCrossRefLoader.wantedRefs.Remove(wantedRef);
				}
			}
		}

		// Token: 0x02000D7C RID: 3452
		private abstract class WantedRef
		{
			// Token: 0x0400339E RID: 13214
			public object wanter;

			// Token: 0x06004D63 RID: 19811
			public abstract bool TryResolve(FailMode failReportMode);
		}

		// Token: 0x02000D7D RID: 3453
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x0400339F RID: 13215
			public FieldInfo fi;

			// Token: 0x040033A0 RID: 13216
			public string defName;

			// Token: 0x06004D64 RID: 19812 RVA: 0x00284DAC File Offset: 0x002831AC
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
			}

			// Token: 0x06004D65 RID: 19813 RVA: 0x00284DCC File Offset: 0x002831CC
			public override bool TryResolve(FailMode failReportMode)
			{
				bool result;
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>(), false);
					result = false;
				}
				else
				{
					Def defSilentFail = GenDefDatabase.GetDefSilentFail(this.fi.FieldType, this.defName, true);
					if (defSilentFail == null)
					{
						if (failReportMode == FailMode.LogErrors)
						{
							Log.Error(string.Concat(new object[]
							{
								"Could not resolve cross-reference: No ",
								this.fi.FieldType,
								" named ",
								this.defName.ToStringSafe<string>(),
								" found to give to ",
								this.wanter.GetType(),
								" ",
								this.wanter.ToStringSafe<object>()
							}), false);
						}
						result = false;
					}
					else
					{
						SoundDef soundDef = defSilentFail as SoundDef;
						if (soundDef != null && soundDef.isUndefined)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Could not resolve cross-reference: No ",
								this.fi.FieldType,
								" named ",
								this.defName.ToStringSafe<string>(),
								" found to give to ",
								this.wanter.GetType(),
								" ",
								this.wanter.ToStringSafe<object>(),
								" (using undefined sound instead)"
							}), false);
						}
						this.fi.SetValue(this.wanter, defSilentFail);
						result = true;
					}
				}
				return result;
			}
		}

		// Token: 0x02000D7E RID: 3454
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef where T : new()
		{
			// Token: 0x040033A1 RID: 13217
			private List<string> defNames = new List<string>();

			// Token: 0x040033A2 RID: 13218
			private object debugWanterInfo;

			// Token: 0x06004D66 RID: 19814 RVA: 0x00284F43 File Offset: 0x00283343
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06004D67 RID: 19815 RVA: 0x00284F65 File Offset: 0x00283365
			public void AddWantedListEntry(string newTargetDefName)
			{
				this.defNames.Add(newTargetDefName);
			}

			// Token: 0x06004D68 RID: 19816 RVA: 0x00284F74 File Offset: 0x00283374
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
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

		// Token: 0x02000D7F RID: 3455
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef where K : new() where V : new()
		{
			// Token: 0x040033A3 RID: 13219
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x040033A4 RID: 13220
			private object debugWanterInfo;

			// Token: 0x06004D69 RID: 19817 RVA: 0x00284FFB File Offset: 0x002833FB
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06004D6A RID: 19818 RVA: 0x0028501D File Offset: 0x0028341D
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x06004D6B RID: 19819 RVA: 0x0028502C File Offset: 0x0028342C
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				List<Pair<K, V>> list = new List<Pair<K, V>>();
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					K first;
					if (flag)
					{
						first = DirectXmlCrossRefLoader.TryResolveDef<K>(xmlNode2.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						first = DirectXmlToObject.ObjectFromXml<K>(xmlNode2, true);
					}
					V second;
					if (flag2)
					{
						second = DirectXmlCrossRefLoader.TryResolveDef<V>(xmlNode3.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						second = DirectXmlToObject.ObjectFromXml<V>(xmlNode3, true);
					}
					list.Add(new Pair<K, V>(first, second));
				}
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<K, V> pair in list)
				{
					try
					{
						dictionary.Add(pair.First, pair.Second);
					}
					catch
					{
						Log.Error(string.Concat(new object[]
						{
							"Failed to load key/value pair: ",
							pair.First,
							", ",
							pair.Second
						}), false);
					}
				}
				return true;
			}
		}
	}
}
