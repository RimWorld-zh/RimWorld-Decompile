using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F82 RID: 3970
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x06005FB3 RID: 24499 RVA: 0x003089C4 File Offset: 0x00306DC4
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x003089F0 File Offset: 0x00306DF0
		public int SourceModuleCount
		{
			get
			{
				return (this.modules != null) ? this.modules.Length : 0;
			}
		}

		// Token: 0x17000F66 RID: 3942
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

		// Token: 0x06005FB7 RID: 24503
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06005FB8 RID: 24504 RVA: 0x00308AF4 File Offset: 0x00306EF4
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x00308B2C File Offset: 0x00306F2C
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x00308B60 File Offset: 0x00306F60
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06005FBB RID: 24507 RVA: 0x00308B94 File Offset: 0x00306F94
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06005FBC RID: 24508 RVA: 0x00308BAF File Offset: 0x00306FAF
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x00308BD4 File Offset: 0x00306FD4
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

		// Token: 0x04003EE6 RID: 16102
		protected ModuleBase[] modules = null;

		// Token: 0x04003EE7 RID: 16103
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;
	}
}
