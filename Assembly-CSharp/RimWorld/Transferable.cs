using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000823 RID: 2083
	public abstract class Transferable : IExposable
	{
		// Token: 0x04001907 RID: 6407
		private string editBuffer = "0";

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002EA3 RID: 11939
		public abstract Thing AnyThing { get; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002EA4 RID: 11940
		public abstract ThingDef ThingDef { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002EA5 RID: 11941
		public abstract bool Interactive { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002EA6 RID: 11942
		public abstract bool HasAnyThing { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002EA7 RID: 11943
		public abstract string Label { get; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x00168644 File Offset: 0x00166A44
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002EA9 RID: 11945
		public abstract string TipDescription { get; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002EAA RID: 11946
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002EAB RID: 11947
		// (set) Token: 0x06002EAC RID: 11948
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x00168664 File Offset: 0x00166A64
		public int CountToTransferToSource
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? (-this.CountToTransfer) : this.CountToTransfer;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x00168698 File Offset: 0x00166A98
		public int CountToTransferToDestination
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? this.CountToTransfer : (-this.CountToTransfer);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x001686CC File Offset: 0x00166ACC
		// (set) Token: 0x06002EB0 RID: 11952 RVA: 0x001686E7 File Offset: 0x00166AE7
		public string EditBuffer
		{
			get
			{
				return this.editBuffer;
			}
			set
			{
				this.editBuffer = value;
			}
		}

		// Token: 0x06002EB1 RID: 11953
		public abstract int GetMinimumToTransfer();

		// Token: 0x06002EB2 RID: 11954
		public abstract int GetMaximumToTransfer();

		// Token: 0x06002EB3 RID: 11955 RVA: 0x001686F4 File Offset: 0x00166AF4
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x00168718 File Offset: 0x00166B18
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x00168740 File Offset: 0x00166B40
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00168764 File Offset: 0x00166B64
		public AcceptanceReport CanAdjustTo(int destination)
		{
			AcceptanceReport result;
			if (destination == this.CountToTransfer)
			{
				result = AcceptanceReport.WasAccepted;
			}
			else
			{
				int num = this.ClampAmount(destination);
				if (num != this.CountToTransfer)
				{
					result = AcceptanceReport.WasAccepted;
				}
				else if (destination < this.CountToTransfer)
				{
					result = this.UnderflowReport();
				}
				else
				{
					result = this.OverflowReport();
				}
			}
			return result;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x001687CF File Offset: 0x00166BCF
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x001687E0 File Offset: 0x00166BE0
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts", false);
			}
			else
			{
				this.CountToTransfer = this.ClampAmount(destination);
			}
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x00168820 File Offset: 0x00166C20
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x0016882A File Offset: 0x00166C2A
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
			}
			else
			{
				this.ForceTo(-value);
			}
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x0016884C File Offset: 0x00166C4C
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
			}
			else
			{
				this.ForceTo(value);
			}
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x00168870 File Offset: 0x00166C70
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x0016888C File Offset: 0x00166C8C
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x001688A7 File Offset: 0x00166CA7
		public virtual void ExposeData()
		{
		}
	}
}
