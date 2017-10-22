using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Verse
{
	public static class DirectXmlSaver
	{
		public static bool IsSimpleTextType(Type type)
		{
			return type == typeof(float) || type == typeof(int) || type == typeof(bool) || type == typeof(string) || type.IsEnum;
		}

		public static void SaveDataObject(object obj, string filePath)
		{
			try
			{
				XDocument xDocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(obj, obj.GetType());
				xDocument.Add(content);
				xDocument.Save(filePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(filePath, ex.ToString()));
				Log.Error("Exception saving data object " + obj + ": " + ex);
			}
		}

		public static XElement XElementFromObject(object obj, Type expectedClass)
		{
			return DirectXmlSaver.XElementFromObject(obj, expectedClass, expectedClass.Name, null, false);
		}

		public static XElement XElementFromObject(object obj, Type expectedType, string nodeName, FieldInfo owningField = null, bool saveDefsAsRefs = false)
		{
			DefaultValueAttribute defaultValueAttribute = default(DefaultValueAttribute);
			XElement result;
			if (owningField != null && ((MemberInfo)owningField).TryGetAttribute<DefaultValueAttribute>(out defaultValueAttribute) && defaultValueAttribute.ObjIsDefault(obj))
			{
				result = null;
			}
			else if (obj == null)
			{
				XElement xElement = new XElement(nodeName);
				xElement.SetAttributeValue("IsNull", "True");
				result = xElement;
			}
			else
			{
				Type type = obj.GetType();
				XElement xElement2 = new XElement(nodeName);
				if (DirectXmlSaver.IsSimpleTextType(type))
				{
					xElement2.Add(new XText(obj.ToString()));
				}
				else if (saveDefsAsRefs && typeof(Def).IsAssignableFrom(type))
				{
					string defName = ((Def)obj).defName;
					xElement2.Add(new XText(defName));
				}
				else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
				{
					Type expectedType2 = type.GetGenericArguments()[0];
					int num = (int)type.GetProperty("Count").GetValue(obj, null);
					for (int num2 = 0; num2 < num; num2++)
					{
						object[] index = new object[1]
						{
							num2
						};
						object value = type.GetProperty("Item").GetValue(obj, index);
						XNode content = DirectXmlSaver.XElementFromObject(value, expectedType2, "li", null, true);
						xElement2.Add(content);
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
							object current = enumerator.Current;
							object value2 = current.GetType().GetProperty("Key").GetValue(current, null);
							object value3 = current.GetType().GetProperty("Value").GetValue(current, null);
							XElement xElement3 = new XElement("li");
							xElement3.Add(DirectXmlSaver.XElementFromObject(value2, expectedType3, "key", null, true));
							xElement3.Add(DirectXmlSaver.XElementFromObject(value3, expectedType4, "value", null, true));
							xElement2.Add(xElement3);
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
						xElement2.Add(content2);
					}
					foreach (FieldInfo item in from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby f.MetadataToken
					select f)
					{
						try
						{
							XElement xElement4 = DirectXmlSaver.XElementFromField(item, obj);
							if (xElement4 != null)
							{
								xElement2.Add(xElement4);
							}
						}
						catch
						{
							throw;
						}
					}
				}
				result = xElement2;
			}
			return result;
		}

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
