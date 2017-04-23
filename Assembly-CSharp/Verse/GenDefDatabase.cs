using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Verse
{
	public static class GenDefDatabase
	{
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		public static Def GetDefSilentFail(Type type, string targetDefName)
		{
			if (type == typeof(SoundDef))
			{
				return SoundDef.Named(targetDefName);
			}
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", new object[]
			{
				targetDefName
			});
		}

		[DebuggerHidden]
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			GenDefDatabase.<AllDefTypesWithDatabases>c__Iterator1B9 <AllDefTypesWithDatabases>c__Iterator1B = new GenDefDatabase.<AllDefTypesWithDatabases>c__Iterator1B9();
			GenDefDatabase.<AllDefTypesWithDatabases>c__Iterator1B9 expr_07 = <AllDefTypesWithDatabases>c__Iterator1B;
			expr_07.$PC = -2;
			return expr_07;
		}

		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}
