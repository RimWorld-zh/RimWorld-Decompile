using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCE RID: 3278
	public class PatchOperation
	{
		// Token: 0x04003104 RID: 12548
		public string sourceFile;

		// Token: 0x04003105 RID: 12549
		private bool neverSucceeded = true;

		// Token: 0x04003106 RID: 12550
		private PatchOperation.Success success = PatchOperation.Success.Normal;

		// Token: 0x0600487D RID: 18557 RVA: 0x002617E8 File Offset: 0x0025FBE8
		public bool Apply(XmlDocument xml)
		{
			bool flag = this.ApplyWorker(xml);
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x0026184C File Offset: 0x0025FC4C
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00261870 File Offset: 0x0025FC70
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text, false);
			}
		}

		// Token: 0x02000CCF RID: 3279
		private enum Success
		{
			// Token: 0x04003108 RID: 12552
			Normal,
			// Token: 0x04003109 RID: 12553
			Invert,
			// Token: 0x0400310A RID: 12554
			Always,
			// Token: 0x0400310B RID: 12555
			Never
		}
	}
}
