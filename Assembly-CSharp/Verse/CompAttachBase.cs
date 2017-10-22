using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public class CompAttachBase : ThingComp
	{
		public List<AttachableThing> attachments = null;

		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = base.parent.Position;
				}
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int num = this.attachments.Count - 1; num >= 0; num--)
				{
					this.attachments[num].Destroy(DestroyMode.Vanish);
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			else
			{
				result = (string)null;
			}
			return result;
		}

		public Thing GetAttachment(ThingDef def)
		{
			int i;
			if (this.attachments != null)
			{
				for (i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
						goto IL_002c;
				}
			}
			Thing result = null;
			goto IL_005c;
			IL_002c:
			result = this.attachments[i];
			goto IL_005c;
			IL_005c:
			return result;
		}

		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}
	}
}
