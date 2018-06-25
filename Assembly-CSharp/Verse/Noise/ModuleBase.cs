using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F86 RID: 3974
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x04003F02 RID: 16130
		protected ModuleBase[] modules = null;

		// Token: 0x04003F03 RID: 16131
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0030B408 File Offset: 0x00309808
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x0030B434 File Offset: 0x00309834
		public int SourceModuleCount
		{
			get
			{
				return (this.modules != null) ? this.modules.Length : 0;
			}
		}

		// Token: 0x17000F68 RID: 3944
		public virtual ModuleBase this[int index]
		{
			get
			{
				System.Diagnostics.Debug.Assert(this.modules != null);
				System.Diagnostics.Debug.Assert(this.modules.Length > 0);
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (this.modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return this.modules[index];
			}
			set
			{
				System.Diagnostics.Debug.Assert(this.modules.Length > 0);
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				this.modules[index] = value;
			}
		}

		// Token: 0x06005FE8 RID: 24552
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06005FE9 RID: 24553 RVA: 0x0030B538 File Offset: 0x00309938
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x0030B570 File Offset: 0x00309970
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x0030B5A4 File Offset: 0x003099A4
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005FEC RID: 24556 RVA: 0x0030B5D8 File Offset: 0x003099D8
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x0030B5F3 File Offset: 0x003099F3
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x0030B618 File Offset: 0x00309A18
		protected virtual bool Disposing()
		{
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					this.modules[i].Dispose();
					this.modules[i] = null;
				}
				this.modules = null;
			}
			return true;
		}
	}
}
