using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AF6 RID: 2806
	public static class GenDefDatabase
	{
		// Token: 0x06003E20 RID: 15904 RVA: 0x0020BB7C File Offset: 0x00209F7C
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x0020BBC0 File Offset: 0x00209FC0
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			Def result;
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				result = SoundDef.Named(targetDefName);
			}
			else
			{
				result = (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", new object[]
				{
					targetDefName
				});
			}
			return result;
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x0020BC1C File Offset: 0x0020A01C
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type defType in typeof(Def).AllSubclasses())
			{
				if (!defType.IsAbstract)
				{
					if (defType != typeof(Def))
					{
						bool foundNonAbstractAncestor = false;
						Type parent = defType.BaseType;
						while (parent != null && parent != typeof(Def))
						{
							if (!parent.IsAbstract)
							{
								foundNonAbstractAncestor = true;
								break;
							}
							parent = parent.BaseType;
						}
						if (!foundNonAbstractAncestor)
						{
							yield return defType;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x0020BC40 File Offset: 0x0020A040
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}
