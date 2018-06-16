using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EA RID: 746
	public class TransferableSorterDef : Def
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0006D83C File Offset: 0x0006BC3C
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

		// Token: 0x040007F4 RID: 2036
		public Type comparerClass;

		// Token: 0x040007F5 RID: 2037
		[Unsaved]
		private TransferableComparer comparerInt;
	}
}
