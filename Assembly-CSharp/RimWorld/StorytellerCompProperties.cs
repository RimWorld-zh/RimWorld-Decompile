using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerCompProperties
	{
		public Type compClass;

		public float minDaysPassed;

		public IncidentTargetType allowedTargetTypes = IncidentTargetType.All;

		public StorytellerCompProperties()
		{
		}

		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		[DebuggerHidden]
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			StorytellerCompProperties.<ConfigErrors>c__Iterator98 <ConfigErrors>c__Iterator = new StorytellerCompProperties.<ConfigErrors>c__Iterator98();
			<ConfigErrors>c__Iterator.parentDef = parentDef;
			<ConfigErrors>c__Iterator.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator.<>f__this = this;
			StorytellerCompProperties.<ConfigErrors>c__Iterator98 expr_1C = <ConfigErrors>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}
	}
}
