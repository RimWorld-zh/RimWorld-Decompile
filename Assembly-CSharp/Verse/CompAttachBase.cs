using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000E00 RID: 3584
	public class CompAttachBase : ThingComp
	{
		// Token: 0x0600511E RID: 20766 RVA: 0x0029A26C File Offset: 0x0029866C
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x0029A2C4 File Offset: 0x002986C4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x0029A31C File Offset: 0x0029871C
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
				result = null;
			}
			return result;
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x0029A38C File Offset: 0x0029878C
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x0029A3F8 File Offset: 0x002987F8
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x0029A41A File Offset: 0x0029881A
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x0029A43F File Offset: 0x0029883F
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x04003544 RID: 13636
		public List<AttachableThing> attachments = null;
	}
}
