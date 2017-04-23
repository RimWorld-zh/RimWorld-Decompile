using System;
using System.Collections;
using System.Diagnostics;

namespace RimWorld.Planet
{
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		protected abstract bool ShouldSkip(WorldObject worldObject);

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_WorldObjects.<Regenerate>c__IteratorF9 <Regenerate>c__IteratorF = new WorldLayer_WorldObjects.<Regenerate>c__IteratorF9();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_WorldObjects.<Regenerate>c__IteratorF9 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
