using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	public static class DirectXmlToObject
	{
		public const string DictionaryKeyName = "key";

		public const string DictionaryValueName = "value";

		public const string LoadDataFromXmlCustomMethodName = "LoadDataFromXmlCustom";

		public const string PostLoadMethodName = "PostLoad";

		public const string ObjectFromXmlMethodName = "ObjectFromXml";

		public const string ListFromXmlMethodName = "ListFromXml";

		public const string DictionaryFromXmlMethodName = "DictionaryFromXml";

		private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		public static T ObjectFromXml<T>(XmlNode xmlRoot, bool doPostLoad) where T : new()
		{
			MethodInfo methodInfo = DirectXmlToObject.CustomDataLoadMethodOf(typeof(T));
			T result;
			if (methodInfo != null)
			{
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				T val = (T)Activator.CreateInstance(type);
				try
				{
					methodInfo.Invoke(val, new object[1]
					{
						xmlRoot
					});
				}
				catch (Exception ex)
				{
					Log.Error("Exception in custom XML loader for " + typeof(T) + ". Node is:\n " + xmlRoot.OuterXml + "\n\nException is:\n " + ex.ToString());
					val = default(T);
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(val);
				}
				result = val;
				goto IL_0834;
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.CDATA)
			{
				if (typeof(T) != typeof(string))
				{
					Log.Error("CDATA can only be used for strings. Bad xml: " + xmlRoot.OuterXml);
					result = default(T);
				}
				else
				{
					result = (T)(object)xmlRoot.FirstChild.Value;
				}
				goto IL_0834;
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.Text)
			{
				try
				{
					return (T)ParseHelper.FromString(xmlRoot.InnerText, typeof(T));
				}
				catch (Exception ex2)
				{
					Log.Error("Exception parsing " + xmlRoot.OuterXml + " to type " + typeof(T) + ": " + ex2);
				}
				result = default(T);
				goto IL_0834;
			}
			if (Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
			{
				List<T> list = DirectXmlToObject.ListFromXml<T>(xmlRoot);
				int num = 0;
				foreach (T item in list)
				{
					int num2 = (int)(object)item;
					num |= num2;
				}
				result = (T)(object)num;
				goto IL_0834;
			}
			if (typeof(T).HasGenericDefinition(typeof(List<>)))
			{
				MethodInfo method = typeof(DirectXmlToObject).GetMethod("ListFromXml", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				Type[] genericArguments = typeof(T).GetGenericArguments();
				MethodInfo methodInfo2 = method.MakeGenericMethod(genericArguments);
				object[] parameters = new object[1]
				{
					xmlRoot
				};
				object obj = methodInfo2.Invoke(null, parameters);
				result = (T)obj;
				goto IL_0834;
			}
			if (typeof(T).HasGenericDefinition(typeof(Dictionary<, >)))
			{
				MethodInfo method2 = typeof(DirectXmlToObject).GetMethod("DictionaryFromXml", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				Type[] genericArguments2 = typeof(T).GetGenericArguments();
				MethodInfo methodInfo3 = method2.MakeGenericMethod(genericArguments2);
				object[] parameters2 = new object[1]
				{
					xmlRoot
				};
				object obj2 = methodInfo3.Invoke(null, parameters2);
				result = (T)obj2;
				goto IL_0834;
			}
			if (!xmlRoot.HasChildNodes)
			{
				if (typeof(T) == typeof(string))
				{
					result = (T)(object)"";
					goto IL_0834;
				}
				XmlAttribute xmlAttribute = xmlRoot.Attributes["IsNull"];
				if (xmlAttribute != null && xmlAttribute.Value.ToUpperInvariant() == "TRUE")
				{
					result = default(T);
					goto IL_0834;
				}
				if (typeof(T).IsGenericType)
				{
					Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
					if (genericTypeDefinition != typeof(List<>) && genericTypeDefinition != typeof(HashSet<>) && genericTypeDefinition != typeof(Dictionary<, >))
					{
						goto IL_0441;
					}
					result = new T();
					goto IL_0834;
				}
			}
			goto IL_0441;
			IL_0441:
			xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
			Type type2 = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
			T val2 = (T)Activator.CreateInstance(type2);
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
							Log.Error("XML " + typeof(T) + " defines the same field twice: " + xmlNode.Name + ".\n\nField contents: " + xmlNode.InnerText + ".\n\nWhole XML:\n\n" + xmlRoot.OuterXml);
						}
						else
						{
							list2.Add(xmlNode.Name);
						}
					}
					FieldInfo fieldInfo = DirectXmlToObject.GetFieldInfoForType(val2.GetType(), xmlNode.Name, xmlRoot);
					if (fieldInfo == null)
					{
						FieldInfo[] fields = val2.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						int num3 = 0;
						while (num3 < fields.Length)
						{
							FieldInfo fieldInfo2 = fields[num3];
							object[] customAttributes = fieldInfo2.GetCustomAttributes(typeof(LoadAliasAttribute), true);
							for (int j = 0; j < customAttributes.Length; j++)
							{
								object obj3 = customAttributes[j];
								string alias = ((LoadAliasAttribute)obj3).alias;
								if (alias.EqualsIgnoreCase(xmlNode.Name))
								{
									fieldInfo = fieldInfo2;
									break;
								}
							}
							if (fieldInfo == null)
							{
								num3++;
								continue;
							}
							break;
						}
					}
					if (fieldInfo == null)
					{
						bool flag = false;
						object[] customAttributes2 = val2.GetType().GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true);
						for (int k = 0; k < customAttributes2.Length; k++)
						{
							object obj4 = customAttributes2[k];
							string elementToIgnore = ((IgnoreSavedElementAttribute)obj4).elementToIgnore;
							if (string.Equals(elementToIgnore, xmlNode.Name, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							Log.Error("XML error: " + xmlNode.OuterXml + " doesn't correspond to any field in type " + val2.GetType().Name + ".");
						}
					}
					else if (typeof(Def).IsAssignableFrom(fieldInfo.FieldType))
					{
						if (xmlNode.InnerText.NullOrEmpty())
						{
							fieldInfo.SetValue(val2, null);
						}
						else
						{
							DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(val2, fieldInfo, xmlNode.InnerText);
						}
					}
					else
					{
						object obj5 = null;
						try
						{
							MethodInfo method3 = typeof(DirectXmlToObject).GetMethod("ObjectFromXml", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							MethodInfo methodInfo4 = method3.MakeGenericMethod(fieldInfo.FieldType);
							obj5 = methodInfo4.Invoke(null, new object[2]
							{
								xmlNode,
								doPostLoad
							});
						}
						catch (Exception ex3)
						{
							Log.Error("Exception loading from " + xmlNode.ToString() + ": " + ex3.ToString());
							continue;
						}
						if (!typeof(T).IsValueType)
						{
							fieldInfo.SetValue(val2, obj5);
						}
						else
						{
							object obj6 = val2;
							fieldInfo.SetValue(obj6, obj5);
							val2 = (T)obj6;
						}
					}
				}
			}
			if (doPostLoad)
			{
				DirectXmlToObject.TryDoPostLoad(val2);
			}
			result = val2;
			goto IL_0834;
			IL_0834:
			return result;
		}

		private static Type ClassTypeOf<T>(XmlNode xmlRoot)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Class"];
			Type result;
			if (xmlAttribute != null)
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value);
				if (typeInAnyAssembly == null)
				{
					Log.Error("Could not find type named " + xmlAttribute.Value + " from node " + xmlRoot.OuterXml);
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
				Log.Error("Exception while executing PostLoad on " + obj.ToStringSafe() + ": " + ex);
			}
		}

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
						XmlNode xmlNode = (XmlNode)enumerator.Current;
						if (DirectXmlToObject.ValidateListNode(xmlNode, listRootNode, typeof(T)))
						{
							if (flag)
							{
								DirectXmlCrossRefLoader.RegisterListWantsCrossRef<T>(list, xmlNode.InnerText);
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
				Log.Error("Exception loading list from XML: " + ex + "\nXML:\n" + listRootNode.OuterXml);
			}
			return list;
		}

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
							XmlNode xmlNode = (XmlNode)enumerator.Current;
							if (DirectXmlToObject.ValidateListNode(xmlNode, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								K key = DirectXmlToObject.ObjectFromXml<K>((XmlNode)xmlNode["key"], true);
								V value = DirectXmlToObject.ObjectFromXml<V>((XmlNode)xmlNode["value"], true);
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
							XmlNode xmlNode2 = (XmlNode)enumerator2.Current;
							if (DirectXmlToObject.ValidateListNode(xmlNode2, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								DirectXmlCrossRefLoader.RegisterDictionaryWantsCrossRef<K, V>(dictionary, xmlNode2);
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
				Log.Error("Malformed dictionary XML. Node: " + dictRootNode.OuterXml + ".\n\nException: " + ex);
			}
			return dictionary;
		}

		private static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			return type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		private static bool ValidateListNode(XmlNode listEntryNode, XmlNode listRootNode, Type listItemType)
		{
			bool result;
			if (listEntryNode is XmlComment)
			{
				result = false;
			}
			else if (listEntryNode is XmlText)
			{
				Log.Error("XML format error: Raw text found inside a list element. Did you mean to surround it with list item <li> tags? " + listRootNode.OuterXml);
				result = false;
			}
			else if (listEntryNode.Name != "li" && DirectXmlToObject.CustomDataLoadMethodOf(listItemType) == null)
			{
				Log.Error("XML format error: List item found with name that is not <li>, and which does not have a custom XML loader method, in " + listRootNode.OuterXml);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		private static FieldInfo GetFieldInfoForType(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary = DirectXmlToObject.fieldInfoLookup.TryGetValue(type);
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, FieldInfo>();
				DirectXmlToObject.fieldInfoLookup[type] = dictionary;
			}
			FieldInfo fieldInfo = dictionary.TryGetValue(token);
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
						Log.Error(text);
					}
				}
				dictionary[token] = fieldInfo;
			}
			return fieldInfo;
		}

		private static FieldInfo SearchTypeHierarchy(Type type, string token, BindingFlags extraFlags)
		{
			FieldInfo fieldInfo = null;
			while (true)
			{
				fieldInfo = type.GetField(token, (BindingFlags)((int)extraFlags | 16 | 32 | 4));
				if (fieldInfo != null)
					break;
				if (type.BaseType == typeof(object))
					break;
				type = type.BaseType;
			}
			return fieldInfo;
		}
	}
}
