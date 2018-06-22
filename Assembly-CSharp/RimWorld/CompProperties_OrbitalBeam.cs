using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024C RID: 588
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x0005F3AE File Offset: 0x0005D7AE
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0005F3E0 File Offset: 0x0005D7E0
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
			yield break;
		}

		// Token: 0x04000499 RID: 1177
		public float width = 8f;

		// Token: 0x0400049A RID: 1178
		public Color color = Color.white;

		// Token: 0x0400049B RID: 1179
		public SoundDef sound;
	}
}
