using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F4C RID: 3916
	public static class GenTypes
	{
		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005E90 RID: 24208 RVA: 0x00301798 File Offset: 0x002FFB98
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

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005E91 RID: 24209 RVA: 0x003017BC File Offset: 0x002FFBBC
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

		// Token: 0x06005E92 RID: 24210 RVA: 0x003017E0 File Offset: 0x002FFBE0
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x0030180C File Offset: 0x002FFC0C
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00301844 File Offset: 0x002FFC44
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x0030187C File Offset: 0x002FFC7C
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x003018BC File Offset: 0x002FFCBC
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

		// Token: 0x06005E97 RID: 24215 RVA: 0x003018E8 File Offset: 0x002FFCE8
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

		// Token: 0x06005E98 RID: 24216 RVA: 0x0030195C File Offset: 0x002FFD5C
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

		// Token: 0x06005E99 RID: 24217 RVA: 0x003019D0 File Offset: 0x002FFDD0
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

		// Token: 0x06005E9A RID: 24218 RVA: 0x00301A48 File Offset: 0x002FFE48
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}

		// Token: 0x04003E24 RID: 15908
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
