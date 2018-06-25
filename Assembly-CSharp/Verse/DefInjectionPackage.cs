using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

namespace Verse
{
	public class DefInjectionPackage
	{
		public Type defType;

		private Dictionary<string, string> injections = new Dictionary<string, string>();

		private Dictionary<string, List<string>> fullListInjections = new Dictionary<string, List<string>>();

		private Dictionary<string, string> injectionsFileSource = new Dictionary<string, string>();

		private Dictionary<string, string> fullListInjectionsFileSource = new Dictionary<string, string>();

		public List<Pair<string, string>> autoFixedBackCompatKeys = new List<Pair<string, string>>();

		public List<string> loadErrors = new List<string>();

		public List<string> loadSyntaxSuggestions = new List<string>();

		public bool usedOldRepSyntax;

		public const BindingFlags FieldBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public const string RepNodeName = "rep";

		[CompilerGenerated]
		private static Func<XElement, bool> <>f__am$cache0;

		public DefInjectionPackage(Type defType)
		{
			this.defType = defType;
		}

		private string ProcessedPath(string path)
		{
			string result;
			if (!path.Contains('[') && !path.Contains(']'))
			{
				result = path;
			}
			else
			{
				result = path.Replace("]", "").Replace('[', '.');
			}
			return result;
		}

		private string ProcessedTranslation(string rawTranslation)
		{
			return rawTranslation.Replace("\\n", "\n");
		}

		public void AddDataFromFile(FileInfo file)
		{
			try
			{
				XDocument xdocument = XDocument.Load(file.FullName);
				foreach (XElement xelement in xdocument.Root.Elements())
				{
					if (xelement.Name == "rep")
					{
						string key = this.ProcessedPath(xelement.Elements("path").First<XElement>().Value);
						string translation = this.ProcessedTranslation(xelement.Elements("trans").First<XElement>().Value);
						this.TryAddInjection(file, key, translation);
						this.usedOldRepSyntax = true;
					}
					else
					{
						string text = this.ProcessedPath(xelement.Name.ToString());
						if (xelement.HasElements)
						{
							List<string> list = new List<string>();
							foreach (XElement xelement2 in xelement.Elements("li"))
							{
								list.Add(this.ProcessedTranslation(xelement2.Value));
							}
							if (xelement.Elements().Any((XElement x) => x.Name != "li"))
							{
								this.loadErrors.Add(text + " has elements which are not 'li' (" + file.Name + ")");
							}
							this.TryAddFullListInjection(file, text, list);
						}
						else
						{
							string translation2 = this.ProcessedTranslation(xelement.Value);
							this.TryAddInjection(file, text, translation2);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading translation data from file ",
					file.Name,
					": ",
					ex
				}));
			}
		}

		private void TryAddInjection(FileInfo file, string key, string translation)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (!this.CheckErrors(file, key, text, false))
			{
				this.injections.Add(key, translation);
				this.injectionsFileSource.Add(key, file.Name);
				if (key != text)
				{
					this.autoFixedBackCompatKeys.Add(new Pair<string, string>(text, key));
				}
			}
		}

		private void TryAddFullListInjection(FileInfo file, string key, List<string> translation)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (!this.CheckErrors(file, key, text, true))
			{
				if (translation == null)
				{
					translation = new List<string>();
				}
				this.fullListInjections.Add(key, translation);
				this.fullListInjectionsFileSource.Add(key, file.Name);
				if (key != text)
				{
					this.autoFixedBackCompatKeys.Add(new Pair<string, string>(text, key));
				}
			}
		}

		private string BackCompatibleKey(string key)
		{
			string[] array = key.Split(new char[]
			{
				'.'
			});
			array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0], true);
			return string.Join(".", array);
		}

		private bool CheckErrors(FileInfo file, string key, string nonBackCompatibleKey, bool replacingFullList)
		{
			bool result;
			if (!key.Contains('.'))
			{
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Error loading DefInjection from file ",
					file.Name,
					": Key lacks a dot: ",
					key,
					(!(key == nonBackCompatibleKey)) ? (" (auto-renamed from " + nonBackCompatibleKey + ")") : "",
					" (",
					file.Name,
					")"
				}));
				result = true;
			}
			else if (this.injections.ContainsKey(key) || this.fullListInjections.ContainsKey(key))
			{
				string text;
				if (key != nonBackCompatibleKey)
				{
					text = " (auto-renamed from " + nonBackCompatibleKey + ")";
				}
				else if (this.autoFixedBackCompatKeys.Any((Pair<string, string> x) => x.Second == key))
				{
					Pair<string, string> pair = this.autoFixedBackCompatKeys.Find((Pair<string, string> x) => x.Second == key);
					text = string.Concat(new string[]
					{
						" (",
						pair.First,
						" was auto-renamed to ",
						pair.Second,
						")"
					});
				}
				else
				{
					text = "";
				}
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Duplicate def-injected translation key: ",
					key,
					text,
					" (",
					file.Name,
					")"
				}));
				result = true;
			}
			else
			{
				bool flag = false;
				if (replacingFullList)
				{
					if (this.injections.Any((KeyValuePair<string, string> x) => x.Key.StartsWith(key + ".")))
					{
						flag = true;
					}
				}
				else if (key.Contains('.') && char.IsNumber(key[key.Length - 1]))
				{
					string key2 = key.Substring(0, key.LastIndexOf('.'));
					if (this.fullListInjections.ContainsKey(key2))
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.loadErrors.Add(string.Concat(new string[]
					{
						"Replacing the whole list and individual elements at the same time doesn't make sense. Either replace the whole list or translate individual elements by using their index. key=",
						key,
						(!(key == nonBackCompatibleKey)) ? (" (auto-renamed from " + nonBackCompatibleKey + ")") : "",
						" (",
						file.Name,
						")"
					}));
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public void InjectIntoDefs(bool errorOnDefNotFound)
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.injections)
			{
				this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value, typeof(string), errorOnDefNotFound, this.injectionsFileSource[keyValuePair.Key]);
			}
			foreach (KeyValuePair<string, List<string>> keyValuePair2 in this.fullListInjections)
			{
				this.SetDefFieldAtPath(this.defType, keyValuePair2.Key, keyValuePair2.Value, typeof(List<string>), errorOnDefNotFound, this.fullListInjectionsFileSource[keyValuePair2.Key]);
			}
			GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "ClearCachedData");
		}

		private void SetDefFieldAtPath(Type defType, string path, object value, Type ensureFieldType, bool errorOnDefNotFound, string fileSource)
		{
			int num = 0;
			string text = path.Split(new char[]
			{
				'.'
			})[0];
			text = BackCompatibility.BackCompatibleDefName(defType, text, true);
			if (GenDefDatabase.GetDefSilentFail(defType, text, false) == null)
			{
				if (errorOnDefNotFound)
				{
					this.loadErrors.Add(string.Concat(new object[]
					{
						"Found no ",
						defType,
						" named ",
						text,
						" to match ",
						path,
						" (",
						fileSource,
						")"
					}));
				}
			}
			else
			{
				path = BackCompatibility.BackCompatibleModifiedTranslationPath(defType, path, this.loadSyntaxSuggestions);
				bool flag = false;
				string text2 = path;
				try
				{
					List<string> list = path.Split(new char[]
					{
						'.'
					}).ToList<string>();
					object obj = GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamedSilentFail", new object[]
					{
						list[0]
					});
					if (obj == null)
					{
						throw new InvalidOperationException("Def named " + list[0] + " not found.");
					}
					list.RemoveAt(0);
					num++;
					string text3;
					int num2;
					DefInjectionPathPartKind defInjectionPathPartKind;
					FieldInfo field;
					int num3;
					int num4;
					for (;;)
					{
						text3 = list[0];
						num2 = -1;
						if (int.TryParse(text3, out num2))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
						}
						else if (this.GetFieldNamed(obj.GetType(), text3) != null)
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.Field;
						}
						else if (obj.GetType().GetProperty("Count") != null)
						{
							if (text3.Contains('-'))
							{
								defInjectionPathPartKind = DefInjectionPathPartKind.ListHandleWithIndex;
								string[] array = text3.Split(new char[]
								{
									'-'
								});
								text3 = array[0];
								num2 = (int)ParseHelper.FromString(array[1], typeof(int));
							}
							else
							{
								defInjectionPathPartKind = DefInjectionPathPartKind.ListHandle;
							}
						}
						else
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.Field;
						}
						if (list.Count == 1)
						{
							break;
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
						{
							field = obj.GetType().GetField(text3, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (field == null)
							{
								goto Block_31;
							}
							if (field.HasAttribute<NoTranslateAttribute>())
							{
								goto Block_32;
							}
							if (field.HasAttribute<UnsavedAttribute>())
							{
								goto Block_33;
							}
							if (field.HasAttribute<TranslationCanChangeCountAttribute>())
							{
								flag = true;
							}
							if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
							{
								obj = field.GetValue(obj);
							}
							else
							{
								object value2 = field.GetValue(obj);
								PropertyInfo property = value2.GetType().GetProperty("Item");
								if (property == null)
								{
									goto Block_36;
								}
								num3 = (int)value2.GetType().GetProperty("Count").GetValue(value2, null);
								if (num2 < 0 || num2 >= num3)
								{
									goto IL_8C6;
								}
								obj = property.GetValue(value2, new object[]
								{
									num2
								});
							}
						}
						else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							object obj2 = obj;
							PropertyInfo property2 = obj2.GetType().GetProperty("Item");
							if (property2 == null)
							{
								goto Block_40;
							}
							bool flag2;
							if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
							{
								num2 = TranslationHandleUtility.GetElementIndexByHandle(obj2, text3, num2);
								flag2 = true;
							}
							else
							{
								flag2 = false;
							}
							num4 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2, null);
							if (num2 < 0 || num2 >= num4)
							{
								goto IL_9A6;
							}
							obj = property2.GetValue(obj2, new object[]
							{
								num2
							});
							if (!flag2)
							{
								string bestHandleWithIndexForListElement = TranslationHandleUtility.GetBestHandleWithIndexForListElement(obj2, obj);
								if (!bestHandleWithIndexForListElement.NullOrEmpty())
								{
									string[] array2 = text2.Split(new char[]
									{
										'.'
									});
									array2[num] = bestHandleWithIndexForListElement;
									text2 = string.Join(".", array2);
								}
							}
						}
						else
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Can't enter node ",
								text3,
								" at path ",
								path,
								", element kind is ",
								defInjectionPathPartKind,
								". (",
								fileSource,
								")"
							}));
						}
						list.RemoveAt(0);
						num++;
					}
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						FieldInfo fieldNamed = this.GetFieldNamed(obj.GetType(), text3);
						if (fieldNamed == null)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Field ",
								text3,
								" does not exist in type ",
								obj.GetType(),
								"."
							}));
						}
						if (fieldNamed.HasAttribute<NoTranslateAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated untranslatable field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								path,
								". Translating this field will break the game. (",
								fileSource,
								")"
							}));
						}
						else if (fieldNamed.HasAttribute<UnsavedAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated untranslatable field (UnsavedAttribute) ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								path,
								". Translating this field will break the game. (",
								fileSource,
								")"
							}));
						}
						else if (fieldNamed.FieldType != ensureFieldType)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated non-",
								ensureFieldType,
								" field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								path,
								". Expected ",
								ensureFieldType,
								". (",
								fileSource,
								")"
							}));
						}
						else if (ensureFieldType != typeof(string) && !fieldNamed.HasAttribute<TranslationCanChangeCountAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Tried to translate field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								path,
								", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (",
								fileSource,
								")"
							}));
						}
						else
						{
							fieldNamed.SetValue(obj, value);
						}
					}
					else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
					{
						object obj3 = obj;
						if (obj3 == null)
						{
							throw new InvalidOperationException("Tried to use index on null list at " + path);
						}
						Type type = obj3.GetType();
						PropertyInfo property3 = type.GetProperty("Count");
						if (property3 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Count' property).");
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj3, text3, num2);
						}
						int num5 = (int)property3.GetValue(obj3, null);
						if (num2 >= num5)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Trying to translate ",
								defType,
								".",
								path,
								" at index ",
								num2,
								" but the list only has ",
								num5,
								" entries (so max index is ",
								(num5 - 1).ToString(),
								")."
							}));
						}
						PropertyInfo property4 = type.GetProperty("Item");
						if (property4 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
						}
						Type propertyType = property4.PropertyType;
						if (propertyType != ensureFieldType)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated non-",
								ensureFieldType,
								" list item of type ",
								propertyType,
								" at path ",
								path,
								". Expected ",
								ensureFieldType,
								". (",
								fileSource,
								")"
							}));
						}
						else if (ensureFieldType != typeof(string) && !flag)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Tried to translate field of type ",
								propertyType,
								" at path ",
								path,
								", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (",
								fileSource,
								")"
							}));
						}
						else if (num2 < 0 || num2 >= (int)type.GetProperty("Count").GetValue(obj3, null))
						{
							this.loadErrors.Add("Index out of bounds (max index is " + ((int)type.GetProperty("Count").GetValue(obj3, null) - 1) + ")");
						}
						else
						{
							property4.SetValue(obj3, value, new object[]
							{
								num2
							});
						}
					}
					else
					{
						this.loadErrors.Add(string.Concat(new object[]
						{
							"Translated ",
							text3,
							" at path ",
							path,
							" but it's not a field, it's ",
							defInjectionPathPartKind,
							". (",
							fileSource,
							")"
						}));
					}
					if (path != text2)
					{
						this.loadSyntaxSuggestions.Add(string.Concat(new string[]
						{
							"Consider using ",
							text2,
							" instead of ",
							path,
							" (",
							fileSource,
							")"
						}));
					}
					return;
					Block_31:
					throw new InvalidOperationException("Field " + text3 + " does not exist.");
					Block_32:
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"Translated untranslatable field ",
						field.Name,
						" of type ",
						field.FieldType,
						" at path ",
						path,
						". Translating this field will break the game."
					}));
					Block_33:
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"Translated untranslatable field ([Unsaved] attribute) ",
						field.Name,
						" of type ",
						field.FieldType,
						" at path ",
						path,
						". Translating this field will break the game."
					}));
					Block_36:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_8C6:
					throw new InvalidOperationException("Index out of bounds (max index is " + (num3 - 1) + ")");
					Block_40:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_9A6:
					throw new InvalidOperationException("Index out of bounds (max index is " + (num4 - 1) + ")");
				}
				catch (Exception ex)
				{
					string text4 = string.Concat(new object[]
					{
						"Couldn't inject ",
						path,
						" into ",
						defType,
						" (",
						fileSource,
						"): ",
						ex.Message
					});
					if (ex.InnerException != null)
					{
						text4 = text4 + " -> " + ex.InnerException.Message;
					}
					this.loadErrors.Add(text4);
				}
			}
		}

		private FieldInfo GetFieldNamed(Type type, string name)
		{
			FieldInfo field = type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					object[] customAttributes = fields[i].GetCustomAttributes(typeof(LoadAliasAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						foreach (LoadAliasAttribute loadAliasAttribute in customAttributes)
						{
							if (loadAliasAttribute.alias == name)
							{
								return fields[i];
							}
						}
					}
				}
			}
			return field;
		}

		public IEnumerable<string> MissingInjections(List<string> outUnnecessaryDefInjections)
		{
			Type databaseType = typeof(DefDatabase<>).MakeGenericType(new Type[]
			{
				this.defType
			});
			PropertyInfo allDefsProperty = databaseType.GetProperty("AllDefs");
			MethodInfo allDefsMethod = allDefsProperty.GetGetMethod();
			IEnumerable allDefsEnum = (IEnumerable)allDefsMethod.Invoke(null, null);
			IEnumerator enumerator = allDefsEnum.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Def def = (Def)obj;
					foreach (string mi in this.MissingInjectionsFromDef(def, outUnnecessaryDefInjections))
					{
						yield return mi;
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
			yield break;
		}

		private IEnumerable<string> MissingInjectionsFromDef(Def def, List<string> outUnnecessaryDefInjections)
		{
			HashSet<object> visited = new HashSet<object>();
			foreach (string missing in this.MissingInjectionsFromDefRecursive(def, def.defName, visited, outUnnecessaryDefInjections, true, def.generated))
			{
				yield return missing;
			}
			yield break;
		}

		private IEnumerable<string> MissingInjectionsFromDefRecursive(object obj, string curPath, HashSet<object> visited, List<string> outUnnecessaryDefInjections, bool translationAllowed, bool defGenerated)
		{
			if (obj == null)
			{
				yield break;
			}
			if (visited.Contains(obj))
			{
				yield break;
			}
			visited.Add(obj);
			foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object val = fi.GetValue(obj);
				bool thisFieldTranslationAllowed = translationAllowed && !fi.HasAttribute<NoTranslateAttribute>() && !fi.HasAttribute<UnsavedAttribute>();
				if (!(val is Def))
				{
					if (typeof(string).IsAssignableFrom(fi.FieldType))
					{
						string str = (string)val;
						string path = curPath + "." + fi.Name;
						if (this.injections.ContainsKey(path))
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(path + " '" + this.injections[path].Replace("\n", "\\n") + "'");
							}
						}
						else if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(str, fi))
						{
							yield return path + " '" + str.Replace("\n", "\\n") + "'";
						}
					}
					else if (val is IEnumerable<string>)
					{
						IEnumerable<string> collection = (IEnumerable<string>)val;
						bool allowFullListTranslation = fi.HasAttribute<TranslationCanChangeCountAttribute>();
						if (this.fullListInjections.ContainsKey(curPath + "." + fi.Name))
						{
							string text = curPath + "." + fi.Name;
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(text + " '" + this.fullListInjections[text].ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
							}
						}
						else
						{
							int i = 0;
							foreach (string elem in collection)
							{
								string path2 = string.Concat(new object[]
								{
									curPath,
									".",
									fi.Name,
									".",
									i
								});
								if (this.injections.ContainsKey(path2))
								{
									if (!thisFieldTranslationAllowed)
									{
										outUnnecessaryDefInjections.Add(path2 + " '" + this.injections[path2].Replace("\n", "\\n") + "'");
									}
								}
								else if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(elem, fi))
								{
									yield return string.Concat(new string[]
									{
										path2,
										" '",
										elem.Replace("\n", "\\n"),
										"'",
										(!allowFullListTranslation) ? "" : " (hint: this list allows full-list translation by using <li> nodes)"
									});
								}
								i++;
							}
						}
					}
					else if (val is IEnumerable)
					{
						IEnumerable collection2 = (IEnumerable)val;
						int j = 0;
						IEnumerator enumerator2 = collection2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object elem2 = enumerator2.Current;
								if (elem2 != null && !(elem2 is Def) && GenTypes.IsCustomType(elem2.GetType()))
								{
									string handleOrIndex = TranslationHandleUtility.GetBestHandleWithIndexForListElement(collection2, elem2);
									if (handleOrIndex.NullOrEmpty())
									{
										handleOrIndex = j.ToString();
									}
									foreach (string missing in this.MissingInjectionsFromDefRecursive(elem2, string.Concat(new string[]
									{
										curPath,
										".",
										fi.Name,
										".",
										handleOrIndex
									}), visited, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated))
									{
										yield return missing;
									}
								}
								j++;
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
					}
					else if (val != null && GenTypes.IsCustomType(val.GetType()))
					{
						foreach (string missing2 in this.MissingInjectionsFromDefRecursive(val, curPath + "." + fi.Name, visited, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated))
						{
							yield return missing2;
						}
					}
				}
			}
			yield break;
		}

		private static bool ShouldCheckMissingInjection(string str, FieldInfo fi)
		{
			return !str.NullOrEmpty() && !fi.HasAttribute<NoTranslateAttribute>() && !fi.HasAttribute<UnsavedAttribute>() && !fi.HasAttribute<MayTranslateAttribute>() && (fi.HasAttribute<MustTranslateAttribute>() || str.Contains(' '));
		}

		[CompilerGenerated]
		private static bool <AddDataFromFile>m__0(XElement x)
		{
			return x.Name != "li";
		}

		[CompilerGenerated]
		private sealed class <CheckErrors>c__AnonStorey3
		{
			internal string key;

			public <CheckErrors>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Pair<string, string> x)
			{
				return x.Second == this.key;
			}

			internal bool <>m__1(Pair<string, string> x)
			{
				return x.Second == this.key;
			}

			internal bool <>m__2(KeyValuePair<string, string> x)
			{
				return x.Key.StartsWith(this.key + ".");
			}
		}

		[CompilerGenerated]
		private sealed class <MissingInjections>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal Type <databaseType>__0;

			internal PropertyInfo <allDefsProperty>__0;

			internal MethodInfo <allDefsMethod>__0;

			internal IEnumerable <allDefsEnum>__0;

			internal IEnumerator $locvar0;

			internal Def <def>__1;

			internal IDisposable $locvar1;

			internal List<string> outUnnecessaryDefInjections;

			internal IEnumerator<string> $locvar2;

			internal string <mi>__2;

			internal DefInjectionPackage $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MissingInjections>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					databaseType = typeof(DefDatabase<>).MakeGenericType(new Type[]
					{
						this.defType
					});
					allDefsProperty = databaseType.GetProperty("AllDefs");
					allDefsMethod = allDefsProperty.GetGetMethod();
					allDefsEnum = (IEnumerable)allDefsMethod.Invoke(null, null);
					enumerator = allDefsEnum.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								mi = enumerator2.Current;
								this.$current = mi;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						def = (Def)enumerator.Current;
						enumerator2 = base.MissingInjectionsFromDef(def, outUnnecessaryDefInjections).GetEnumerator();
						num = 4294967293u;
						goto Block_4;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DefInjectionPackage.<MissingInjections>c__Iterator0 <MissingInjections>c__Iterator = new DefInjectionPackage.<MissingInjections>c__Iterator0();
				<MissingInjections>c__Iterator.$this = this;
				<MissingInjections>c__Iterator.outUnnecessaryDefInjections = outUnnecessaryDefInjections;
				return <MissingInjections>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <MissingInjectionsFromDef>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal HashSet<object> <visited>__0;

			internal Def def;

			internal List<string> outUnnecessaryDefInjections;

			internal IEnumerator<string> $locvar0;

			internal string <missing>__1;

			internal DefInjectionPackage $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MissingInjectionsFromDef>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					visited = new HashSet<object>();
					enumerator = base.MissingInjectionsFromDefRecursive(def, def.defName, visited, outUnnecessaryDefInjections, true, def.generated).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						missing = enumerator.Current;
						this.$current = missing;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DefInjectionPackage.<MissingInjectionsFromDef>c__Iterator1 <MissingInjectionsFromDef>c__Iterator = new DefInjectionPackage.<MissingInjectionsFromDef>c__Iterator1();
				<MissingInjectionsFromDef>c__Iterator.$this = this;
				<MissingInjectionsFromDef>c__Iterator.def = def;
				<MissingInjectionsFromDef>c__Iterator.outUnnecessaryDefInjections = outUnnecessaryDefInjections;
				return <MissingInjectionsFromDef>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <MissingInjectionsFromDefRecursive>c__Iterator2 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal object obj;

			internal HashSet<object> visited;

			internal FieldInfo[] $locvar0;

			internal int $locvar1;

			internal FieldInfo <fi>__1;

			internal object <val>__2;

			internal bool translationAllowed;

			internal bool <thisFieldTranslationAllowed>__2;

			internal string <str>__3;

			internal string curPath;

			internal string <path>__3;

			internal List<string> outUnnecessaryDefInjections;

			internal bool defGenerated;

			internal IEnumerable<string> <collection>__4;

			internal bool <allowFullListTranslation>__4;

			internal int <i>__5;

			internal IEnumerator<string> $locvar2;

			internal string <elem>__6;

			internal string <path>__7;

			internal IEnumerable <collection>__8;

			internal int <i>__8;

			internal IEnumerator $locvar3;

			internal object <elem>__9;

			internal IDisposable $locvar4;

			internal string <handleOrIndex>__10;

			internal IEnumerator<string> $locvar5;

			internal string <missing>__11;

			internal IEnumerator<string> $locvar6;

			internal string <missing>__12;

			internal DefInjectionPackage $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MissingInjectionsFromDefRecursive>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (obj == null)
					{
						return false;
					}
					if (visited.Contains(obj))
					{
						return false;
					}
					visited.Add(obj);
					fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					k = 0;
					goto IL_7F5;
				case 1u:
					IL_238:
					break;
				case 2u:
					Block_17:
					try
					{
						switch (num)
						{
						case 2u:
							IL_4A8:
							i++;
							break;
						}
						if (enumerator.MoveNext())
						{
							elem = enumerator.Current;
							path2 = string.Concat(new object[]
							{
								curPath,
								".",
								fi.Name,
								".",
								i
							});
							if (this.injections.ContainsKey(path2))
							{
								if (!thisFieldTranslationAllowed)
								{
									outUnnecessaryDefInjections.Add(path2 + " '" + this.injections[path2].Replace("\n", "\\n") + "'");
								}
								goto IL_4A8;
							}
							if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(elem, fi))
							{
								this.$current = string.Concat(new string[]
								{
									path2,
									" '",
									elem.Replace("\n", "\\n"),
									"'",
									(!allowFullListTranslation) ? "" : " (hint: this list allows full-list translation by using <li> nodes)"
								});
								if (!this.$disposing)
								{
									this.$PC = 2;
								}
								flag = true;
								return true;
							}
							goto IL_4A8;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					break;
				case 3u:
					Block_19:
					try
					{
						switch (num)
						{
						case 3u:
							Block_41:
							try
							{
								switch (num)
								{
								}
								if (enumerator3.MoveNext())
								{
									missing = enumerator3.Current;
									this.$current = missing;
									if (!this.$disposing)
									{
										this.$PC = 3;
									}
									flag = true;
									return true;
								}
							}
							finally
							{
								if (!flag)
								{
									if (enumerator3 != null)
									{
										enumerator3.Dispose();
									}
								}
							}
							break;
						default:
							goto IL_6B7;
						}
						IL_6A8:
						j++;
						IL_6B7:
						if (enumerator2.MoveNext())
						{
							elem2 = enumerator2.Current;
							if (elem2 != null && !(elem2 is Def) && GenTypes.IsCustomType(elem2.GetType()))
							{
								handleOrIndex = TranslationHandleUtility.GetBestHandleWithIndexForListElement(collection2, elem2);
								if (handleOrIndex.NullOrEmpty())
								{
									handleOrIndex = j.ToString();
								}
								enumerator3 = base.MissingInjectionsFromDefRecursive(elem2, string.Concat(new string[]
								{
									curPath,
									".",
									fi.Name,
									".",
									handleOrIndex
								}), visited, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated).GetEnumerator();
								num = 4294967293u;
								goto Block_41;
							}
							goto IL_6A8;
						}
					}
					finally
					{
						if (!flag)
						{
							if ((disposable = (enumerator2 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
					break;
				case 4u:
					Block_22:
					try
					{
						switch (num)
						{
						}
						if (enumerator4.MoveNext())
						{
							missing2 = enumerator4.Current;
							this.$current = missing2;
							if (!this.$disposing)
							{
								this.$PC = 4;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator4 != null)
							{
								enumerator4.Dispose();
							}
						}
					}
					break;
				default:
					return false;
				}
				IL_4E8:
				IL_7E6:
				IL_7E7:
				k++;
				IL_7F5:
				if (k >= fields.Length)
				{
					this.$PC = -1;
				}
				else
				{
					fi = fields[k];
					val = fi.GetValue(obj);
					thisFieldTranslationAllowed = (translationAllowed && !fi.HasAttribute<NoTranslateAttribute>() && !fi.HasAttribute<UnsavedAttribute>());
					if (val is Def)
					{
						goto IL_7E7;
					}
					if (typeof(string).IsAssignableFrom(fi.FieldType))
					{
						str = (string)val;
						path = curPath + "." + fi.Name;
						if (this.injections.ContainsKey(path))
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(path + " '" + this.injections[path].Replace("\n", "\\n") + "'");
							}
							goto IL_238;
						}
						if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(str, fi))
						{
							this.$current = path + " '" + str.Replace("\n", "\\n") + "'";
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
						goto IL_238;
					}
					else if (val is IEnumerable<string>)
					{
						collection = (IEnumerable<string>)val;
						allowFullListTranslation = fi.HasAttribute<TranslationCanChangeCountAttribute>();
						if (this.fullListInjections.ContainsKey(curPath + "." + fi.Name))
						{
							string text = curPath + "." + fi.Name;
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(text + " '" + this.fullListInjections[text].ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
							}
							goto IL_4E8;
						}
						i = 0;
						enumerator = collection.GetEnumerator();
						num = 4294967293u;
						goto Block_17;
					}
					else
					{
						if (val is IEnumerable)
						{
							collection2 = (IEnumerable)val;
							j = 0;
							enumerator2 = collection2.GetEnumerator();
							num = 4294967293u;
							goto Block_19;
						}
						if (val != null && GenTypes.IsCustomType(val.GetType()))
						{
							enumerator4 = base.MissingInjectionsFromDefRecursive(val, curPath + "." + fi.Name, visited, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated).GetEnumerator();
							num = 4294967293u;
							goto Block_22;
						}
						goto IL_7E6;
					}
				}
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					finally
					{
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				case 4u:
					try
					{
					}
					finally
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DefInjectionPackage.<MissingInjectionsFromDefRecursive>c__Iterator2 <MissingInjectionsFromDefRecursive>c__Iterator = new DefInjectionPackage.<MissingInjectionsFromDefRecursive>c__Iterator2();
				<MissingInjectionsFromDefRecursive>c__Iterator.$this = this;
				<MissingInjectionsFromDefRecursive>c__Iterator.obj = obj;
				<MissingInjectionsFromDefRecursive>c__Iterator.visited = visited;
				<MissingInjectionsFromDefRecursive>c__Iterator.translationAllowed = translationAllowed;
				<MissingInjectionsFromDefRecursive>c__Iterator.curPath = curPath;
				<MissingInjectionsFromDefRecursive>c__Iterator.outUnnecessaryDefInjections = outUnnecessaryDefInjections;
				<MissingInjectionsFromDefRecursive>c__Iterator.defGenerated = defGenerated;
				return <MissingInjectionsFromDefRecursive>c__Iterator;
			}
		}
	}
}
