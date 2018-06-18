using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D86 RID: 3462
	public static class DirectXmlSaver
	{
		// Token: 0x06004D5C RID: 19804 RVA: 0x00284A64 File Offset: 0x00282E64
		public static bool IsSimpleTextType(Type type)
		{
			return type == typeof(float) || type == typeof(double) || type == typeof(long) || type == typeof(ulong) || type == typeof(char) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(int) || type == typeof(uint) || type == typeof(bool) || type == typeof(short) || type == typeof(ushort) || type == typeof(string) || type.IsEnum;
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x00284B54 File Offset: 0x00282F54
		public static void SaveDataObject(object obj, string filePath)
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(obj, obj.GetType());
				xdocument.Add(content);
				xdocument.Save(filePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(new object[]
				{
					filePath,
					ex.ToString()
				}));
				Log.Error(string.Concat(new object[]
				{
					"Exception saving data object ",
					obj,
					": ",
					ex
				}), false);
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x00284BF0 File Offset: 0x00282FF0
		public static XElement XElementFromObject(object obj, Type expectedClass)
		{
			return DirectXmlSaver.XElementFromObject(obj, expectedClass, expectedClass.Name, null, false);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x00284C14 File Offset: 0x00283014
		public static XElement XElementFromObject(object obj, Type expectedType, string nodeName, FieldInfo owningField = null, bool saveDefsAsRefs = false)
		{
			if (owningField != null)
			{
				DefaultValueAttribute defaultValueAttribute;
				if (owningField.TryGetAttribute(out defaultValueAttribute))
				{
					if (defaultValueAttribute.ObjIsDefault(obj))
					{
						return null;
					}
				}
			}
			XElement result;
			if (obj == null)
			{
				XElement xelement = new XElement(nodeName);
				xelement.SetAttributeValue("IsNull", "True");
				result = xelement;
			}
			else
			{
				Type type = obj.GetType();
				XElement xelement2 = new XElement(nodeName);
				if (DirectXmlSaver.IsSimpleTextType(type))
				{
					xelement2.Add(new XText(obj.ToString()));
				}
				else if (saveDefsAsRefs && typeof(Def).IsAssignableFrom(type))
				{
					string defName = ((Def)obj).defName;
					xelement2.Add(new XText(defName));
				}
				else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
				{
					Type expectedType2 = type.GetGenericArguments()[0];
					int num = (int)type.GetProperty("Count").GetValue(obj, null);
					for (int i = 0; i < num; i++)
					{
						object[] index = new object[]
						{
							i
						};
						object value = type.GetProperty("Item").GetValue(obj, index);
						XNode content = DirectXmlSaver.XElementFromObject(value, expectedType2, "li", null, true);
						xelement2.Add(content);
					}
				}
				else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
				{
					Type expectedType3 = type.GetGenericArguments()[0];
					Type expectedType4 = type.GetGenericArguments()[1];
					IEnumerator enumerator = (obj as IEnumerable).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							object value2 = obj2.GetType().GetProperty("Key").GetValue(obj2, null);
							object value3 = obj2.GetType().GetProperty("Value").GetValue(obj2, null);
							XElement xelement3 = new XElement("li");
							xelement3.Add(DirectXmlSaver.XElementFromObject(value2, expectedType3, "key", null, true));
							xelement3.Add(DirectXmlSaver.XElementFromObject(value3, expectedType4, "value", null, true));
							xelement2.Add(xelement3);
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
					if (type != expectedType)
					{
						XAttribute content2 = new XAttribute("Class", GenTypes.GetTypeNameWithoutIgnoredNamespaces(obj.GetType()));
						xelement2.Add(content2);
					}
					foreach (FieldInfo fi in from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby f.MetadataToken
					select f)
					{
						try
						{
							XElement xelement4 = DirectXmlSaver.XElementFromField(fi, obj);
							if (xelement4 != null)
							{
								xelement2.Add(xelement4);
							}
						}
						catch
						{
							throw;
						}
					}
				}
				result = xelement2;
			}
			return result;
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00284F80 File Offset: 0x00283380
		private static XElement XElementFromField(FieldInfo fi, object owningObj)
		{
			XElement result;
			if (Attribute.IsDefined(fi, typeof(UnsavedAttribute)))
			{
				result = null;
			}
			else
			{
				object value = fi.GetValue(owningObj);
				result = DirectXmlSaver.XElementFromObject(value, fi.FieldType, fi.Name, fi, false);
			}
			return result;
		}
	}
}
