using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F50 RID: 3920
	public static class GenTypes
	{
		// Token: 0x04003E39 RID: 15929
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
		// (get) Token: 0x06005EC2 RID: 24258 RVA: 0x00303E54 File Offset: 0x00302254
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
		// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x00303E78 File Offset: 0x00302278
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

		// Token: 0x06005EC4 RID: 24260 RVA: 0x00303E9C File Offset: 0x0030229C
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x00303EC8 File Offset: 0x003022C8
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x00303F00 File Offset: 0x00302300
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x00303F38 File Offset: 0x00302338
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x00303F78 File Offset: 0x00302378
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

		// Token: 0x06005EC9 RID: 24265 RVA: 0x00303FA4 File Offset: 0x003023A4
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

		// Token: 0x06005ECA RID: 24266 RVA: 0x00304018 File Offset: 0x00302418
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

		// Token: 0x06005ECB RID: 24267 RVA: 0x0030408C File Offset: 0x0030248C
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

		// Token: 0x06005ECC RID: 24268 RVA: 0x00304104 File Offset: 0x00302504
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}
	}
}
