using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024C RID: 588
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0005F352 File Offset: 0x0005D752
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0005F384 File Offset: 0x0005D784
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

		// Token: 0x0400049B RID: 1179
		public float width = 8f;

		// Token: 0x0400049C RID: 1180
		public Color color = Color.white;

		// Token: 0x0400049D RID: 1181
		public SoundDef sound;
	}
}
