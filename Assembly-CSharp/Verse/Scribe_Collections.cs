using RimWorld.Planet;
using System;
using System.Collections;
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
						goto IL_0116;
					}
					if (typeof(T) == typeof(TargetInfo))
					{
						lookMode = LookMode.TargetInfo;
						goto IL_0116;
					}
					if (typeof(T) == typeof(GlobalTargetInfo))
					{
						lookMode = LookMode.GlobalTargetInfo;
						goto IL_0116;
					}
					if (typeof(Def).IsAssignableFrom(typeof(T)))
					{
						lookMode = LookMode.Def;
						goto IL_0116;
					}
					if (typeof(IExposable).IsAssignableFrom(typeof(T)) && !typeof(ILoadReferenceable).IsAssignableFrom(typeof(T)))
					{
						lookMode = LookMode.Deep;
						goto IL_0116;
					}
					Log.Error("LookList call with a list of " + typeof(T) + " must have lookMode set explicitly.");
					return;
				}
				lookMode = LookMode.Value;
			}
			goto IL_0116;
			IL_0116:
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
							foreach (T item7 in list)
							{
								switch (lookMode)
								{
								case LookMode.Value:
								{
									T val = item7;
									Scribe_Values.Look<T>(ref val, "li", default(T), true);
									break;
								}
								case LookMode.LocalTargetInfo:
								{
									LocalTargetInfo localTargetInfo = (LocalTargetInfo)(object)item7;
									Scribe_TargetInfo.Look(ref localTargetInfo, saveDestroyedThings, "li");
									break;
								}
								case LookMode.TargetInfo:
								{
									TargetInfo targetInfo = (TargetInfo)(object)item7;
									Scribe_TargetInfo.Look(ref targetInfo, saveDestroyedThings, "li");
									break;
								}
								case LookMode.GlobalTargetInfo:
								{
									GlobalTargetInfo globalTargetInfo = (GlobalTargetInfo)(object)item7;
									Scribe_TargetInfo.Look(ref globalTargetInfo, saveDestroyedThings, "li");
									break;
								}
								case LookMode.Def:
								{
									Def def = (Def)(object)item7;
									Scribe_Defs.Look<Def>(ref def, "li");
									break;
								}
								case LookMode.Deep:
								{
									T val2 = item7;
									Scribe_Deep.Look<T>(ref val2, saveDestroyedThings, "li", ctorArgs);
									break;
								}
								case LookMode.Reference:
								{
									ILoadReferenceable loadReferenceable = (ILoadReferenceable)(object)item7;
									Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "li", saveDestroyedThings);
									break;
								}
								}
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
								IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										XmlNode subNode = (XmlNode)enumerator2.Current;
										T item = ScribeExtractor.ValueFromNode<T>(subNode, default(T));
										list.Add(item);
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator2 as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
								break;
							}
							case LookMode.Deep:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								IEnumerator enumerator3 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										XmlNode subNode2 = (XmlNode)enumerator3.Current;
										T item2 = ScribeExtractor.SaveableFromNode<T>(subNode2, ctorArgs);
										list.Add(item2);
									}
								}
								finally
								{
									IDisposable disposable2;
									if ((disposable2 = (enumerator3 as IDisposable)) != null)
									{
										disposable2.Dispose();
									}
								}
								break;
							}
							case LookMode.Def:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								IEnumerator enumerator4 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										XmlNode subNode3 = (XmlNode)enumerator4.Current;
										T item3 = ScribeExtractor.DefFromNodeUnsafe<T>(subNode3);
										list.Add(item3);
									}
								}
								finally
								{
									IDisposable disposable3;
									if ((disposable3 = (enumerator4 as IDisposable)) != null)
									{
										disposable3.Dispose();
									}
								}
								break;
							}
							case LookMode.LocalTargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num = 0;
								IEnumerator enumerator5 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator5.MoveNext())
									{
										XmlNode node = (XmlNode)enumerator5.Current;
										LocalTargetInfo localTargetInfo2 = ScribeExtractor.LocalTargetInfoFromNode(node, num.ToString(), LocalTargetInfo.Invalid);
										T item4 = (T)(object)localTargetInfo2;
										list.Add(item4);
										num++;
									}
								}
								finally
								{
									IDisposable disposable4;
									if ((disposable4 = (enumerator5 as IDisposable)) != null)
									{
										disposable4.Dispose();
									}
								}
								break;
							}
							case LookMode.TargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num2 = 0;
								IEnumerator enumerator6 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator6.MoveNext())
									{
										XmlNode node2 = (XmlNode)enumerator6.Current;
										TargetInfo targetInfo2 = ScribeExtractor.TargetInfoFromNode(node2, num2.ToString(), TargetInfo.Invalid);
										T item5 = (T)(object)targetInfo2;
										list.Add(item5);
										num2++;
									}
								}
								finally
								{
									IDisposable disposable5;
									if ((disposable5 = (enumerator6 as IDisposable)) != null)
									{
										disposable5.Dispose();
									}
								}
								break;
							}
							case LookMode.GlobalTargetInfo:
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num3 = 0;
								IEnumerator enumerator7 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator7.MoveNext())
									{
										XmlNode node3 = (XmlNode)enumerator7.Current;
										GlobalTargetInfo globalTargetInfo2 = ScribeExtractor.GlobalTargetInfoFromNode(node3, num3.ToString(), GlobalTargetInfo.Invalid);
										T item6 = (T)(object)globalTargetInfo2;
										list.Add(item6);
										num3++;
									}
								}
								finally
								{
									IDisposable disposable6;
									if ((disposable6 = (enumerator7 as IDisposable)) != null)
									{
										disposable6.Dispose();
									}
								}
								break;
							}
							case LookMode.Reference:
							{
								List<string> list2 = new List<string>(curXmlParent.ChildNodes.Count);
								IEnumerator enumerator8 = curXmlParent.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator8.MoveNext())
									{
										XmlNode xmlNode = (XmlNode)enumerator8.Current;
										list2.Add(xmlNode.InnerText);
									}
								}
								finally
								{
									IDisposable disposable7;
									if ((disposable7 = (enumerator8 as IDisposable)) != null)
									{
										disposable7.Dispose();
									}
								}
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(list2, "");
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
							list = Scribe.loader.crossRefs.TakeResolvedRefList<T>("");
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
						foreach (KeyValuePair<K, V> item in dict)
						{
							keysWorkingList.Add(item.Key);
							valuesWorkingList.Add(item.Value);
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
						goto IL_0128;
					}
					if (!flag && Scribe.mode == LoadSaveMode.LoadingVars)
						goto IL_0128;
					goto IL_0294;
					IL_0128:
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
					goto IL_0294;
					IL_0294:
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
				foreach (T item in valueHashSet)
				{
					list.Add(item);
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
				foreach (T item in valueStack)
				{
					list.Add(item);
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
