using System;

namespace Verse
{
	// Token: 0x02000E01 RID: 3585
	public static class AttachmentUtility
	{
		// Token: 0x06005125 RID: 20773 RVA: 0x0029A450 File Offset: 0x00298850
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

		// Token: 0x06005126 RID: 20774 RVA: 0x0029A480 File Offset: 0x00298880
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
