using System;
using System.Collections;
using System.Diagnostics;

namespace RimWorld.Planet
{
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		private const int SubdivisionsCount = 4;

		private const float ViewAngleOffset = 10f;

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_UngeneratedPlanetParts.<Regenerate>c__IteratorF8 <Regenerate>c__IteratorF = new WorldLayer_UngeneratedPlanetParts.<Regenerate>c__IteratorF8();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_UngeneratedPlanetParts.<Regenerate>c__IteratorF8 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
