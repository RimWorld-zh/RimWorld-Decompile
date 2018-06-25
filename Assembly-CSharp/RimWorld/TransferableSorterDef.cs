using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EC RID: 748
	public class TransferableSorterDef : Def
	{
		// Token: 0x040007F9 RID: 2041
		public Type comparerClass;

		// Token: 0x040007FA RID: 2042
		[Unsaved]
		private TransferableComparer comparerInt;

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0006DA48 File Offset: 0x0006BE48
		public TransferableComparer Comparer
		{
			get
			{
				if (this.comparerInt == null)
				{
					this.comparerInt = (TransferableComparer)Activator.CreateInstance(this.comparerClass);
				}
				return this.comparerInt;
			}
		}
	}
}
