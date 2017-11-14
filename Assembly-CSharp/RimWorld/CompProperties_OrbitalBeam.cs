using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompProperties_OrbitalBeam : CompProperties
	{
		public float width = 8f;

		public Color color = Color.white;

		public SoundDef sound;

		public CompProperties_OrbitalBeam()
		{
			base.compClass = typeof(CompOrbitalBeam);
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (parentDef.drawerType == DrawerType.RealtimeOnly)
				yield break;
			if (parentDef.drawerType == DrawerType.MapMeshAndRealTime)
				yield break;
			yield return "orbital beam requires realtime drawer";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0104:
			/*Error near IL_0105: Unexpected return in MoveNext()*/;
		}
	}
}
