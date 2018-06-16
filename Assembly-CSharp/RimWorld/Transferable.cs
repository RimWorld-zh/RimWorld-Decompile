using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000825 RID: 2085
	public abstract class Transferable : IExposable
	{
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002EA5 RID: 11941
		public abstract Thing AnyThing { get; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002EA6 RID: 11942
		public abstract ThingDef ThingDef { get; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002EA7 RID: 11943
		public abstract bool Interactive { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002EA8 RID: 11944
		public abstract bool HasAnyThing { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002EA9 RID: 11945
		public abstract string Label { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x00168028 File Offset: 0x00166428
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002EAB RID: 11947
		public abstract string TipDescription { get; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002EAC RID: 11948
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002EAD RID: 11949
		// (set) Token: 0x06002EAE RID: 11950
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x00168048 File Offset: 0x00166448
		public int CountToTransferToSource
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? (-this.CountToTransfer) : this.CountToTransfer;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x0016807C File Offset: 0x0016647C
		public int CountToTransferToDestination
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? this.CountToTransfer : (-this.CountToTransfer);
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002EB1 RID: 11953 RVA: 0x001680B0 File Offset: 0x001664B0
		// (set) Token: 0x06002EB2 RID: 11954 RVA: 0x001680CB File Offset: 0x001664CB
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

		// Token: 0x06002EB3 RID: 11955
		public abstract int GetMinimumToTransfer();

		// Token: 0x06002EB4 RID: 11956
		public abstract int GetMaximumToTransfer();

		// Token: 0x06002EB5 RID: 11957 RVA: 0x001680D8 File Offset: 0x001664D8
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x001680FC File Offset: 0x001664FC
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00168124 File Offset: 0x00166524
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x00168148 File Offset: 0x00166548
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

		// Token: 0x06002EB9 RID: 11961 RVA: 0x001681B3 File Offset: 0x001665B3
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x001681C4 File Offset: 0x001665C4
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

		// Token: 0x06002EBB RID: 11963 RVA: 0x00168204 File Offset: 0x00166604
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x0016820E File Offset: 0x0016660E
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

		// Token: 0x06002EBD RID: 11965 RVA: 0x00168230 File Offset: 0x00166630
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

		// Token: 0x06002EBE RID: 11966 RVA: 0x00168254 File Offset: 0x00166654
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x00168270 File Offset: 0x00166670
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x0016828B File Offset: 0x0016668B
		public virtual void ExposeData()
		{
		}

		// Token: 0x04001905 RID: 6405
		private string editBuffer = "0";
	}
}
