using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F81 RID: 3969
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x04003EF7 RID: 16119
		protected ModuleBase[] modules = null;

		// Token: 0x04003EF8 RID: 16120
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;

		// Token: 0x06005FDA RID: 24538 RVA: 0x0030AB44 File Offset: 0x00308F44
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005FDB RID: 24539 RVA: 0x0030AB70 File Offset: 0x00308F70
		public int SourceModuleCount
		{
			get
			{
				return (this.modules != null) ? this.modules.Length : 0;
			}
		}

		// Token: 0x17000F69 RID: 3945
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

		// Token: 0x06005FDE RID: 24542
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06005FDF RID: 24543 RVA: 0x0030AC74 File Offset: 0x00309074
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x0030ACAC File Offset: 0x003090AC
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0030ACE0 File Offset: 0x003090E0
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005FE2 RID: 24546 RVA: 0x0030AD14 File Offset: 0x00309114
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0030AD2F File Offset: 0x0030912F
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0030AD54 File Offset: 0x00309154
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
