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
			WorldLayer_WorldObjects.<Regenerate>c__IteratorFA <Regenerate>c__IteratorFA = new WorldLayer_WorldObjects.<Regenerate>c__IteratorFA();
			<Regenerate>c__IteratorFA.<>f__this = this;
			WorldLayer_WorldObjects.<Regenerate>c__IteratorFA expr_0E = <Regenerate>c__IteratorFA;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
