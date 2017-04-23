using System;
using System.Collections;
using System.Diagnostics;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Hills : WorldLayer
	{
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Hills.<Regenerate>c__IteratorF0 <Regenerate>c__IteratorF = new WorldLayer_Hills.<Regenerate>c__IteratorF0();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Hills.<Regenerate>c__IteratorF0 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
