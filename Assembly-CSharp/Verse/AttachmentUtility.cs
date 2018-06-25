using System;

namespace Verse
{
	// Token: 0x02000E01 RID: 3585
	public static class AttachmentUtility
	{
		// Token: 0x0600513D RID: 20797 RVA: 0x0029BE38 File Offset: 0x0029A238
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

		// Token: 0x0600513E RID: 20798 RVA: 0x0029BE68 File Offset: 0x0029A268
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
