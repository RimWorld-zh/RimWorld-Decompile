using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D7C RID: 3452
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004D40 RID: 19776 RVA: 0x002833EC File Offset: 0x002817EC
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x00283410 File Offset: 0x00281810
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName)
		{
			DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x00283434 File Offset: 0x00281834
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName)
		{
			DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName);
			DirectXmlCrossRefLoader.wantedRefs.Add(item);
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x00283464 File Offset: 0x00281864
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

		// Token: 0x06004D44 RID: 19780 RVA: 0x002834FC File Offset: 0x002818FC
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

		// Token: 0x06004D45 RID: 19781 RVA: 0x00283594 File Offset: 0x00281994
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

		// Token: 0x06004D46 RID: 19782 RVA: 0x00283635 File Offset: 0x00281A35
		public static void Clear()
		{
			DirectXmlCrossRefLoader.wantedRefs.Clear();
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00283644 File Offset: 0x00281A44
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

		// Token: 0x04003392 RID: 13202
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x02000D7D RID: 3453
		private abstract class WantedRef
		{
			// Token: 0x06004D4A RID: 19786
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x04003393 RID: 13203
			public object wanter;
		}

		// Token: 0x02000D7E RID: 3454
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06004D4B RID: 19787 RVA: 0x002836D0 File Offset: 0x00281AD0
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
			}

			// Token: 0x06004D4C RID: 19788 RVA: 0x002836F0 File Offset: 0x00281AF0
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

			// Token: 0x04003394 RID: 13204
			public FieldInfo fi;

			// Token: 0x04003395 RID: 13205
			public string defName;
		}

		// Token: 0x02000D7F RID: 3455
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef where T : new()
		{
			// Token: 0x06004D4D RID: 19789 RVA: 0x00283867 File Offset: 0x00281C67
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06004D4E RID: 19790 RVA: 0x00283889 File Offset: 0x00281C89
			public void AddWantedListEntry(string newTargetDefName)
			{
				this.defNames.Add(newTargetDefName);
			}

			// Token: 0x06004D4F RID: 19791 RVA: 0x00283898 File Offset: 0x00281C98
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

			// Token: 0x04003396 RID: 13206
			private List<string> defNames = new List<string>();

			// Token: 0x04003397 RID: 13207
			private object debugWanterInfo;
		}

		// Token: 0x02000D80 RID: 3456
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef where K : new() where V : new()
		{
			// Token: 0x06004D50 RID: 19792 RVA: 0x0028391F File Offset: 0x00281D1F
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06004D51 RID: 19793 RVA: 0x00283941 File Offset: 0x00281D41
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x06004D52 RID: 19794 RVA: 0x00283950 File Offset: 0x00281D50
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

			// Token: 0x04003398 RID: 13208
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x04003399 RID: 13209
			private object debugWanterInfo;
		}
	}
}
