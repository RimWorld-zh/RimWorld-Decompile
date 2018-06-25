using System;

namespace RimWorld
{
	public class TransferableComparer_None : TransferableComparer
	{
		public TransferableComparer_None()
		{
		}

		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return 0;
		}
	}
}
