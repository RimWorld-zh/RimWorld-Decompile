using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000821 RID: 2081
	public abstract class Transferable : IExposable
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002EA0 RID: 11936
		public abstract Thing AnyThing { get; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002EA1 RID: 11937
		public abstract ThingDef ThingDef { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002EA2 RID: 11938
		public abstract bool Interactive { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002EA3 RID: 11939
		public abstract bool HasAnyThing { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002EA4 RID: 11940
		public abstract string Label { get; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x00168294 File Offset: 0x00166694
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002EA6 RID: 11942
		public abstract string TipDescription { get; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002EA7 RID: 11943
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002EA8 RID: 11944
		// (set) Token: 0x06002EA9 RID: 11945
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x001682B4 File Offset: 0x001666B4
		public int CountToTransferToSource
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? (-this.CountToTransfer) : this.CountToTransfer;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x001682E8 File Offset: 0x001666E8
		public int CountToTransferToDestination
		{
			get
			{
				return (this.PositiveCountDirection != TransferablePositiveCountDirection.Source) ? this.CountToTransfer : (-this.CountToTransfer);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002EAC RID: 11948 RVA: 0x0016831C File Offset: 0x0016671C
		// (set) Token: 0x06002EAD RID: 11949 RVA: 0x00168337 File Offset: 0x00166737
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

		// Token: 0x06002EAE RID: 11950
		public abstract int GetMinimumToTransfer();

		// Token: 0x06002EAF RID: 11951
		public abstract int GetMaximumToTransfer();

		// Token: 0x06002EB0 RID: 11952 RVA: 0x00168344 File Offset: 0x00166744
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x00168368 File Offset: 0x00166768
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x00168390 File Offset: 0x00166790
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x001683B4 File Offset: 0x001667B4
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

		// Token: 0x06002EB4 RID: 11956 RVA: 0x0016841F File Offset: 0x0016681F
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x00168430 File Offset: 0x00166830
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

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00168470 File Offset: 0x00166870
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x0016847A File Offset: 0x0016687A
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

		// Token: 0x06002EB8 RID: 11960 RVA: 0x0016849C File Offset: 0x0016689C
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

		// Token: 0x06002EB9 RID: 11961 RVA: 0x001684C0 File Offset: 0x001668C0
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x001684DC File Offset: 0x001668DC
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x001684F7 File Offset: 0x001668F7
		public virtual void ExposeData()
		{
		}

		// Token: 0x04001903 RID: 6403
		private string editBuffer = "0";
	}
}
