using System;

namespace Verse
{
	// Token: 0x02000E00 RID: 3584
	public static class AttachmentUtility
	{
		// Token: 0x0600513D RID: 20797 RVA: 0x0029BB58 File Offset: 0x00299F58
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

		// Token: 0x0600513E RID: 20798 RVA: 0x0029BB88 File Offset: 0x00299F88
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
