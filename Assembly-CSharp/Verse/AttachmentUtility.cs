using System;

namespace Verse
{
	// Token: 0x02000E02 RID: 3586
	public static class AttachmentUtility
	{
		// Token: 0x06005127 RID: 20775 RVA: 0x0029A470 File Offset: 0x00298870
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

		// Token: 0x06005128 RID: 20776 RVA: 0x0029A4A0 File Offset: 0x002988A0
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
