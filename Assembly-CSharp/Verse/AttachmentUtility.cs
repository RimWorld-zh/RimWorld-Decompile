using System;

namespace Verse
{
	// Token: 0x02000DFE RID: 3582
	public static class AttachmentUtility
	{
		// Token: 0x06005139 RID: 20793 RVA: 0x0029BA2C File Offset: 0x00299E2C
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			Thing result;
			if (compAttachBase == null)
			{
				result = null;
			}
			else
			{
				result = compAttachBase.GetAttachment(def);
			}
			return result;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x0029BA5C File Offset: 0x00299E5C
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
