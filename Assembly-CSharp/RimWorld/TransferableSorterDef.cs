using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EC RID: 748
	public class TransferableSorterDef : Def
	{
		// Token: 0x040007F6 RID: 2038
		public Type comparerClass;

		// Token: 0x040007F7 RID: 2039
		[Unsaved]
		private TransferableComparer comparerInt;

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0006DA40 File Offset: 0x0006BE40
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
