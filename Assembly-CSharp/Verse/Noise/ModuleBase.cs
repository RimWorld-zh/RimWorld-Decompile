using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F81 RID: 3969
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x06005FB1 RID: 24497 RVA: 0x00308AA0 File Offset: 0x00306EA0
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x00308ACC File Offset: 0x00306ECC
		public int SourceModuleCount
		{
			get
			{
				return (this.modules != null) ? this.modules.Length : 0;
			}
		}

		// Token: 0x17000F65 RID: 3941
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

		// Token: 0x06005FB5 RID: 24501
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06005FB6 RID: 24502 RVA: 0x00308BD0 File Offset: 0x00306FD0
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x00308C08 File Offset: 0x00307008
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06005FB8 RID: 24504 RVA: 0x00308C3C File Offset: 0x0030703C
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x00308C70 File Offset: 0x00307070
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x00308C8B File Offset: 0x0030708B
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005FBB RID: 24507 RVA: 0x00308CB0 File Offset: 0x003070B0
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

		// Token: 0x04003EE5 RID: 16101
		protected ModuleBase[] modules = null;

		// Token: 0x04003EE6 RID: 16102
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;
	}
}
