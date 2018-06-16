using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCF RID: 3279
	public class PatchOperation
	{
		// Token: 0x0600486B RID: 18539 RVA: 0x0026003C File Offset: 0x0025E43C
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

		// Token: 0x0600486C RID: 18540 RVA: 0x002600A0 File Offset: 0x0025E4A0
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x002600C4 File Offset: 0x0025E4C4
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

		// Token: 0x040030F4 RID: 12532
		public string sourceFile;

		// Token: 0x040030F5 RID: 12533
		private bool neverSucceeded = true;

		// Token: 0x040030F6 RID: 12534
		private PatchOperation.Success success = PatchOperation.Success.Normal;

		// Token: 0x02000CD0 RID: 3280
		private enum Success
		{
			// Token: 0x040030F8 RID: 12536
			Normal,
			// Token: 0x040030F9 RID: 12537
			Invert,
			// Token: 0x040030FA RID: 12538
			Always,
			// Token: 0x040030FB RID: 12539
			Never
		}
	}
}
