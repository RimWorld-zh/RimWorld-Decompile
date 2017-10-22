using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	public static class Scribe_Collections
	{
		public static void Look<T>(ref List<T> list, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			Scribe_Collections.Look<T>(ref list, false, label, lookMode, ctorArgs);
		}

		public static void Look<T>(ref List<T> list, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			if (lookMode == LookMode.Undefined)
			{
				if (!ParseHelper.HandlesType(typeof(T)))
				{
					if (typeof(T) == typeof(LocalTargetInfo))
					{
						lookMode = LookMode.LocalTargetInfo;
						goto IL_010e;
					}
					if (typeof(T) == typeof(TargetInfo))
					{
						lookMode = LookMode.TargetInfo;
						goto IL_010e;
					}
					if (typeof(T) == typeof(GlobalTargetInfo))
					{
						lookMode = LookMode.GlobalTargetInfo;
						goto IL_010e;
					}
					if (typeof(Def).IsAssignableFrom(typeof(T)))
					{
						lookMode = LookMode.Def;
						goto IL_010e;
					}
					if (typeof(IExposable).IsAssignableFrom(typeof(T)) && !typeof(ILoadReferenceable).IsAssignableFrom(typeof(T)))
					{
						lookMode = LookMode.Deep;
						goto IL_010e;
					}
					Log.Error("LookList call with a list of " + typeof(T) + " must have lookMode set explicitly.");
					return;
				}
				lookMode = LookMode.Value;
			}
			goto IL_010e;
			IL_010e:
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (list == null)
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
						}
						else
						{
							List<T>.Enumerator enumerator = list.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									T current = enumerator.Current;
									switch (lookMode)
									{
									case LookMode.Value:
									{
										T val = current;
										Scribe_Values.Look<T>(ref val, "li", default(T), true);
										break;
									}
									case LookMode.LocalTargetInfo:
									{
										LocalTargetInfo localTargetInfo = (LocalTargetInfo)(object)current;
										Scribe_TargetInfo.Look(ref localTargetInfo, saveDestroyedThings, "li");
										break;
									}
									case LookMode.TargetInfo:
									{
										TargetInfo targetInfo = (TargetInfo)(object)current;
										Scribe_TargetInfo.Look(ref targetInfo, saveDestroyedThings, "li");
										break;
									}
									case LookMode.GlobalTargetInfo:
									{
										GlobalTargetInfo globalTargetInfo = (GlobalTargetInfo)(object)current;
										Scribe_TargetInfo.Look(ref globalTargetInfo, saveDestroyedThings, "li");
										break;
									}
									case LookMode.Def:
									{
										Def def = (Def)(object)current;
										Scribe_Defs.Look<Def>(ref def, "li");
										break;
									}
									case LookMode.Deep:
									{
										T val2 = current;
										Scribe_Deep.Look<T>(ref val2, saveDestroyedThings, "li", ctorArgs);
										break;
									}
									case LookMode.Reference:
									{
										ILoadReferenceable loadReferenceable = (ILoadReferenceable)(object)current;
										Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "li", saveDestroyedThings);
										break;
									}
									}
								}
							}
							finally
							{
								((IDisposable)(object)enumerator).Dispose();
							}
						}
					}
					else if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlNode curXmlParent = Scribe.loader.curXmlParent;
						XmlAttribute xmlAttribute = curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
						{
							list = null;
						}
						else
						{
							switch (lookMode)
							{
							case LookMode.Value:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								foreach (XmlNode childNode in curXmlParent.ChildNodes)
								{
									T item = ScribeExtractor.ValueFromNode<T>(childNode, default(T));
									list.Add(item);
								}
								break;
							}
							case LookMode.Deep:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								foreach (XmlNode childNode2 in curXmlParent.ChildNodes)
								{
									T item2 = ScribeExtractor.SaveableFromNode<T>(childNode2, ctorArgs);
									list.Add(item2);
								}
								break;
							}
							case LookMode.Def:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								foreach (XmlNode childNode3 in curXmlParent.ChildNodes)
								{
									T item3 = ScribeExtractor.DefFromNodeUnsafe<T>(childNode3);
									list.Add(item3);
								}
								break;
							}
							case LookMode.LocalTargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num = 0;
								foreach (XmlNode childNode4 in curXmlParent.ChildNodes)
								{
									LocalTargetInfo localTargetInfo2 = ScribeExtractor.LocalTargetInfoFromNode(childNode4, num.ToString(), LocalTargetInfo.Invalid);
									T item4 = (T)(object)localTargetInfo2;
									list.Add(item4);
									num++;
								}
								break;
							}
							case LookMode.TargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num2 = 0;
								foreach (XmlNode childNode5 in curXmlParent.ChildNodes)
								{
									TargetInfo targetInfo2 = ScribeExtractor.TargetInfoFromNode(childNode5, num2.ToString(), TargetInfo.Invalid);
									T item5 = (T)(object)targetInfo2;
									list.Add(item5);
									num2++;
								}
								break;
							}
							case LookMode.GlobalTargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num3 = 0;
								foreach (XmlNode childNode6 in curXmlParent.ChildNodes)
								{
									GlobalTargetInfo globalTargetInfo2 = ScribeExtractor.GlobalTargetInfoFromNode(childNode6, num3.ToString(), GlobalTargetInfo.Invalid);
									T item6 = (T)(object)globalTargetInfo2;
									list.Add(item6);
									num3++;
								}
								break;
							}
							case LookMode.Reference:
							{
								List<string> list2 = new List<string>(curXmlParent.ChildNodes.Count);
								foreach (XmlNode childNode7 in curXmlParent.ChildNodes)
								{
									list2.Add(childNode7.InnerText);
								}
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(list2, string.Empty);
								break;
							}
							}
						}
					}
					else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
					{
						switch (lookMode)
						{
						case LookMode.Reference:
						{
							list = Scribe.loader.crossRefs.TakeResolvedRefList<T>(string.Empty);
							break;
						}
						case LookMode.LocalTargetInfo:
						{
							if (list != null)
							{
								for (int i = 0; i < list.Count; i++)
								{
									list[i] = (T)(object)ScribeExtractor.ResolveLocalTargetInfo((LocalTargetInfo)(object)list[i], i.ToString());
								}
							}
							break;
						}
						case LookMode.TargetInfo:
						{
							if (list != null)
							{
								for (int j = 0; j < list.Count; j++)
								{
									list[j] = (T)(object)ScribeExtractor.ResolveTargetInfo((TargetInfo)(object)list[j], j.ToString());
								}
							}
							break;
						}
						case LookMode.GlobalTargetInfo:
						{
							if (list != null)
							{
								for (int k = 0; k < list.Count; k++)
								{
									list[k] = (T)(object)ScribeExtractor.ResolveGlobalTargetInfo((GlobalTargetInfo)(object)list[k], k.ToString());
								}
							}
							break;
						}
						}
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (lookMode == LookMode.Reference)
				{
					Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, label);
				}
				list = null;
			}
		}

		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode = LookMode.Undefined, LookMode valueLookMode = LookMode.Undefined) where K : new()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				bool flag = keyLookMode == LookMode.Reference;
				bool flag2 = valueLookMode == LookMode.Reference;
				if (flag != flag2)
				{
					Log.Error("You need to provide working lists for the keys and values in order to be able to load such dictionary.");
					return;
				}
			}
			List<K> list = null;
			List<V> list2 = null;
			Scribe_Collections.Look<K, V>(ref dict, label, keyLookMode, valueLookMode, ref list, ref list2);
		}

		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode, LookMode valueLookMode, ref List<K> keysWorkingList, ref List<V> valuesWorkingList) where K : new()
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
					{
						keysWorkingList = new List<K>();
						valuesWorkingList = new List<V>();
					}
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						Dictionary<K, V>.Enumerator enumerator = dict.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<K, V> current = enumerator.Current;
								keysWorkingList.Add(current.Key);
								valuesWorkingList.Add(current.Value);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator).Dispose();
						}
					}
					Scribe_Collections.Look<K>(ref keysWorkingList, "keys", keyLookMode, new object[0]);
					Scribe_Collections.Look<V>(ref valuesWorkingList, "values", valueLookMode, new object[0]);
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
					bool flag = keyLookMode == LookMode.Reference || valueLookMode == LookMode.Reference;
					if (flag && Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
					{
						goto IL_0116;
					}
					if (!flag && Scribe.mode == LoadSaveMode.LoadingVars)
						goto IL_0116;
					goto IL_0277;
					IL_0116:
					dict.Clear();
					if (keysWorkingList == null)
					{
						Log.Error("Cannot fill dictionary because there are no keys.");
					}
					else if (valuesWorkingList == null)
					{
						Log.Error("Cannot fill dictionary because there are no values.");
					}
					else
					{
						if (keysWorkingList.Count != valuesWorkingList.Count)
						{
							Log.Error("Keys count does not match the values count while loading a dictionary (maybe keys and values were resolved during different passes?). Some elements will be skipped. keys=" + keysWorkingList.Count + ", values=" + valuesWorkingList.Count);
						}
						int num = Math.Min(keysWorkingList.Count, valuesWorkingList.Count);
						for (int num2 = 0; num2 < num; num2++)
						{
							if (keysWorkingList[num2] == null)
							{
								Log.Error("Null key while loading dictionary of " + typeof(K) + " and " + typeof(V) + ".");
							}
							else
							{
								try
								{
									dict.Add(keysWorkingList[num2], valuesWorkingList[num2]);
								}
								catch (Exception ex)
								{
									Log.Error("Exception in LookDictionary(node=" + label + "): " + ex);
								}
							}
						}
					}
					goto IL_0277;
					IL_0277:
					if (Scribe.mode == LoadSaveMode.PostLoadInit)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				dict = null;
			}
		}

		public static void Look<T>(ref HashSet<T> valueHashSet, string label, LookMode lookMode = LookMode.Undefined) where T : new()
		{
			Scribe_Collections.Look<T>(ref valueHashSet, false, label, lookMode);
		}

		public static void Look<T>(ref HashSet<T> valueHashSet, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined) where T : new()
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueHashSet != null)
			{
				list = new List<T>();
				HashSet<T>.Enumerator enumerator = valueHashSet.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.Current;
						list.Add(current);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			Scribe_Collections.Look<T>(ref list, saveDestroyedThings, label, lookMode, new object[0]);
			if (lookMode != LookMode.Reference || Scribe.mode != LoadSaveMode.ResolvingCrossRefs)
			{
				if (lookMode == LookMode.Reference)
					return;
				if (Scribe.mode != LoadSaveMode.LoadingVars)
					return;
			}
			if (list == null)
			{
				valueHashSet = null;
			}
			else
			{
				if (valueHashSet == null)
				{
					valueHashSet = new HashSet<T>();
				}
				else
				{
					valueHashSet.Clear();
				}
				for (int i = 0; i < list.Count; i++)
				{
					valueHashSet.Add(list[i]);
				}
			}
		}

		public static void Look<T>(ref Stack<T> valueStack, string label, LookMode lookMode = LookMode.Undefined) where T : new()
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueStack != null)
			{
				list = new List<T>();
				Stack<T>.Enumerator enumerator = valueStack.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.Current;
						list.Add(current);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			Scribe_Collections.Look<T>(ref list, label, lookMode, new object[0]);
			if (lookMode != LookMode.Reference || Scribe.mode != LoadSaveMode.ResolvingCrossRefs)
			{
				if (lookMode == LookMode.Reference)
					return;
				if (Scribe.mode != LoadSaveMode.LoadingVars)
					return;
			}
			if (list == null)
			{
				valueStack = null;
			}
			else
			{
				if (valueStack == null)
				{
					valueStack = new Stack<T>();
				}
				else
				{
					valueStack.Clear();
				}
				for (int i = 0; i < list.Count; i++)
				{
					valueStack.Push(list[i]);
				}
			}
		}
	}
}
