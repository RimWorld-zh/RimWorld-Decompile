using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class GenDefDatabase
	{
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", defName, errorOnFail);
		}

		public static Def GetDefSilentFail(Type type, string targetDefName)
		{
			return (type != typeof(SoundDef)) ? ((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", targetDefName)) : SoundDef.Named(targetDefName);
		}

		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			using (IEnumerator<Type> enumerator = typeof(Def).AllSubclasses().GetEnumerator())
			{
				Type defType;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						defType = enumerator.Current;
						if (!defType.IsAbstract && defType != typeof(Def))
						{
							bool foundNonAbstractAncestor = false;
							Type parent = defType.BaseType;
							while (parent != null && parent != typeof(Def))
							{
								if (parent.IsAbstract)
								{
									parent = parent.BaseType;
									continue;
								}
								foundNonAbstractAncestor = true;
								break;
							}
							if (!foundNonAbstractAncestor)
								break;
						}
						continue;
					}
					yield break;
				}
				yield return defType;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_016d:
			/*Error near IL_016e: Unexpected return in MoveNext()*/;
		}

		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return ((IEnumerable)mod.AllDefs).OfType<T>();
		}
	}
}
