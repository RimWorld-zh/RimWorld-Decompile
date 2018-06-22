using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AF2 RID: 2802
	public static class GenDefDatabase
	{
		// Token: 0x06003E1B RID: 15899 RVA: 0x0020BEA0 File Offset: 0x0020A2A0
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x0020BEE4 File Offset: 0x0020A2E4
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

		// Token: 0x06003E1D RID: 15901 RVA: 0x0020BF40 File Offset: 0x0020A340
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

		// Token: 0x06003E1E RID: 15902 RVA: 0x0020BF64 File Offset: 0x0020A364
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}
