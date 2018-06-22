using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000DFD RID: 3581
	public class CompAttachBase : ThingComp
	{
		// Token: 0x06005132 RID: 20786 RVA: 0x0029B848 File Offset: 0x00299C48
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

		// Token: 0x06005133 RID: 20787 RVA: 0x0029B8A0 File Offset: 0x00299CA0
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

		// Token: 0x06005134 RID: 20788 RVA: 0x0029B8F8 File Offset: 0x00299CF8
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

		// Token: 0x06005135 RID: 20789 RVA: 0x0029B968 File Offset: 0x00299D68
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

		// Token: 0x06005136 RID: 20790 RVA: 0x0029B9D4 File Offset: 0x00299DD4
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x0029B9F6 File Offset: 0x00299DF6
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x0029BA1B File Offset: 0x00299E1B
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x0400354B RID: 13643
		public List<AttachableThing> attachments = null;
	}
}
