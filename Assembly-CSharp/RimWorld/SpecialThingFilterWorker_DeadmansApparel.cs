﻿using System;
using Verse;

namespace RimWorld
{
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		public SpecialThingFilterWorker_DeadmansApparel()
		{
		}

		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
