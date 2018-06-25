using System;
using RimWorld;

namespace Verse
{
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		public ThoughtDef thought;

		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}
	}
}
