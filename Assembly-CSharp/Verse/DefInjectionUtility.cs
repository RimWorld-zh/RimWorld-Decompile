using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class DefInjectionUtility
	{
		[CompilerGenerated]
		private static Func<FieldInfo, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<FieldInfo, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<FieldInfo, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<FieldInfo, string> <>f__am$cache3;

		public static void ForEachPossibleDefInjection(Type defType, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			IEnumerable<Def> allDefsInDatabaseForDef = GenDefDatabase.GetAllDefsInDatabaseForDef(defType);
			foreach (Def def in allDefsInDatabaseForDef)
			{
				DefInjectionUtility.ForEachPossibleDefInjectionInDef(def, action);
			}
		}

		private static void ForEachPossibleDefInjectionInDef(Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			HashSet<object> visited = new HashSet<object>();
			DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(def, def.defName, def.defName, visited, true, def, action);
		}

		private static void ForEachPossibleDefInjectionInDefRecursive(object obj, string curNormalizedPath, string curSuggestedPath, HashSet<object> visited, bool translationAllowed, Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			if (obj == null)
			{
				return;
			}
			if (visited.Contains(obj))
			{
				return;
			}
			visited.Add(obj);
			foreach (FieldInfo fieldInfo in DefInjectionUtility.FieldsInDeterministicOrder(obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)))
			{
				object value = fieldInfo.GetValue(obj);
				bool flag = translationAllowed && !fieldInfo.HasAttribute<NoTranslateAttribute>() && !fieldInfo.HasAttribute<UnsavedAttribute>();
				if (!(value is Def))
				{
					if (typeof(string).IsAssignableFrom(fieldInfo.FieldType))
					{
						string currentValue = (string)value;
						string normalizedPath = curNormalizedPath + "." + fieldInfo.Name;
						string suggestedPath = curSuggestedPath + "." + fieldInfo.Name;
						action(suggestedPath, normalizedPath, false, currentValue, null, flag, false, fieldInfo, def);
					}
					else if (value is IEnumerable<string>)
					{
						IEnumerable<string> currentValueCollection = (IEnumerable<string>)value;
						bool flag2 = fieldInfo.HasAttribute<TranslationCanChangeCountAttribute>();
						string normalizedPath2 = curNormalizedPath + "." + fieldInfo.Name;
						string suggestedPath2 = curSuggestedPath + "." + fieldInfo.Name;
						action(suggestedPath2, normalizedPath2, true, null, currentValueCollection, flag, flag && flag2, fieldInfo, def);
					}
					else if (value is IEnumerable)
					{
						IEnumerable enumerable = (IEnumerable)value;
						int num = 0;
						IEnumerator enumerator2 = enumerable.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								if (obj2 != null && !(obj2 is Def) && GenTypes.IsCustomType(obj2.GetType()))
								{
									string text = TranslationHandleUtility.GetBestHandleWithIndexForListElement(enumerable, obj2);
									if (text.NullOrEmpty())
									{
										text = num.ToString();
									}
									string curNormalizedPath2 = string.Concat(new object[]
									{
										curNormalizedPath,
										".",
										fieldInfo.Name,
										".",
										num
									});
									string curSuggestedPath2 = string.Concat(new string[]
									{
										curSuggestedPath,
										".",
										fieldInfo.Name,
										".",
										text
									});
									DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(obj2, curNormalizedPath2, curSuggestedPath2, visited, flag, def, action);
								}
								num++;
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
					else if (value != null && GenTypes.IsCustomType(value.GetType()))
					{
						string curNormalizedPath3 = curNormalizedPath + "." + fieldInfo.Name;
						string curSuggestedPath3 = curSuggestedPath + "." + fieldInfo.Name;
						DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(value, curNormalizedPath3, curSuggestedPath3, visited, flag, def, action);
					}
				}
			}
		}

		public static bool ShouldCheckMissingInjection(string str, FieldInfo fi, Def def)
		{
			return !def.generated && !str.NullOrEmpty() && !fi.HasAttribute<NoTranslateAttribute>() && !fi.HasAttribute<UnsavedAttribute>() && !fi.HasAttribute<MayTranslateAttribute>() && (fi.HasAttribute<MustTranslateAttribute>() || str.Contains(' '));
		}

		private static IEnumerable<FieldInfo> FieldsInDeterministicOrder(IEnumerable<FieldInfo> fields)
		{
			return from x in fields
			orderby x.HasAttribute<UnsavedAttribute>() || x.HasAttribute<NoTranslateAttribute>(), x.Name == "label" descending, x.Name == "description" descending, x.Name
			select x;
		}

		[CompilerGenerated]
		private static bool <FieldsInDeterministicOrder>m__0(FieldInfo x)
		{
			return x.HasAttribute<UnsavedAttribute>() || x.HasAttribute<NoTranslateAttribute>();
		}

		[CompilerGenerated]
		private static bool <FieldsInDeterministicOrder>m__1(FieldInfo x)
		{
			return x.Name == "label";
		}

		[CompilerGenerated]
		private static bool <FieldsInDeterministicOrder>m__2(FieldInfo x)
		{
			return x.Name == "description";
		}

		[CompilerGenerated]
		private static string <FieldsInDeterministicOrder>m__3(FieldInfo x)
		{
			return x.Name;
		}

		public delegate void PossibleDefInjectionTraverser(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def);
	}
}
