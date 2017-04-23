using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld.Planet
{
	public class DestroyedFactionBase : MapParent
	{
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			DestroyedFactionBase.<GetGizmos>c__Iterator103 <GetGizmos>c__Iterator = new DestroyedFactionBase.<GetGizmos>c__Iterator103();
			<GetGizmos>c__Iterator.<>f__this = this;
			DestroyedFactionBase.<GetGizmos>c__Iterator103 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
