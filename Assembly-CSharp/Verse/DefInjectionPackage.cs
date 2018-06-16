using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000BF2 RID: 3058
	public class DefInjectionPackage
	{
		// Token: 0x0600429D RID: 17053 RVA: 0x00231788 File Offset: 0x0022FB88
		public DefInjectionPackage(Type defType)
		{
			this.defType = defType;
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x002317E8 File Offset: 0x0022FBE8
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

		// Token: 0x0600429F RID: 17055 RVA: 0x00231838 File Offset: 0x0022FC38
		private string ProcessedTranslation(string rawTranslation)
		{
			return rawTranslation.Replace("\\n", "\n");
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00231860 File Offset: 0x0022FC60
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

		// Token: 0x060042A1 RID: 17057 RVA: 0x00231ABC File Offset: 0x0022FEBC
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

		// Token: 0x060042A2 RID: 17058 RVA: 0x00231B28 File Offset: 0x0022FF28
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

		// Token: 0x060042A3 RID: 17059 RVA: 0x00231BA0 File Offset: 0x0022FFA0
		private string BackCompatibleKey(string key)
		{
			string[] array = key.Split(new char[]
			{
				'.'
			});
			array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0], true);
			return string.Join(".", array);
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x00231BE4 File Offset: 0x0022FFE4
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

		// Token: 0x060042A5 RID: 17061 RVA: 0x00231ECC File Offset: 0x002302CC
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

		// Token: 0x060042A6 RID: 17062 RVA: 0x00231FF8 File Offset: 0x002303F8
		private void SetDefFieldAtPath(Type defType, string path, object value, Type ensureFieldType, bool errorOnDefNotFound, string fileSource)
		{
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
				path = BackCompatibility.BackCompatibleModifiedTranslationPath(defType, path);
				bool flag = false;
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
					DefInjectionPathPartKind defInjectionPathPartKind;
					string text2;
					int num;
					int num2;
					FieldInfo field;
					int num3;
					for (;;)
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.Field;
						text2 = list[0];
						num = -1;
						if (text2.Contains('['))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.FieldWithListIndex;
							string[] array = text2.Split(new char[]
							{
								'['
							});
							string text3 = array[1];
							text3 = text3.Substring(0, text3.Length - 1);
							num = (int)ParseHelper.FromString(text3, typeof(int));
							text2 = array[0];
						}
						else if (int.TryParse(text2, out num))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
						}
						if (list.Count == 1)
						{
							break;
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex)
						{
							PropertyInfo property = obj.GetType().GetProperty("Item");
							if (property == null)
							{
								goto Block_28;
							}
							num2 = (int)obj.GetType().GetProperty("Count").GetValue(obj, null);
							if (num < 0 || num >= num2)
							{
								goto IL_6EE;
							}
							obj = property.GetValue(obj, new object[]
							{
								num
							});
						}
						else
						{
							field = obj.GetType().GetField(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (field == null)
							{
								goto Block_30;
							}
							if (field.HasAttribute<NoTranslateAttribute>())
							{
								goto Block_31;
							}
							if (field.HasAttribute<UnsavedAttribute>())
							{
								goto Block_32;
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
								PropertyInfo property2 = value2.GetType().GetProperty("Item");
								if (property2 == null)
								{
									goto Block_35;
								}
								num3 = (int)value2.GetType().GetProperty("Count").GetValue(value2, null);
								if (num < 0 || num >= num3)
								{
									goto IL_891;
								}
								obj = property2.GetValue(value2, new object[]
								{
									num
								});
							}
						}
						list.RemoveAt(0);
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
					else
					{
						object obj2;
						if (defInjectionPathPartKind == DefInjectionPathPartKind.FieldWithListIndex)
						{
							FieldInfo field2 = obj.GetType().GetField(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (field2 == null)
							{
								throw new InvalidOperationException("Field " + text2 + " does not exist.");
							}
							obj2 = field2.GetValue(obj);
							if (field2.HasAttribute<TranslationCanChangeCountAttribute>())
							{
								flag = true;
							}
						}
						else
						{
							obj2 = obj;
						}
						if (obj2 == null)
						{
							throw new InvalidOperationException("Tried to use index on null list at " + path);
						}
						Type type = obj2.GetType();
						PropertyInfo property3 = type.GetProperty("Count");
						if (property3 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Count' property).");
						}
						int num4 = (int)property3.GetValue(obj2, null);
						if (num >= num4)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Trying to translate ",
								defType,
								".",
								path,
								" at index ",
								num,
								" but the list only has ",
								num4,
								" entries (so max index is ",
								(num4 - 1).ToString(),
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
						else if (num < 0 || num >= (int)type.GetProperty("Count").GetValue(obj2, null))
						{
							this.loadErrors.Add("Index out of bounds (max index is " + ((int)type.GetProperty("Count").GetValue(obj2, null) - 1) + ")");
						}
						else
						{
							property4.SetValue(obj2, value, new object[]
							{
								num
							});
						}
					}
					return;
					Block_28:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_6EE:
					throw new InvalidOperationException("Index out of bounds (max index is " + (num2 - 1) + ")");
					Block_30:
					throw new InvalidOperationException("Field " + text2 + " does not exist.");
					Block_31:
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
					Block_32:
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
					Block_35:
					throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
					IL_891:
					throw new InvalidOperationException("Index out of bounds (max index is " + (num3 - 1) + ")");
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

		// Token: 0x060042A7 RID: 17063 RVA: 0x0023297C File Offset: 0x00230D7C
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

		// Token: 0x060042A8 RID: 17064 RVA: 0x00232A2C File Offset: 0x00230E2C
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

		// Token: 0x060042A9 RID: 17065 RVA: 0x00232A60 File Offset: 0x00230E60
		private IEnumerable<string> MissingInjectionsFromDef(Def def, List<string> outUnnecessaryDefInjections)
		{
			HashSet<object> visited = new HashSet<object>();
			foreach (string missing in this.MissingInjectionsFromDefRecursive(def, def.defName, visited, outUnnecessaryDefInjections, true, def.generated))
			{
				yield return missing;
			}
			yield break;
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00232A98 File Offset: 0x00230E98
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
									foreach (string missing in this.MissingInjectionsFromDefRecursive(elem2, string.Concat(new object[]
									{
										curPath,
										".",
										fi.Name,
										".",
										j
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

		// Token: 0x060042AB RID: 17067 RVA: 0x00232AF0 File Offset: 0x00230EF0
		private static bool ShouldCheckMissingInjection(string str, FieldInfo fi)
		{
			return !str.NullOrEmpty() && (fi.HasAttribute<MustTranslateAttribute>() || str.Contains(' '));
		}

		// Token: 0x04002D8B RID: 11659
		public Type defType;

		// Token: 0x04002D8C RID: 11660
		private Dictionary<string, string> injections = new Dictionary<string, string>();

		// Token: 0x04002D8D RID: 11661
		private Dictionary<string, List<string>> fullListInjections = new Dictionary<string, List<string>>();

		// Token: 0x04002D8E RID: 11662
		private Dictionary<string, string> injectionsFileSource = new Dictionary<string, string>();

		// Token: 0x04002D8F RID: 11663
		private Dictionary<string, string> fullListInjectionsFileSource = new Dictionary<string, string>();

		// Token: 0x04002D90 RID: 11664
		public List<Pair<string, string>> autoFixedBackCompatKeys = new List<Pair<string, string>>();

		// Token: 0x04002D91 RID: 11665
		public List<string> loadErrors = new List<string>();

		// Token: 0x04002D92 RID: 11666
		public bool usedOldRepSyntax;

		// Token: 0x04002D93 RID: 11667
		private const BindingFlags FieldBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04002D94 RID: 11668
		public const string RepNodeName = "rep";
	}
}
