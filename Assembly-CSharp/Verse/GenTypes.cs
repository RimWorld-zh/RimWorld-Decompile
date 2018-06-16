using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F4D RID: 3917
	public static class GenTypes
	{
		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005E92 RID: 24210 RVA: 0x003016BC File Offset: 0x002FFABC
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i++)
					{
						yield return mod.assemblies.loadedAssemblies[i];
					}
				}
				yield break;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005E93 RID: 24211 RVA: 0x003016E0 File Offset: 0x002FFAE0
		public static IEnumerable<Type> AllTypes
		{
			get
			{
				foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
				{
					Type[] assemblyTypes = null;
					try
					{
						assemblyTypes = assembly.GetTypes();
					}
					catch (ReflectionTypeLoadException)
					{
						Log.Error("Exception getting types in assembly " + assembly.ToString(), false);
					}
					if (assemblyTypes != null)
					{
						foreach (Type type in assemblyTypes)
						{
							yield return type;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00301704 File Offset: 0x002FFB04
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00301730 File Offset: 0x002FFB30
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x00301768 File Offset: 0x002FFB68
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x003017A0 File Offset: 0x002FFBA0
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x003017E0 File Offset: 0x002FFBE0
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type descendant in baseType.AllSubclasses())
			{
				if (!descendant.IsAbstract)
				{
					yield return descendant;
				}
			}
			yield break;
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x0030180C File Offset: 0x002FFC0C
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
					{
						return typeInAnyAssemblyRaw;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x00301880 File Offset: 0x002FFC80
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type = assembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x003018F4 File Offset: 0x002FFCF4
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
					{
						return type.Name;
					}
				}
				result = type.FullName;
			}
			return result;
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x0030196C File Offset: 0x002FFD6C
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}

		// Token: 0x04003E25 RID: 15909
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
	}
}
