using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DD RID: 733
	public class StorytellerCompProperties
	{
		// Token: 0x0400077D RID: 1917
		[TranslationHandle]
		public Type compClass;

		// Token: 0x0400077E RID: 1918
		public float minDaysPassed = 0f;

		// Token: 0x0400077F RID: 1919
		public List<IncidentTargetTypeDef> allowedTargetTypes = null;

		// Token: 0x04000780 RID: 1920
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x06000C14 RID: 3092 RVA: 0x0006B4FC File Offset: 0x000698FC
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0006B522 File Offset: 0x00069922
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0006B550 File Offset: 0x00069950
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0006B581 File Offset: 0x00069981
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}
	}
}
