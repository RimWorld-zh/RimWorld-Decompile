using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCD RID: 3277
	public class PatchOperation
	{
		// Token: 0x040030FD RID: 12541
		public string sourceFile;

		// Token: 0x040030FE RID: 12542
		private bool neverSucceeded = true;

		// Token: 0x040030FF RID: 12543
		private PatchOperation.Success success = PatchOperation.Success.Normal;

		// Token: 0x0600487D RID: 18557 RVA: 0x00261508 File Offset: 0x0025F908
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

		// Token: 0x0600487E RID: 18558 RVA: 0x0026156C File Offset: 0x0025F96C
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00261590 File Offset: 0x0025F990
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

		// Token: 0x02000CCE RID: 3278
		private enum Success
		{
			// Token: 0x04003101 RID: 12545
			Normal,
			// Token: 0x04003102 RID: 12546
			Invert,
			// Token: 0x04003103 RID: 12547
			Always,
			// Token: 0x04003104 RID: 12548
			Never
		}
	}
}
