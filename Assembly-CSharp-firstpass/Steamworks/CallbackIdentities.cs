using System;

namespace Steamworks
{
	// Token: 0x0200000C RID: 12
	internal class CallbackIdentities
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000283C File Offset: 0x00000A3C
		public static int GetCallbackIdentity(Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new Exception("Callback number not found for struct " + callbackStruct);
			}
			CallbackIdentityAttribute callbackIdentityAttribute = (CallbackIdentityAttribute)customAttributes[num];
			return callbackIdentityAttribute.Identity;
		}
	}
}
