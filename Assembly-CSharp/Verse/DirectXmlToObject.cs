using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D87 RID: 3463
	public static class DirectXmlToObject
	{
		// Token: 0x040033B1 RID: 13233
		public static Stack<Type> currentlyInstantiatingObjectOfType = new Stack<Type>();

		// Token: 0x040033B2 RID: 13234
		public const string DictionaryKeyName = "key";

		// Token: 0x040033B3 RID: 13235
		public const string DictionaryValueName = "value";

		// Token: 0x040033B4 RID: 13236
		public const string LoadDataFromXmlCustomMethodName = "LoadDataFromXmlCustom";

		// Token: 0x040033B5 RID: 13237
		public const string PostLoadMethodName = "PostLoad";

		// Token: 0x040033B6 RID: 13238
		public const string ObjectFromXmlMethodName = "ObjectFromXml";

		// Token: 0x040033B7 RID: 13239
		public const string ListFromXmlMethodName = "ListFromXml";

		// Token: 0x040033B8 RID: 13240
		public const string DictionaryFromXmlMethodName = "DictionaryFromXml";

		// Token: 0x040033B9 RID: 13241
		private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x06004D7B RID: 19835 RVA: 0x002869A8 File Offset: 0x00284DA8
		public static T ObjectFromXml<T>(XmlNode xmlRoot, bool doPostLoad) where T : new()
		{
			MethodInfo methodInfo = DirectXmlToObject.CustomDataLoadMethodOf(typeof(T));
			T result;
			if (methodInfo != null)
			{
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type);
				T t;
				try
				{
					t = (T)((object)Activator.CreateInstance(type));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				try
				{
					methodInfo.Invoke(t, new object[]
					{
						xmlRoot
					});
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in custom XML loader for ",
						typeof(T),
						". Node is:\n ",
						xmlRoot.OuterXml,
						"\n\nException is:\n ",
						ex.ToString()
					}), false);
					t = default(T);
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t);
				}
				result = t;
			}
			else if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.CDATA)
			{
				if (typeof(T) != typeof(string))
				{
					Log.Error("CDATA can only be used for strings. Bad xml: " + xmlRoot.OuterXml, false);
					result = default(T);
				}
				else
				{
					result = (T)((object)xmlRoot.FirstChild.Value);
				}
			}
			else if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.Text)
			{
				try
				{
					return (T)((object)ParseHelper.FromString(xmlRoot.InnerText, typeof(T)));
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing ",
						xmlRoot.OuterXml,
						" to type ",
						typeof(T),
						": ",
						ex2
					}), false);
				}
				result = default(T);
			}
			else if (Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
			{
				List<T> list = DirectXmlToObject.ListFromXml<T>(xmlRoot);
				int num = 0;
				foreach (T t2 in list)
				{
					int num2 = (int)((object)t2);
					num |= num2;
				}
				result = (T)((object)num);
			}
			else if (typeof(T).HasGenericDefinition(typeof(List<>)))
			{
				MethodInfo method = typeof(DirectXmlToObject).GetMethod("ListFromXml", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				Type[] genericArguments = typeof(T).GetGenericArguments();
				MethodInfo methodInfo2 = method.MakeGenericMethod(genericArguments);
				object[] parameters = new object[]
				{
					xmlRoot
				};
				object obj = methodInfo2.Invoke(null, parameters);
				result = (T)((object)obj);
			}
			else if (typeof(T).HasGenericDefinition(typeof(Dictionary<, >)))
			{
				MethodInfo method2 = typeof(DirectXmlToObject).GetMethod("DictionaryFromXml", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				Type[] genericArguments2 = typeof(T).GetGenericArguments();
				MethodInfo methodInfo3 = method2.MakeGenericMethod(genericArguments2);
				object[] parameters2 = new object[]
				{
					xmlRoot
				};
				object obj2 = methodInfo3.Invoke(null, parameters2);
				result = (T)((object)obj2);
			}
			else
			{
				if (!xmlRoot.HasChildNodes)
				{
					if (typeof(T) == typeof(string))
					{
						return (T)((object)"");
					}
					XmlAttribute xmlAttribute = xmlRoot.Attributes["IsNull"];
					if (xmlAttribute != null && xmlAttribute.Value.ToUpperInvariant() == "TRUE")
					{
						return default(T);
					}
					if (typeof(T).IsGenericType)
					{
						Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
						if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>) || genericTypeDefinition == typeof(Dictionary<, >))
						{
							return Activator.CreateInstance<T>();
						}
					}
				}
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type2 = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				Type type3 = Nullable.GetUnderlyingType(type2) ?? type2;
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type3);
				T t3;
				try
				{
					t3 = (T)((object)Activator.CreateInstance(type3));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				List<string> list2 = null;
				if (xmlRoot.ChildNodes.Count > 1)
				{
					list2 = new List<string>();
				}
				for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
				{
					XmlNode xmlNode = xmlRoot.ChildNodes[i];
					if (!(xmlNode is XmlComment))
					{
						if (xmlRoot.ChildNodes.Count > 1)
						{
							if (list2.Contains(xmlNode.Name))
							{
								Log.Error(string.Concat(new object[]
								{
									"XML ",
									typeof(T),
									" defines the same field twice: ",
									xmlNode.Name,
									".\n\nField contents: ",
									xmlNode.InnerText,
									".\n\nWhole XML:\n\n",
									xmlRoot.OuterXml
								}), false);
							}
							else
							{
								list2.Add(xmlNode.Name);
							}
						}
						FieldInfo fieldInfo = DirectXmlToObject.GetFieldInfoForType(t3.GetType(), xmlNode.Name, xmlRoot);
						if (fieldInfo == null)
						{
							foreach (FieldInfo fieldInfo2 in t3.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
							{
								foreach (object obj3 in fieldInfo2.GetCustomAttributes(typeof(LoadAliasAttribute), true))
								{
									string alias = ((LoadAliasAttribute)obj3).alias;
									if (alias.EqualsIgnoreCase(xmlNode.Name))
									{
										fieldInfo = fieldInfo2;
										break;
									}
								}
								if (fieldInfo != null)
								{
									break;
								}
							}
						}
						if (fieldInfo == null)
						{
							bool flag = false;
							foreach (object obj4 in t3.GetType().GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true))
							{
								string elementToIgnore = ((IgnoreSavedElementAttribute)obj4).elementToIgnore;
								if (string.Equals(elementToIgnore, xmlNode.Name, StringComparison.OrdinalIgnoreCase))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								Log.Error(string.Concat(new string[]
								{
									"XML error: ",
									xmlNode.OuterXml,
									" doesn't correspond to any field in type ",
									t3.GetType().Name,
									". Context: ",
									xmlRoot.OuterXml
								}), false);
							}
						}
						else if (typeof(Def).IsAssignableFrom(fieldInfo.FieldType))
						{
							if (xmlNode.InnerText.NullOrEmpty())
							{
								fieldInfo.SetValue(t3, null);
							}
							else
							{
								DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(t3, fieldInfo, xmlNode.InnerText);
							}
						}
						else
						{
							object value = null;
							try
							{
								MethodInfo method3 = typeof(DirectXmlToObject).GetMethod("ObjectFromXml", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
								MethodInfo methodInfo4 = method3.MakeGenericMethod(new Type[]
								{
									fieldInfo.FieldType
								});
								value = methodInfo4.Invoke(null, new object[]
								{
									xmlNode,
									doPostLoad
								});
							}
							catch (Exception ex3)
							{
								Log.Error("Exception loading from " + xmlNode.ToString() + ": " + ex3.ToString(), false);
								goto IL_863;
							}
							if (!typeof(T).IsValueType)
							{
								fieldInfo.SetValue(t3, value);
							}
							else
							{
								object obj5 = t3;
								fieldInfo.SetValue(obj5, value);
								t3 = (T)((object)obj5);
							}
						}
					}
					IL_863:;
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t3);
				}
				result = t3;
			}
			return result;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0028729C File Offset: 0x0028569C
		private static Type ClassTypeOf<T>(XmlNode xmlRoot)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Class"];
			Type result;
			if (xmlAttribute != null)
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value);
				if (typeInAnyAssembly == null)
				{
					Log.Error("Could not find type named " + xmlAttribute.Value + " from node " + xmlRoot.OuterXml, false);
					result = typeof(T);
				}
				else
				{
					result = typeInAnyAssembly;
				}
			}
			else
			{
				result = typeof(T);
			}
			return result;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x00287320 File Offset: 0x00285720
		private static void TryDoPostLoad(object obj)
		{
			try
			{
				MethodInfo method = obj.GetType().GetMethod("PostLoad");
				if (method != null)
				{
					method.Invoke(obj, null);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while executing PostLoad on ",
					obj.ToStringSafe<object>(),
					": ",
					ex
				}), false);
			}
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0028739C File Offset: 0x0028579C
		private static List<T> ListFromXml<T>(XmlNode listRootNode) where T : new()
		{
			List<T> list = new List<T>();
			try
			{
				bool flag = typeof(Def).IsAssignableFrom(typeof(T));
				IEnumerator enumerator = listRootNode.ChildNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlNode xmlNode = (XmlNode)obj;
						if (DirectXmlToObject.ValidateListNode(xmlNode, listRootNode, typeof(T)))
						{
							if (flag)
							{
								DirectXmlCrossRefLoader.RegisterListWantsCrossRef<T>(list, xmlNode.InnerText, listRootNode.Name);
							}
							else
							{
								list.Add(DirectXmlToObject.ObjectFromXml<T>(xmlNode, true));
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading list from XML: ",
					ex,
					"\nXML:\n",
					listRootNode.OuterXml
				}), false);
			}
			return list;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x002874BC File Offset: 0x002858BC
		private static Dictionary<K, V> DictionaryFromXml<K, V>(XmlNode dictRootNode) where K : new() where V : new()
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			try
			{
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				if (!flag && !flag2)
				{
					IEnumerator enumerator = dictRootNode.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode xmlNode = (XmlNode)obj;
							if (DirectXmlToObject.ValidateListNode(xmlNode, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								K key = DirectXmlToObject.ObjectFromXml<K>(xmlNode["key"], true);
								V value = DirectXmlToObject.ObjectFromXml<V>(xmlNode["value"], true);
								dictionary.Add(key, value);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				else
				{
					IEnumerator enumerator2 = dictRootNode.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (DirectXmlToObject.ValidateListNode(xmlNode2, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								DirectXmlCrossRefLoader.RegisterDictionaryWantsCrossRef<K, V>(dictionary, xmlNode2, dictRootNode.Name);
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Malformed dictionary XML. Node: ",
					dictRootNode.OuterXml,
					".\n\nException: ",
					ex
				}), false);
			}
			return dictionary;
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x002876B8 File Offset: 0x00285AB8
		private static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			return type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x002876DC File Offset: 0x00285ADC
		private static bool ValidateListNode(XmlNode listEntryNode, XmlNode listRootNode, Type listItemType)
		{
			bool result;
			if (listEntryNode is XmlComment)
			{
				result = false;
			}
			else if (listEntryNode is XmlText)
			{
				Log.Error("XML format error: Raw text found inside a list element. Did you mean to surround it with list item <li> tags? " + listRootNode.OuterXml, false);
				result = false;
			}
			else if (listEntryNode.Name != "li" && DirectXmlToObject.CustomDataLoadMethodOf(listItemType) == null)
			{
				Log.Error("XML format error: List item found with name that is not <li>, and which does not have a custom XML loader method, in " + listRootNode.OuterXml, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0028776C File Offset: 0x00285B6C
		private static FieldInfo GetFieldInfoForType(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary = DirectXmlToObject.fieldInfoLookup.TryGetValue(type, null);
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, FieldInfo>();
				DirectXmlToObject.fieldInfoLookup[type] = dictionary;
			}
			FieldInfo fieldInfo = dictionary.TryGetValue(token, null);
			if (fieldInfo == null && !dictionary.ContainsKey(token))
			{
				fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, BindingFlags.Default);
				if (fieldInfo == null)
				{
					fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, BindingFlags.IgnoreCase);
					if (fieldInfo != null && !type.HasAttribute<CaseInsensitiveXMLParsing>())
					{
						string text = string.Format("Attempt to use string {0} to refer to field {1} in type {2}; xml tags are now case-sensitive", token, fieldInfo.Name, type);
						if (debugXmlNode != null)
						{
							text = text + ". XML: " + debugXmlNode.OuterXml;
						}
						Log.Error(text, false);
					}
				}
				dictionary[token] = fieldInfo;
			}
			return fieldInfo;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x00287830 File Offset: 0x00285C30
		private static FieldInfo SearchTypeHierarchy(Type type, string token, BindingFlags extraFlags)
		{
			FieldInfo field;
			for (;;)
			{
				field = type.GetField(token, extraFlags | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (field != null || type.BaseType == typeof(object))
				{
					break;
				}
				type = type.BaseType;
			}
			return field;
		}
	}
}
