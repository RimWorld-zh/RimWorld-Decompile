using System;

namespace Verse
{
	// Token: 0x02000C30 RID: 3120
	public struct ThingRequest
	{
		// Token: 0x04002E8D RID: 11917
		public ThingDef singleDef;

		// Token: 0x04002E8E RID: 11918
		public ThingRequestGroup group;

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x00241140 File Offset: 0x0023F540
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x0024116C File Offset: 0x0023F56C
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x002411B8 File Offset: 0x0023F5B8
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x002411E8 File Offset: 0x0023F5E8
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00241218 File Offset: 0x0023F618
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x00241248 File Offset: 0x0023F648
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

		// Token: 0x06004499 RID: 17561 RVA: 0x002412A0 File Offset: 0x0023F6A0
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
