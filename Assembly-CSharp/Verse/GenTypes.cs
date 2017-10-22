using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	public static class GenTypes
	{
		public static readonly List<string> IgnoredNamespaceNames = new List<string>
		{
			"RimWorld",
			"Verse",
			"Verse.AI",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen"
		};

		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static IEnumerable<Type> AllTypes
		{
			get
			{
				using (IEnumerator<Assembly> enumerator = GenTypes.AllActiveAssemblies.GetEnumerator())
				{
					Type[] array;
					int num;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							Assembly assembly = enumerator.Current;
							Type[] assemblyTypes = null;
							try
							{
								assemblyTypes = assembly.GetTypes();
							}
							catch (ReflectionTypeLoadException)
							{
								Log.Error("Exception getting types in assembly " + assembly.ToString());
							}
							if (assemblyTypes != null)
							{
								array = assemblyTypes;
								num = 0;
								if (num < array.Length)
									break;
							}
							continue;
						}
						yield break;
					}
					Type type = array[num];
					yield return type;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_0154:
				/*Error near IL_0155: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where ((MemberInfo)x).HasAttribute<TAttr>()
			select x;
		}

		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any()
			select type;
		}

		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			using (IEnumerator<Type> enumerator = baseType.AllSubclasses().GetEnumerator())
			{
				Type descendant;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						descendant = enumerator.Current;
						if (!descendant.IsAbstract)
							break;
						continue;
					}
					yield break;
				}
				yield return descendant;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_0101:
			/*Error near IL_0102: Unexpected return in MoveNext()*/;
		}

		public static Type GetTypeInAnyAssembly(string typeName)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			Type result;
			if (typeInAnyAssemblyRaw != null)
			{
				result = typeInAnyAssemblyRaw;
			}
			else
			{
				for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
				{
					string typeName2 = GenTypes.IgnoredNamespaceNames[i] + "." + typeName;
					typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName2);
					if (typeInAnyAssemblyRaw != null)
						goto IL_0041;
				}
				result = null;
			}
			goto IL_0064;
			IL_0041:
			result = typeInAnyAssemblyRaw;
			goto IL_0064;
			IL_0064:
			return result;
		}

		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			foreach (Assembly allActiveAssembly in GenTypes.AllActiveAssemblies)
			{
				Type type = allActiveAssembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			string result;
			if (type.IsGenericType)
			{
				result = type.ToString();
			}
			else
			{
				for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
				{
					if (type.Namespace == GenTypes.IgnoredNamespaceNames[i])
						goto IL_003b;
				}
				result = type.FullName;
			}
			goto IL_0068;
			IL_003b:
			result = type.Name;
			goto IL_0068;
			IL_0068:
			return result;
		}
	}
}
