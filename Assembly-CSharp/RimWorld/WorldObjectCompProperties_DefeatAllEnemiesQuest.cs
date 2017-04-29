using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class WorldObjectCompProperties_DefeatAllEnemiesQuest : WorldObjectCompProperties
	{
		public WorldObjectCompProperties_DefeatAllEnemiesQuest()
		{
			this.compClass = typeof(DefeatAllEnemiesQuestComp);
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_DefeatAllEnemiesQuest.<ConfigErrors>c__Iterator88 <ConfigErrors>c__Iterator = new WorldObjectCompProperties_DefeatAllEnemiesQuest.<ConfigErrors>c__Iterator88();
			<ConfigErrors>c__Iterator.parentDef = parentDef;
			<ConfigErrors>c__Iterator.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator.<>f__this = this;
			WorldObjectCompProperties_DefeatAllEnemiesQuest.<ConfigErrors>c__Iterator88 expr_1C = <ConfigErrors>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
