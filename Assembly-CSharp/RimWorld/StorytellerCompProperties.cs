using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DD RID: 733
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

		// Token: 0x06000C15 RID: 3093 RVA: 0x0006B4F4 File Offset: 0x000698F4
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0006B51A File Offset: 0x0006991A
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0006B548 File Offset: 0x00069948
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0006B579 File Offset: 0x00069979
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}
	}
}
