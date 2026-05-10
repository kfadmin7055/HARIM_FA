using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum enumPcStatus
    {
        [Description("계획")]
        Plan = 031002,

        [Description("진행")]
        InProgress = 031003,

        [Description("완료")]
        Completed = 031004,

        [Description("취소")]
        Canceled = 031005,

        [Description("강제완료")]
        ForceCompleted = 031006,

        [Description("보류")]
        OnHold = 031007
    }
}
