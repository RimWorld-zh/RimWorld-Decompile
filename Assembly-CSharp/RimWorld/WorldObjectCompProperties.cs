using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000279 RID: 633
	public class WorldObjectCompProperties
	{
		// Token: 0x04000558 RID: 1368
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);

		// Token: 0x06000ADE RID: 2782 RVA: 0x000624A0 File Offset: 0x000608A0
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000624D1 File Offset: 0x000608D1
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}
	}
}
