using System.Collections;
using System.Collections.Generic;

public class CPacketType
{
    enum ePacketType
    {
		CS_PT_LOGIN = 1,
		CS_PT_LOGOUT,
		CS_PT_USERLIST,
		CS_PT_ROOMLIST,
		CS_PT_CHAT,
		CS_PT_CREATEROOM,
		CS_PT_ROOMIN,
		CS_PT_ROOMOUT,
		CS_PT_ROOMSTATE,
		CS_PT_TEAMCHANGE,
		CS_PT_READY,
		CS_PT_START,
		CS_PT_MAX
	}
}
