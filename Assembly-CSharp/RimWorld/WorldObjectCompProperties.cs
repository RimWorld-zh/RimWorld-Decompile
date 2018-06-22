using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000277 RID: 631
	public class WorldObjectCompProperties
	{
		// Token: 0x06000ADA RID: 2778 RVA: 0x00062350 File Offset: 0x00060750
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00062381 File Offset: 0x00060781
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x04000558 RID: 1368
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
