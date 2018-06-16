using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000277 RID: 631
	public class WorldObjectCompProperties
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x000622F4 File Offset: 0x000606F4
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00062325 File Offset: 0x00060725
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x0400055A RID: 1370
		public Type compClass = typeof(WorldObjectComp);
	}
}
