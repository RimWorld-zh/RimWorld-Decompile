using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Verse
{
	public static class TranslationHandleUtility
	{
		public const char HandleIndexCharacter = '-';

		private static Regex StringFormatSymbolsRegex = new Regex("{[0-9]*}");

		public static int GetElementIndexByHandle(object list, string handle, int handleIndex)
		{
			if (list == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on null object.");
			}
			if (handleIndex < 0)
			{
				handleIndex = 0;
			}
			PropertyInfo property = list.GetType().GetProperty("Count");
			if (property == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on non-list (missing 'Count' property).");
			}
			PropertyInfo property2 = list.GetType().GetProperty("Item");
			if (property2 == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on non-list (missing 'Item' property).");
			}
			int num = (int)property.GetValue(list, null);
			FieldInfo fieldInfo = null;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				object value = property2.GetValue(list, new object[]
				{
					i
				});
				if (value != null)
				{
					FieldInfo[] fields = value.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo fieldInfo2 in fields)
					{
						TranslationHandleAttribute translationHandleAttribute = fieldInfo2.TryGetAttribute<TranslationHandleAttribute>();
						if (translationHandleAttribute != null)
						{
							object value2 = fieldInfo2.GetValue(value);
							if (value2 != null)
							{
								if (TranslationHandleUtility.HandlesMatch(value2, handle))
								{
									int priority = translationHandleAttribute.Priority;
									if (fieldInfo == null || priority > num2)
									{
										fieldInfo = fieldInfo2;
										num2 = priority;
									}
								}
							}
						}
					}
				}
			}
			if (fieldInfo == null)
			{
				throw new InvalidOperationException("None of the list elements have a handle named " + handle + ".");
			}
			int num3 = 0;
			for (int k = 0; k < num; k++)
			{
				object value3 = property2.GetValue(list, new object[]
				{
					k
				});
				if (value3 != null)
				{
					FieldInfo[] fields2 = value3.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo fieldInfo3 in fields2)
					{
						if (TranslationHandleUtility.FieldInfosEqual(fieldInfo3, fieldInfo))
						{
							object value4 = fieldInfo3.GetValue(value3);
							if (value4 != null)
							{
								if (TranslationHandleUtility.HandlesMatch(value4, handle))
								{
									if (num3 == handleIndex)
									{
										return k;
									}
									num3++;
								}
							}
						}
					}
				}
			}
			throw new InvalidOperationException(string.Concat(new object[]
			{
				"Tried to access handle ",
				handle,
				"[",
				handleIndex,
				"], but there are only ",
				num3,
				" handles matching this name."
			}));
		}

		public static string GetBestHandleWithIndexForListElement(object list, object element)
		{
			string result;
			if (list == null || element == null)
			{
				result = null;
			}
			else
			{
				PropertyInfo property = list.GetType().GetProperty("Count");
				if (property == null)
				{
					result = null;
				}
				else
				{
					PropertyInfo property2 = list.GetType().GetProperty("Item");
					if (property2 == null)
					{
						result = null;
					}
					else
					{
						FieldInfo fieldInfo = null;
						string handle = null;
						int num = 0;
						FieldInfo[] fields = element.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						foreach (FieldInfo fieldInfo2 in fields)
						{
							TranslationHandleAttribute translationHandleAttribute = fieldInfo2.TryGetAttribute<TranslationHandleAttribute>();
							if (translationHandleAttribute != null)
							{
								object value = fieldInfo2.GetValue(element);
								if (value != null)
								{
									Type type = value as Type;
									string text;
									if (type != null)
									{
										text = type.Name;
									}
									else
									{
										try
										{
											text = value.ToString();
										}
										catch
										{
											return null;
										}
									}
									if (!text.NullOrEmpty())
									{
										int priority = translationHandleAttribute.Priority;
										if (fieldInfo == null || priority > num)
										{
											fieldInfo = fieldInfo2;
											handle = text;
											num = priority;
										}
									}
								}
							}
						}
						if (fieldInfo == null)
						{
							result = null;
						}
						else
						{
							int num2 = 0;
							int num3 = -1;
							int num4 = (int)property.GetValue(list, null);
							for (int j = 0; j < num4; j++)
							{
								object value2 = property2.GetValue(list, new object[]
								{
									j
								});
								if (value2 != null)
								{
									if (value2 == element)
									{
										num3 = num2;
										num2++;
									}
									else
									{
										FieldInfo[] fields2 = value2.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
										foreach (FieldInfo fieldInfo3 in fields2)
										{
											if (TranslationHandleUtility.FieldInfosEqual(fieldInfo3, fieldInfo))
											{
												object value3 = fieldInfo3.GetValue(value2);
												if (value3 != null)
												{
													if (TranslationHandleUtility.HandlesMatch(value3, handle))
													{
														num2++;
														break;
													}
												}
											}
										}
									}
								}
							}
							if (num3 < 0)
							{
								result = null;
							}
							else
							{
								string text2 = TranslationHandleUtility.NormalizedHandle(handle);
								if (num2 <= 1)
								{
									result = text2;
								}
								else
								{
									result = text2 + '-' + num3;
								}
							}
						}
					}
				}
			}
			return result;
		}

		public static bool HandlesMatch(object item, string handle)
		{
			bool result;
			if (item == null)
			{
				result = false;
			}
			else if (handle.NullOrEmpty())
			{
				result = false;
			}
			else
			{
				handle = TranslationHandleUtility.NormalizedHandle(handle);
				if (handle.NullOrEmpty())
				{
					result = false;
				}
				else
				{
					Type type = item as Type;
					if (type != null)
					{
						result = (TranslationHandleUtility.NormalizedHandle(type.Name) == handle || TranslationHandleUtility.NormalizedHandle(type.FullName) == handle || TranslationHandleUtility.NormalizedHandle(type.ToString()) == handle);
					}
					else
					{
						string text;
						try
						{
							text = item.ToString();
						}
						catch (Exception arg)
						{
							throw new InvalidOperationException("Could not get element by handle because one of the elements threw an exception in its ToString(): " + arg);
						}
						result = (!text.NullOrEmpty() && TranslationHandleUtility.NormalizedHandle(text) == handle);
					}
				}
			}
			return result;
		}

		private static string NormalizedHandle(string handle)
		{
			string result;
			if (handle.NullOrEmpty())
			{
				result = handle;
			}
			else
			{
				handle = handle.Trim();
				handle = handle.Replace(' ', '_');
				handle = handle.Replace('\n', '_');
				handle = handle.Replace("\r", "");
				handle = handle.Replace('\t', '_');
				handle = handle.Replace(".", "");
				if (handle.IndexOf('-') >= 0)
				{
					handle = handle.Replace('-'.ToString(), "");
				}
				if (handle.IndexOf("{") >= 0)
				{
					handle = TranslationHandleUtility.StringFormatSymbolsRegex.Replace(handle, "");
				}
				string text = "(){}[]<>,/?;:'’\"„”‘’‛’|\\+=`~!@#$%^&* \t\r\n";
				for (int i = 0; i < text.Length; i++)
				{
					if (handle.IndexOf(text[i]) >= 0)
					{
						handle = handle.Replace(text[i].ToString(), "");
					}
				}
				handle = handle.Replace("___", "_");
				handle = handle.Replace("__", "_");
				handle = handle.Trim(new char[]
				{
					'_'
				});
				result = handle;
			}
			return result;
		}

		private static bool FieldInfosEqual(FieldInfo lhs, FieldInfo rhs)
		{
			return lhs.DeclaringType == rhs.DeclaringType && lhs.Name == rhs.Name;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static TranslationHandleUtility()
		{
		}
	}
}
