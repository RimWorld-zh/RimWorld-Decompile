using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020002DB RID: 731
	public class StorytellerCompProperties
	{
		// Token: 0x06000C13 RID: 3091 RVA: 0x0006B33C File Offset: 0x0006973C
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0006B362 File Offset: 0x00069762
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0006B390 File Offset: 0x00069790
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0006B3C1 File Offset: 0x000697C1
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x0400077B RID: 1915
		public Type compClass;

		// Token: 0x0400077C RID: 1916
		public float minDaysPassed = 0f;

		// Token: 0x0400077D RID: 1917
		public List<IncidentTargetTypeDef> allowedTargetTypes = null;

		// Token: 0x0400077E RID: 1918
		public float minIncChancePopulationIntentFactor = 0.05f;
	}
}
