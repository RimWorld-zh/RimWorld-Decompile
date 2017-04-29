using System;
using System.Collections;
using System.Diagnostics;

namespace RimWorld.Planet
{
	public class WorldLayer_Glow : WorldLayer
	{
		private const int SubdivisionsCount = 4;

		public const float GlowRadius = 8f;

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Glow.<Regenerate>c__IteratorF0 <Regenerate>c__IteratorF = new WorldLayer_Glow.<Regenerate>c__IteratorF0();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Glow.<Regenerate>c__IteratorF0 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
