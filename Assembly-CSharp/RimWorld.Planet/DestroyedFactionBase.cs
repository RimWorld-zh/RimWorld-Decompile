using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class DestroyedFactionBase : MapParent
	{
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			bool result;
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				result = true;
			}
			else
			{
				alsoRemoveWorldObject = false;
				result = false;
			}
			return result;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!base.HasMap)
				yield break;
			if (Find.WorldSelector.SingleSelectedObject != this)
				yield break;
			yield return (Gizmo)SettleInExistingMapUtility.SettleCommand(base.Map, false);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0111:
			/*Error near IL_0112: Unexpected return in MoveNext()*/;
		}
	}
}
