using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000279 RID: 633
	public class WorldObjectCompProperties
	{
		// Token: 0x0400055A RID: 1370
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);

		// Token: 0x06000ADD RID: 2781 RVA: 0x0006249C File Offset: 0x0006089C
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000624CD File Offset: 0x000608CD
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}
	}
}
