using System;
using Verse;

namespace RimWorld
{
	public class CompProperties_Mannable : CompProperties
	{
		public WorkTags manWorkType = WorkTags.None;

		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}
	}
}
