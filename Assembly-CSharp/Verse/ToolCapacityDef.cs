using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class ToolCapacityDef : Def
	{
		[CompilerGenerated]
		private static Func<ManeuverDef, VerbProperties> <>f__am$cache0;

		public ToolCapacityDef()
		{
		}

		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		[CompilerGenerated]
		private bool <get_Maneuvers>m__0(ManeuverDef x)
		{
			return x.requiredCapacity == this;
		}

		[CompilerGenerated]
		private static VerbProperties <get_VerbsProperties>m__1(ManeuverDef x)
		{
			return x.verb;
		}
	}
}
