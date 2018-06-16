using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FBB RID: 4027
	public static class StaticConstructorOnStartupUtility
	{
		// Token: 0x06006138 RID: 24888 RVA: 0x00310BC8 File Offset: 0x0030EFC8
		public static void CallAll()
		{
			IEnumerable<Type> enumerable = GenTypes.AllTypesWithAttribute<StaticConstructorOnStartup>();
			foreach (Type type in enumerable)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x00310C30 File Offset: 0x0030F030
		public static void ReportProbablyMissingAttributes()
		{
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (Type type in GenTypes.AllTypes)
			{
				if (!type.HasAttribute<StaticConstructorOnStartup>())
				{
					FieldInfo fieldInfo = type.GetFields(bindingAttr).FirstOrDefault(delegate(FieldInfo x)
					{
						Type type2 = x.FieldType;
						if (type2.IsArray)
						{
							type2 = type2.GetElementType();
						}
						return typeof(Texture).IsAssignableFrom(type2) || typeof(Material).IsAssignableFrom(type2) || typeof(Shader).IsAssignableFrom(type2) || typeof(Graphic).IsAssignableFrom(type2) || typeof(GameObject).IsAssignableFrom(type2) || typeof(MaterialPropertyBlock).IsAssignableFrom(type2);
					});
					if (fieldInfo != null)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Type ",
							type.Name,
							" probably needs a StaticConstructorOnStartup attribute, because it has a field ",
							fieldInfo.Name,
							" of type ",
							fieldInfo.FieldType.Name,
							". All assets must be loaded in the main thread."
						}), false);
					}
				}
			}
		}

		// Token: 0x04003F98 RID: 16280
		public static bool coreStaticAssetsLoaded;
	}
}
