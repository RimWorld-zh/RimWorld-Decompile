using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCE RID: 3278
	public class PatchOperation
	{
		// Token: 0x06004869 RID: 18537 RVA: 0x00260014 File Offset: 0x0025E414
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

		// Token: 0x0600486A RID: 18538 RVA: 0x00260078 File Offset: 0x0025E478
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x0026009C File Offset: 0x0025E49C
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

		// Token: 0x040030F2 RID: 12530
		public string sourceFile;

		// Token: 0x040030F3 RID: 12531
		private bool neverSucceeded = true;

		// Token: 0x040030F4 RID: 12532
		private PatchOperation.Success success = PatchOperation.Success.Normal;

		// Token: 0x02000CCF RID: 3279
		private enum Success
		{
			// Token: 0x040030F6 RID: 12534
			Normal,
			// Token: 0x040030F7 RID: 12535
			Invert,
			// Token: 0x040030F8 RID: 12536
			Always,
			// Token: 0x040030F9 RID: 12537
			Never
		}
	}
}
