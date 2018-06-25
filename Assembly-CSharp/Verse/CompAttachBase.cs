using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000DFF RID: 3583
	public class CompAttachBase : ThingComp
	{
		// Token: 0x0400354B RID: 13643
		public List<AttachableThing> attachments = null;

		// Token: 0x06005136 RID: 20790 RVA: 0x0029B974 File Offset: 0x00299D74
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

		// Token: 0x06005137 RID: 20791 RVA: 0x0029B9CC File Offset: 0x00299DCC
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

		// Token: 0x06005138 RID: 20792 RVA: 0x0029BA24 File Offset: 0x00299E24
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

		// Token: 0x06005139 RID: 20793 RVA: 0x0029BA94 File Offset: 0x00299E94
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

		// Token: 0x0600513A RID: 20794 RVA: 0x0029BB00 File Offset: 0x00299F00
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x0029BB22 File Offset: 0x00299F22
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x0029BB47 File Offset: 0x00299F47
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}
	}
}
