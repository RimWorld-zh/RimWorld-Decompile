using System;

namespace Verse
{
	// Token: 0x02000C2F RID: 3119
	public struct ThingRequest
	{
		// Token: 0x04002E86 RID: 11910
		public ThingDef singleDef;

		// Token: 0x04002E87 RID: 11911
		public ThingRequestGroup group;

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x00240E60 File Offset: 0x0023F260
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x00240E8C File Offset: 0x0023F28C
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x00240ED8 File Offset: 0x0023F2D8
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00240F08 File Offset: 0x0023F308
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00240F38 File Offset: 0x0023F338
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x00240F68 File Offset: 0x0023F368
		public bool Accepts(Thing t)
		{
			bool result;
			if (this.singleDef != null)
			{
				result = (t.def == this.singleDef);
			}
			else
			{
				result = (this.group == ThingRequestGroup.Everything || this.group.Includes(t.def));
			}
			return result;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x00240FC0 File Offset: 0x0023F3C0
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}
	}
}
