namespace Verse
{
	public static class AttachmentUtility
	{
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			return (compAttachBase != null) ? compAttachBase.GetAttachment(def) : null;
		}

		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
