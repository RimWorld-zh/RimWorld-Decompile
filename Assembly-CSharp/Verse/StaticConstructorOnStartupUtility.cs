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
		// Token: 0x0600615F RID: 24927 RVA: 0x00312D78 File Offset: 0x00311178
		public static void CallAll()
		{
			IEnumerable<Type> enumerable = GenTypes.AllTypesWithAttribute<StaticConstructorOnStartup>();
			foreach (Type type in enumerable)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x00312DE0 File Offset: 0x003111E0
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

		// Token: 0x04003FB4 RID: 16308
		public static bool coreStaticAssetsLoaded;
	}
}
