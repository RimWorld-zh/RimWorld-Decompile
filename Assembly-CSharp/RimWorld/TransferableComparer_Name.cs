using System;

namespace RimWorld
{
	public class TransferableComparer_Name : TransferableComparer
	{
		public TransferableComparer_Name()
		{
		}

		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
