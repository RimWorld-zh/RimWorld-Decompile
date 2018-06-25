using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000E00 RID: 3584
	public class CompAttachBase : ThingComp
	{
		// Token: 0x04003552 RID: 13650
		public List<AttachableThing> attachments = null;

		// Token: 0x06005136 RID: 20790 RVA: 0x0029BC54 File Offset: 0x0029A054
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

		// Token: 0x06005137 RID: 20791 RVA: 0x0029BCAC File Offset: 0x0029A0AC
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

		// Token: 0x06005138 RID: 20792 RVA: 0x0029BD04 File Offset: 0x0029A104
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

		// Token: 0x06005139 RID: 20793 RVA: 0x0029BD74 File Offset: 0x0029A174
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

		// Token: 0x0600513A RID: 20794 RVA: 0x0029BDE0 File Offset: 0x0029A1E0
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x0029BE02 File Offset: 0x0029A202
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x0029BE27 File Offset: 0x0029A227
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}
	}
}
