using System;

namespace Verse
{
	// Token: 0x02000C31 RID: 3121
	public struct ThingRequest
	{
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x0023F9E4 File Offset: 0x0023DDE4
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x0600448A RID: 17546 RVA: 0x0023FA10 File Offset: 0x0023DE10
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0023FA5C File Offset: 0x0023DE5C
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0023FA8C File Offset: 0x0023DE8C
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0023FABC File Offset: 0x0023DEBC
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0023FAEC File Offset: 0x0023DEEC
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

		// Token: 0x0600448F RID: 17551 RVA: 0x0023FB44 File Offset: 0x0023DF44
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

		// Token: 0x04002E7E RID: 11902
		public ThingDef singleDef;

		// Token: 0x04002E7F RID: 11903
		public ThingRequestGroup group;
	}
}
