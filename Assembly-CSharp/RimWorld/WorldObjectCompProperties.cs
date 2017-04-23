using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class WorldObjectCompProperties
	{
		public Type compClass = typeof(WorldObjectComp);

		[DebuggerHidden]
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties.<ConfigErrors>c__Iterator86 <ConfigErrors>c__Iterator = new WorldObjectCompProperties.<ConfigErrors>c__Iterator86();
			<ConfigErrors>c__Iterator.parentDef = parentDef;
			<ConfigErrors>c__Iterator.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator.<>f__this = this;
			WorldObjectCompProperties.<ConfigErrors>c__Iterator86 expr_1C = <ConfigErrors>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}
	}
}
