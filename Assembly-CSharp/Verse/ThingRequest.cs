using System;

namespace Verse
{
	// Token: 0x02000C30 RID: 3120
	public struct ThingRequest
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004487 RID: 17543 RVA: 0x0023F9BC File Offset: 0x0023DDBC
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004488 RID: 17544 RVA: 0x0023F9E8 File Offset: 0x0023DDE8
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0023FA34 File Offset: 0x0023DE34
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0023FA64 File Offset: 0x0023DE64
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0023FA94 File Offset: 0x0023DE94
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0023FAC4 File Offset: 0x0023DEC4
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

		// Token: 0x0600448D RID: 17549 RVA: 0x0023FB1C File Offset: 0x0023DF1C
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

		// Token: 0x04002E7C RID: 11900
		public ThingDef singleDef;

		// Token: 0x04002E7D RID: 11901
		public ThingRequestGroup group;
	}
}
