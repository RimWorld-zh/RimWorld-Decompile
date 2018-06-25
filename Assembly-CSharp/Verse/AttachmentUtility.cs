using System;

namespace Verse
{
	public static class AttachmentUtility
	{
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

		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
