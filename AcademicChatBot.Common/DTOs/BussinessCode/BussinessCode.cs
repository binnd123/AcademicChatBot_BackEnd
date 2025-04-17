using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.DTOs.BussinessCode
{
    public enum BusinessCode
    {
        GET_DATA_SUCCESSFULLY = 2000,
        EXCEPTION = 4000,
        INVALID_REFRESHTOKEN = 404,
        EXPIRED_REFRESHTOKEN = 402,
        SIGN_UP_SUCCESSFULLY = 2001,
        SIGN_IN_SUCCESSFULLY = 2007,
        UPDATE_SUCCESSFULLY = 2004,
        DELETE_SUCCESSFULLY = 2006,
        SIGN_UP_FAILED = 2002,
        EXISTED_USER = 2003,
        AUTH_NOT_FOUND = 401,
        WRONG_PASSWORD = 405,
        ACCESS_DENIED = 403,
        INVALID_EMAIL_FPTU = 406,
    }
}
