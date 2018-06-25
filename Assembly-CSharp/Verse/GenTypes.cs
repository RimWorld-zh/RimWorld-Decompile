using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F51 RID: 3921
	public static class GenTypes
	{
		// Token: 0x04003E41 RID: 15937
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

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005EC2 RID: 24258 RVA: 0x00304074 File Offset: 0x00302474
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

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x00304098 File Offset: 0x00302498
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

		// Token: 0x06005EC4 RID: 24260 RVA: 0x003040BC File Offset: 0x003024BC
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x003040E8 File Offset: 0x003024E8
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x00304120 File Offset: 0x00302520
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x00304158 File Offset: 0x00302558
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x00304198 File Offset: 0x00302598
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

		// Token: 0x06005EC9 RID: 24265 RVA: 0x003041C4 File Offset: 0x003025C4
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

		// Token: 0x06005ECA RID: 24266 RVA: 0x00304238 File Offset: 0x00302638
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

		// Token: 0x06005ECB RID: 24267 RVA: 0x003042AC File Offset: 0x003026AC
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

		// Token: 0x06005ECC RID: 24268 RVA: 0x00304324 File Offset: 0x00302724
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}
	}
}
