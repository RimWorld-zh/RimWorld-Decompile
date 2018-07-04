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
using RimWorld;

namespace Verse
{
	public class DefInjectionPackage
	{
		public Type defType;

		public Dictionary<string, DefInjectionPackage.DefInjection> injections = new Dictionary<string, DefInjectionPackage.DefInjection>();

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
			if (path == null)
			{
				path = "";
			}
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
				DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
				defInjection.path = key;
				defInjection.injection = translation;
				defInjection.fileSource = file.Name;
				defInjection.nonBackCompatiblePath = text;
				this.injections.Add(key, defInjection);
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
				DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
				defInjection.path = key;
				defInjection.fullListInjection = translation;
				defInjection.fileSource = file.Name;
				defInjection.nonBackCompatiblePath = text;
				this.injections.Add(key, defInjection);
			}
		}

		private string BackCompatibleKey(string key)
		{
			string[] array = key.Split(new char[]
			{
				'.'
			});
			if (array.Any<string>())
			{
				array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0], true);
			}
			key = string.Join(".", array);
			if (this.defType == typeof(ConceptDef))
			{
				if (key.Contains(".helpTexts.0"))
				{
					key = key.Replace(".helpTexts.0", ".helpText");
				}
			}
			return key;
		}

		private bool CheckErrors(FileInfo file, string key, string nonBackCompatibleKey, bool replacingFullList)
		{
			bool result;
			DefInjectionPackage.DefInjection defInjection;
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
			else if (this.injections.TryGetValue(key, out defInjection))
			{
				string text;
				if (key != nonBackCompatibleKey)
				{
					text = " (auto-renamed from " + nonBackCompatibleKey + ")";
				}
				else if (defInjection.path != defInjection.nonBackCompatiblePath)
				{
					text = string.Concat(new string[]
					{
						" (",
						defInjection.nonBackCompatiblePath,
						" was auto-renamed to ",
						defInjection.path,
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
					if (this.injections.Any((KeyValuePair<string, DefInjectionPackage.DefInjection> x) => !x.Value.IsFullListInjection && x.Key.StartsWith(key + ".")))
					{
						flag = true;
					}
				}
				else if (key.Contains('.') && char.IsNumber(key[key.Length - 1]))
				{
					string key2 = key.Substring(0, key.LastIndexOf('.'));
					if (this.injections.ContainsKey(key2) && this.injections[key2].IsFullListInjection)
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
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				string normalizedPath;
				string suggestedPath;
				if (keyValuePair.Value.IsFullListInjection)
				{
					this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.fullListInjection, typeof(List<string>), errorOnDefNotFound, keyValuePair.Value.fileSource, out normalizedPath, out suggestedPath);
				}
				else
				{
					this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.injection, typeof(string), errorOnDefNotFound, keyValuePair.Value.fileSource, out normalizedPath, out suggestedPath);
				}
				keyValuePair.Value.normalizedPath = normalizedPath;
				keyValuePair.Value.suggestedPath = suggestedPath;
			}
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair2 in this.injections)
			{
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair3 in this.injections)
				{
					if (!(keyValuePair2.Key == keyValuePair3.Key))
					{
						if (keyValuePair2.Value.normalizedPath == keyValuePair3.Value.normalizedPath)
						{
							string text = string.Concat(new string[]
							{
								"Duplicate def-injected translation key. Both ",
								keyValuePair2.Value.path,
								" and ",
								keyValuePair3.Value.path,
								" refer to the same field (",
								keyValuePair2.Value.suggestedPath,
								")"
							});
							if (keyValuePair2.Value.path != keyValuePair2.Value.nonBackCompatiblePath)
							{
								string text2 = text;
								text = string.Concat(new string[]
								{
									text2,
									" (",
									keyValuePair2.Value.nonBackCompatiblePath,
									" was auto-renamed to ",
									keyValuePair2.Value.path,
									")"
								});
							}
							text = text + " (" + keyValuePair2.Value.fileSource + ")";
							this.loadErrors.Add(text);
						}
					}
				}
			}
			GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "ClearCachedData");
		}

		private void SetDefFieldAtPath(Type defType, string path, object value, Type ensureFieldType, bool errorOnDefNotFound, string fileSource, out string normalizedPath, out string suggestedPath)
		{
			normalizedPath = path;
			suggestedPath = path;
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
				bool flag = false;
				int num = 0;
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
					string text2;
					int num2;
					DefInjectionPathPartKind defInjectionPathPartKind;
					FieldInfo field;
					int num3;
					int num4;
					for (;;)
					{
						text2 = list[0];
						num2 = -1;
						if (int.TryParse(text2, out num2))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
						}
						else if (this.GetFieldNamed(obj.GetType(), text2) != null)
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.Field;
						}
						else if (obj.GetType().GetProperty("Count") != null)
						{
							if (text2.Contains('-'))
							{
								defInjectionPathPartKind = DefInjectionPathPartKind.ListHandleWithIndex;
								string[] array = text2.Split(new char[]
								{
									'-'
								});
								text2 = array[0];
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
							field = obj.GetType().GetField(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (field == null)
							{
								goto Block_32;
							}
							if (field.HasAttribute<NoTranslateAttribute>())
							{
								goto Block_33;
							}
							if (field.HasAttribute<UnsavedAttribute>())
							{
								goto Block_34;
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
									goto Block_37;
								}
								num3 = (int)value2.GetType().GetProperty("Count").GetValue(value2, null);
								if (num2 < 0 || num2 >= num3)
								{
									goto IL_8F3;
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
								goto Block_41;
							}
							bool flag2;
							if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
							{
								num2 = TranslationHandleUtility.GetElementIndexByHandle(obj2, text2, num2);
								flag2 = true;
							}
							else
							{
								flag2 = false;
							}
							num4 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2, null);
							if (num2 < 0 || num2 >= num4)
							{
								goto IL_9D3;
							}
							obj = property2.GetValue(obj2, new object[]
							{
								num2
							});
							if (flag2)
							{
								string[] array2 = normalizedPath.Split(new char[]
								{
									'.'
								});
								array2[num] = num2.ToString();
								normalizedPath = string.Join(".", array2);
							}
							else
							{
								string bestHandleWithIndexForListElement = TranslationHandleUtility.GetBestHandleWithIndexForListElement(obj2, obj);
								if (!bestHandleWithIndexForListElement.NullOrEmpty())
								{
									string[] array3 = suggestedPath.Split(new char[]
									{
										'.'
									});
									array3[num] = bestHandleWithIndexForListElement;
									suggestedPath = string.Join(".", array3);
								}
							}
						}
						else
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Can't enter node ",
								text2,
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
						FieldInfo fieldNamed = this.GetFieldNamed(obj.GetType(), text2);
						if (fieldNamed == null)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Field ",
								text2,
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
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj3, text2, num2);
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
							text2,
							" at path ",
							path,
							" but it's not a field, it's ",
							defInjectionPathPartKind,
							". (",
							fileSource,
							")"
						}));
					}
					if (path != suggestedPath)
					{
						IList<string> list2 = value as IList<string>;
						string text3;
						if (list2 != null)
						{
							text3 = list2.ToStringSafeEnumerable();
						}
						else
						{
							text3 = value.ToString();
						}
						this.loadSyntaxSuggestions.Add(string.Concat(new string[]
						{
							"Consider using ",
							suggestedPath,
							" instead of ",
							path,
							" for translation '",
							text3,
							"' (",
							fileSource,
							")"
						}));
					}
					return;
					Block_32:
					throw new InvalidOperationException("Field " + text2 + " does not exist.");
					Block_33:
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
					Block_34:
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
					Block_37:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_8F3:
					throw new InvalidOperationException("Index out of bounds (max index is " + (num3 - 1) + ")");
					Block_41:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_9D3:
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
			Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				if (!injectionsByNormalizedPath.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					injectionsByNormalizedPath.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			IEnumerator enumerator2 = allDefsEnum.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj = enumerator2.Current;
					Def def = (Def)obj;
					foreach (string mi in this.MissingInjectionsFromDef(def, injectionsByNormalizedPath, outUnnecessaryDefInjections))
					{
						yield return mi;
					}
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
			yield break;
		}

		private IEnumerable<string> MissingInjectionsFromDef(Def def, Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath, List<string> outUnnecessaryDefInjections)
		{
			HashSet<object> visited = new HashSet<object>();
			foreach (string missing in this.MissingInjectionsFromDefRecursive(def, def.defName, def.defName, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, true, def.generated))
			{
				yield return missing;
			}
			yield break;
		}

		private IEnumerable<string> MissingInjectionsFromDefRecursive(object obj, string curNormalizedPath, string curSuggestedPath, HashSet<object> visited, Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath, List<string> outUnnecessaryDefInjections, bool translationAllowed, bool defGenerated)
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
						string normalizedPath = curNormalizedPath + "." + fi.Name;
						string suggestedPath = curSuggestedPath + "." + fi.Name;
						DefInjectionPackage.DefInjection existingNonListInjection;
						if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out existingNonListInjection) && !existingNonListInjection.IsFullListInjection)
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(existingNonListInjection.path + " '" + existingNonListInjection.injection.Replace("\n", "\\n") + "'");
							}
						}
						else if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(str, fi))
						{
							yield return suggestedPath + " '" + str.Replace("\n", "\\n") + "'";
						}
					}
					else if (val is IEnumerable<string>)
					{
						IEnumerable<string> collection = (IEnumerable<string>)val;
						bool allowFullListTranslation = fi.HasAttribute<TranslationCanChangeCountAttribute>();
						string fullListNormalizedPath = curNormalizedPath + "." + fi.Name;
						DefInjectionPackage.DefInjection existingListInjection;
						if (injectionsByNormalizedPath.TryGetValue(fullListNormalizedPath, out existingListInjection) && existingListInjection.IsFullListInjection)
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(existingListInjection.path + " '" + existingListInjection.fullListInjection.ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
							}
						}
						else
						{
							int i = 0;
							foreach (string elem in collection)
							{
								string normalizedPath2 = string.Concat(new object[]
								{
									curNormalizedPath,
									".",
									fi.Name,
									".",
									i
								});
								string suggestedPath2 = string.Concat(new object[]
								{
									curSuggestedPath,
									".",
									fi.Name,
									".",
									i
								});
								DefInjectionPackage.DefInjection existingNonListInjection2;
								if (injectionsByNormalizedPath.TryGetValue(normalizedPath2, out existingNonListInjection2) && !existingNonListInjection2.IsFullListInjection)
								{
									if (!thisFieldTranslationAllowed)
									{
										outUnnecessaryDefInjections.Add(existingNonListInjection2.path + " '" + existingNonListInjection2.injection.Replace("\n", "\\n") + "'");
									}
								}
								else if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(elem, fi))
								{
									yield return string.Concat(new string[]
									{
										suggestedPath2,
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
									string nextNormalizedPath = string.Concat(new object[]
									{
										curNormalizedPath,
										".",
										fi.Name,
										".",
										j
									});
									string nextSuggestedPath = string.Concat(new string[]
									{
										curSuggestedPath,
										".",
										fi.Name,
										".",
										handleOrIndex
									});
									foreach (string missing in this.MissingInjectionsFromDefRecursive(elem2, nextNormalizedPath, nextSuggestedPath, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated))
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
						string nextNormalizedPath2 = curNormalizedPath + "." + fi.Name;
						string nextSuggestedPath2 = curSuggestedPath + "." + fi.Name;
						foreach (string missing2 in this.MissingInjectionsFromDefRecursive(val, nextNormalizedPath2, nextSuggestedPath2, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated))
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

		public class DefInjection
		{
			public string path;

			public string normalizedPath;

			public string nonBackCompatiblePath;

			public string suggestedPath;

			public string injection;

			public List<string> fullListInjection;

			public string fileSource;

			public DefInjection()
			{
			}

			public bool IsFullListInjection
			{
				get
				{
					return this.fullListInjection != null;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <CheckErrors>c__AnonStorey3
		{
			internal string key;

			public <CheckErrors>c__AnonStorey3()
			{
			}

			internal bool <>m__0(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
			{
				return !x.Value.IsFullListInjection && x.Key.StartsWith(this.key + ".");
			}
		}

		[CompilerGenerated]
		private sealed class <MissingInjections>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal Type <databaseType>__0;

			internal PropertyInfo <allDefsProperty>__0;

			internal MethodInfo <allDefsMethod>__0;

			internal IEnumerable <allDefsEnum>__0;

			internal Dictionary<string, DefInjectionPackage.DefInjection> <injectionsByNormalizedPath>__0;

			internal Dictionary<string, DefInjectionPackage.DefInjection>.Enumerator $locvar0;

			internal IEnumerator $locvar1;

			internal Def <def>__1;

			internal IDisposable $locvar2;

			internal List<string> outUnnecessaryDefInjections;

			internal IEnumerator<string> $locvar3;

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
					injectionsByNormalizedPath = new Dictionary<string, DefInjectionPackage.DefInjection>();
					enumerator = this.injections.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair = enumerator.Current;
							if (!injectionsByNormalizedPath.ContainsKey(keyValuePair.Value.normalizedPath))
							{
								injectionsByNormalizedPath.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					enumerator2 = allDefsEnum.GetEnumerator();
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
						Block_9:
						try
						{
							switch (num)
							{
							}
							if (enumerator3.MoveNext())
							{
								mi = enumerator3.Current;
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
								if (enumerator3 != null)
								{
									enumerator3.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator2.MoveNext())
					{
						def = (Def)enumerator2.Current;
						enumerator3 = base.MissingInjectionsFromDef(def, injectionsByNormalizedPath, outUnnecessaryDefInjections).GetEnumerator();
						num = 4294967293u;
						goto Block_9;
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

			internal Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath;

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
					enumerator = base.MissingInjectionsFromDefRecursive(def, def.defName, def.defName, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, true, def.generated).GetEnumerator();
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
				<MissingInjectionsFromDef>c__Iterator.injectionsByNormalizedPath = injectionsByNormalizedPath;
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

			internal string curNormalizedPath;

			internal string <normalizedPath>__3;

			internal string curSuggestedPath;

			internal string <suggestedPath>__3;

			internal Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath;

			internal DefInjectionPackage.DefInjection <existingNonListInjection>__3;

			internal List<string> outUnnecessaryDefInjections;

			internal bool defGenerated;

			internal IEnumerable<string> <collection>__4;

			internal bool <allowFullListTranslation>__4;

			internal string <fullListNormalizedPath>__4;

			internal DefInjectionPackage.DefInjection <existingListInjection>__4;

			internal int <i>__5;

			internal IEnumerator<string> $locvar2;

			internal string <elem>__6;

			internal string <normalizedPath>__7;

			internal string <suggestedPath>__7;

			internal DefInjectionPackage.DefInjection <existingNonListInjection>__7;

			internal IEnumerable <collection>__8;

			internal int <i>__8;

			internal IEnumerator $locvar3;

			internal object <elem>__9;

			internal IDisposable $locvar4;

			internal string <handleOrIndex>__10;

			internal string <nextNormalizedPath>__10;

			internal string <nextSuggestedPath>__10;

			internal IEnumerator<string> $locvar5;

			internal string <missing>__11;

			internal string <nextNormalizedPath>__12;

			internal string <nextSuggestedPath>__12;

			internal IEnumerator<string> $locvar6;

			internal string <missing>__13;

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
					goto IL_90E;
				case 1u:
					IL_264:
					break;
				case 2u:
					Block_19:
					try
					{
						switch (num)
						{
						case 2u:
							IL_52A:
							i++;
							break;
						}
						if (enumerator.MoveNext())
						{
							elem = enumerator.Current;
							normalizedPath2 = string.Concat(new object[]
							{
								curNormalizedPath,
								".",
								fi.Name,
								".",
								i
							});
							suggestedPath2 = string.Concat(new object[]
							{
								curSuggestedPath,
								".",
								fi.Name,
								".",
								i
							});
							if (injectionsByNormalizedPath.TryGetValue(normalizedPath2, out existingNonListInjection2) && !existingNonListInjection2.IsFullListInjection)
							{
								if (!thisFieldTranslationAllowed)
								{
									outUnnecessaryDefInjections.Add(existingNonListInjection2.path + " '" + existingNonListInjection2.injection.Replace("\n", "\\n") + "'");
								}
								goto IL_52A;
							}
							if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(elem, fi))
							{
								this.$current = string.Concat(new string[]
								{
									suggestedPath2,
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
							goto IL_52A;
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
					Block_21:
					try
					{
						switch (num)
						{
						case 3u:
							Block_44:
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
							goto IL_797;
						}
						IL_788:
						j++;
						IL_797:
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
								nextNormalizedPath = string.Concat(new object[]
								{
									curNormalizedPath,
									".",
									fi.Name,
									".",
									j
								});
								nextSuggestedPath = string.Concat(new string[]
								{
									curSuggestedPath,
									".",
									fi.Name,
									".",
									handleOrIndex
								});
								enumerator3 = base.MissingInjectionsFromDefRecursive(elem2, nextNormalizedPath, nextSuggestedPath, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated).GetEnumerator();
								num = 4294967293u;
								goto Block_44;
							}
							goto IL_788;
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
					Block_24:
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
				IL_56A:
				IL_8FF:
				IL_900:
				k++;
				IL_90E:
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
						goto IL_900;
					}
					if (typeof(string).IsAssignableFrom(fi.FieldType))
					{
						str = (string)val;
						normalizedPath = curNormalizedPath + "." + fi.Name;
						suggestedPath = curSuggestedPath + "." + fi.Name;
						if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out existingNonListInjection) && !existingNonListInjection.IsFullListInjection)
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(existingNonListInjection.path + " '" + existingNonListInjection.injection.Replace("\n", "\\n") + "'");
							}
							goto IL_264;
						}
						if (thisFieldTranslationAllowed && !defGenerated && DefInjectionPackage.ShouldCheckMissingInjection(str, fi))
						{
							this.$current = suggestedPath + " '" + str.Replace("\n", "\\n") + "'";
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
						goto IL_264;
					}
					else if (val is IEnumerable<string>)
					{
						collection = (IEnumerable<string>)val;
						allowFullListTranslation = fi.HasAttribute<TranslationCanChangeCountAttribute>();
						fullListNormalizedPath = curNormalizedPath + "." + fi.Name;
						if (injectionsByNormalizedPath.TryGetValue(fullListNormalizedPath, out existingListInjection) && existingListInjection.IsFullListInjection)
						{
							if (!thisFieldTranslationAllowed)
							{
								outUnnecessaryDefInjections.Add(existingListInjection.path + " '" + existingListInjection.fullListInjection.ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
							}
							goto IL_56A;
						}
						i = 0;
						enumerator = collection.GetEnumerator();
						num = 4294967293u;
						goto Block_19;
					}
					else
					{
						if (val is IEnumerable)
						{
							collection2 = (IEnumerable)val;
							j = 0;
							enumerator2 = collection2.GetEnumerator();
							num = 4294967293u;
							goto Block_21;
						}
						if (val != null && GenTypes.IsCustomType(val.GetType()))
						{
							nextNormalizedPath2 = curNormalizedPath + "." + fi.Name;
							nextSuggestedPath2 = curSuggestedPath + "." + fi.Name;
							enumerator4 = base.MissingInjectionsFromDefRecursive(val, nextNormalizedPath2, nextSuggestedPath2, visited, injectionsByNormalizedPath, outUnnecessaryDefInjections, thisFieldTranslationAllowed, defGenerated).GetEnumerator();
							num = 4294967293u;
							goto Block_24;
						}
						goto IL_8FF;
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
				<MissingInjectionsFromDefRecursive>c__Iterator.curNormalizedPath = curNormalizedPath;
				<MissingInjectionsFromDefRecursive>c__Iterator.curSuggestedPath = curSuggestedPath;
				<MissingInjectionsFromDefRecursive>c__Iterator.injectionsByNormalizedPath = injectionsByNormalizedPath;
				<MissingInjectionsFromDefRecursive>c__Iterator.outUnnecessaryDefInjections = outUnnecessaryDefInjections;
				<MissingInjectionsFromDefRecursive>c__Iterator.defGenerated = defGenerated;
				return <MissingInjectionsFromDefRecursive>c__Iterator;
			}
		}
	}
}
