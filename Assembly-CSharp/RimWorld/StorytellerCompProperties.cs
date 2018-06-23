using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DB RID: 731
	public class StorytellerCompProperties
	{
		// Token: 0x0400077A RID: 1914
		[TranslationHandle]
		public Type compClass;

		// Token: 0x0400077B RID: 1915
		public float minDaysPassed = 0f;

		// Token: 0x0400077C RID: 1916
		public List<IncidentTargetTypeDef> allowedTargetTypes = null;

		// Token: 0x0400077D RID: 1917
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x06000C11 RID: 3089 RVA: 0x0006B3A4 File Offset: 0x000697A4
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0006B3CA File Offset: 0x000697CA
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0006B3F8 File Offset: 0x000697F8
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0006B429 File Offset: 0x00069829
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}
	}
}
