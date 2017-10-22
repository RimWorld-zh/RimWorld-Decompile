using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Verse
{
	public class DefInjectionPackage
	{
		public Type defType;

		private Dictionary<string, string> injections = new Dictionary<string, string>();

		private const BindingFlags FieldBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public DefInjectionPackage(Type defType)
		{
			this.defType = defType;
		}

		private string ProcessedPath(string path)
		{
			return (path.Contains('[') || path.Contains(']')) ? path.Replace("]", "").Replace('[', '.') : path;
		}

		private string ProcessedTranslation(string rawTranslation)
		{
			return rawTranslation.Replace("\\n", "\n");
		}

		public void AddDataFromFile(FileInfo file)
		{
			try
			{
				XDocument xDocument = XDocument.Load(file.FullName);
				foreach (XElement item in xDocument.Root.Elements())
				{
					if (item.Name == "rep")
					{
						string key = this.ProcessedPath(item.Elements("path").First().Value);
						string translation = this.ProcessedTranslation(item.Elements("trans").First().Value);
						this.TryAddInjection(file, key, translation);
					}
					else
					{
						string key2 = this.ProcessedPath(item.Name.ToString());
						string translation2 = this.ProcessedTranslation(item.Value);
						this.TryAddInjection(file, key2, translation2);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Exception loading translation data from file " + file + ": " + ex);
			}
		}

		private void TryAddInjection(FileInfo file, string key, string translation)
		{
			string[] array = key.Split('.');
			array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0]);
			key = string.Join(".", array);
			if (!this.HasError(file, key))
			{
				this.injections.Add(key, translation);
			}
		}

		private bool HasError(FileInfo file, string key)
		{
			bool result;
			if (!key.Contains('.'))
			{
				Log.Warning("Error loading DefInjection from file " + file + ": Key lacks a dot: " + key);
				result = true;
			}
			else if (this.injections.ContainsKey(key))
			{
				Log.Warning("Duplicate def-linked translation key: " + key);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void InjectIntoDefs()
		{
			foreach (KeyValuePair<string, string> injection in this.injections)
			{
				string[] array = injection.Key.Split('.');
				string defName = array[0];
				defName = BackCompatibility.BackCompatibleDefName(this.defType, defName);
				object obj = GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "GetNamedSilentFail", defName);
				if (obj == null)
				{
					Log.Warning("Def-linked translation error: Found no " + this.defType + " named " + defName + " to match " + injection.Key);
				}
				else
				{
					this.SetDefFieldAtPath(this.defType, injection.Key, injection.Value);
				}
			}
			GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "ClearCachedData");
		}

		private void SetDefFieldAtPath(Type defType, string path, string value)
		{
			path = BackCompatibility.BackCompatibleModifiedTranslationPath(defType, path);
			try
			{
				List<string> list = path.Split('.').ToList();
				object obj = GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamedSilentFail", list[0]);
				if (obj == null)
				{
					throw new InvalidOperationException("Def named " + list[0] + " not found.");
				}
				list.RemoveAt(0);
				while (true)
				{
					DefInjectionPathPartKind defInjectionPathPartKind = DefInjectionPathPartKind.Field;
					string text = list[0];
					int num = -1;
					if (text.Contains('['))
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.FieldWithListIndex;
						string[] array = text.Split('[');
						string text2 = array[1];
						text2 = text2.Substring(0, text2.Length - 1);
						num = (int)ParseHelper.FromString(text2, typeof(int));
						text = array[0];
					}
					else if (int.TryParse(text, out num))
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
					}
					if (list.Count == 1)
					{
						object obj2;
						switch (defInjectionPathPartKind)
						{
						case DefInjectionPathPartKind.Field:
						{
							FieldInfo fieldNamed = this.GetFieldNamed(obj.GetType(), text);
							if (fieldNamed == null)
							{
								throw new InvalidOperationException("Field " + text + " does not exist in type " + obj.GetType() + ".");
							}
							if (fieldNamed.HasAttribute<NoTranslateAttribute>())
							{
								Log.Error("Translated untranslateable field " + fieldNamed.Name + " of type " + fieldNamed.FieldType + " at path " + path + ". Translating this field will break the game.");
							}
							else if (fieldNamed.FieldType != typeof(string))
							{
								Log.Error("Translated non-string field " + fieldNamed.Name + " of type " + fieldNamed.FieldType + " at path " + path + ". Only string fields should be translated.");
							}
							else
							{
								fieldNamed.SetValue(obj, value);
							}
							return;
						}
						case DefInjectionPathPartKind.FieldWithListIndex:
						{
							FieldInfo field = obj.GetType().GetField(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (field == null)
							{
								throw new InvalidOperationException("Field " + text + " does not exist.");
							}
							obj2 = field.GetValue(obj);
							break;
						}
						default:
						{
							obj2 = obj;
							break;
						}
						}
						Type type = obj2.GetType();
						PropertyInfo property = type.GetProperty("Count");
						if (property == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Count' property).");
						}
						int num2 = (int)property.GetValue(obj2, null);
						if (num >= num2)
						{
							throw new InvalidOperationException("Trying to translate " + defType + "." + path + " at index " + num + " but the original list only has " + num2 + " entries (so the max index is " + (num2 - 1).ToString() + ").");
						}
						PropertyInfo property2 = type.GetProperty("Item");
						if (property2 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
						}
						property2.SetValue(obj2, value, new object[1]
						{
							num
						});
						return;
					}
					if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex)
					{
						PropertyInfo property3 = obj.GetType().GetProperty("Item");
						if (property3 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
						}
						obj = property3.GetValue(obj, new object[1]
						{
							num
						});
						goto IL_043b;
					}
					FieldInfo field2 = obj.GetType().GetField(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (field2 == null)
					{
						throw new InvalidOperationException("Field " + text + " does not exist.");
					}
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						obj = field2.GetValue(obj);
						goto IL_043b;
					}
					object value2 = field2.GetValue(obj);
					PropertyInfo property4 = value2.GetType().GetProperty("Item");
					if (property4 != null)
					{
						obj = property4.GetValue(value2, new object[1]
						{
							num
						});
						goto IL_043b;
					}
					break;
					IL_043b:
					list.RemoveAt(0);
				}
				throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
			}
			catch (Exception ex)
			{
				Log.Warning("Def-linked translation error: Exception getting field at path " + path + " in " + defType + ": " + ex.ToString());
			}
		}

		private FieldInfo GetFieldNamed(Type type, string name)
		{
			FieldInfo field = type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo[] fields;
			int i;
			if (field == null)
			{
				fields = type.GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (i = 0; i < fields.Length; i++)
				{
					object[] customAttributes = fields[i].GetCustomAttributes(typeof(LoadAliasAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							LoadAliasAttribute loadAliasAttribute = (LoadAliasAttribute)customAttributes[j];
							if (loadAliasAttribute.alias == name)
								goto IL_006d;
						}
					}
				}
			}
			FieldInfo result = field;
			goto IL_00a0;
			IL_00a0:
			return result;
			IL_006d:
			result = fields[i];
			goto IL_00a0;
		}

		public IEnumerable<string> MissingInjections()
		{
			Type databaseType = typeof(DefDatabase<>).MakeGenericType(this.defType);
			PropertyInfo allDefsProperty = databaseType.GetProperty("AllDefs");
			MethodInfo allDefsMethod = allDefsProperty.GetGetMethod();
			IEnumerable allDefsEnum = (IEnumerable)allDefsMethod.Invoke(null, null);
			IEnumerator enumerator = allDefsEnum.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Def def = (Def)enumerator.Current;
					using (IEnumerator<string> enumerator2 = this.MissingInjectionsFromDef(def).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							string mi = enumerator2.Current;
							yield return mi;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable2 = disposable = (enumerator as IDisposable);
				if (disposable != null)
				{
					disposable2.Dispose();
				}
			}
			yield break;
			IL_01a7:
			/*Error near IL_01a8: Unexpected return in MoveNext()*/;
		}

		private IEnumerable<string> MissingInjectionsFromDef(Def def)
		{
			if (!def.label.NullOrEmpty())
			{
				string path3 = def.defName + ".label";
				if (!this.injections.ContainsKey(path3))
				{
					yield return path3 + " '" + def.label + "'";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!def.description.NullOrEmpty())
			{
				string path2 = def.defName + ".description";
				if (!this.injections.ContainsKey(path2))
				{
					yield return path2 + " '" + def.description + "'";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			FieldInfo[] fields = def.GetType().GetFields();
			int num = 0;
			string val;
			string path;
			while (true)
			{
				if (num < fields.Length)
				{
					FieldInfo fi = fields[num];
					if (fi.FieldType == typeof(string))
					{
						val = (string)fi.GetValue(def);
						if (!val.NullOrEmpty() && !(fi.Name == "defName") && !(fi.Name == "label") && !(fi.Name == "description") && !fi.HasAttribute<NoTranslateAttribute>() && !fi.HasAttribute<UnsavedAttribute>() && (fi.HasAttribute<MustTranslateAttribute>() || (val != null && val.Contains(' '))))
						{
							path = def.defName + "." + fi.Name;
							if (!this.injections.ContainsKey(path))
								break;
						}
					}
					num++;
					continue;
				}
				yield break;
			}
			yield return path + " '" + val + "'";
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
