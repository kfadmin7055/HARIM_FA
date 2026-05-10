using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum TaransFlag
    {
        [Description("입력")]
        INPUT = 1,

        [Description("전송")]
        TRANS = 2
    }

    public enum PlcAddressType
    {
        /// <summary>
        /// 스케일
        /// </summary>
        [Description("SCALE")]            
        SCALE,

        /// <summary>
        /// 빈
        /// </summary>
        [Description("BIN")]              
        BIN,

        /// <summary>
        /// 작지정보
        /// </summary>
        [Description("WORKINFO")]         
        WORKINFO,

        /// <summary>
        /// 수작업여부
        /// </summary>
        [Description("HANDYN")]           
        HANDYN,

        /// <summary>
        /// 배치수
        /// </summary>
        [Description("BATCHCOUNT")]
        BATCHCOUNT,

        /// <summary>
        /// 현재배치번호
        /// </summary>
        [Description("CURRENTBATCH")]     
        CURRENTBATCH,

        /// <summary>
        /// 믹싱작지정보
        /// </summary>
        [Description("MIXINGINFO")]       
        MIXINGINFO,

        /// <summary>
        /// 믹싱시간
        /// </summary>
        [Description("MIXINGTIME")]       
        MIXINGTIME,

        /// <summary>
        /// 작업시작여부
        /// </summary>
        [Description("WORKSTART")]        
        WORKSTART,

        /// <summary>
        /// 빈교체
        /// </summary>
        [Description("BINCHANGE")]        
        BINCHANGE,

        /// <summary>
        /// 빈보류
        /// </summary>
        [Description("BINLOCKYN")]        
        BINLOCKYN,

        /// <summary>
        /// 수작업완료여부
        /// </summary>
        [Description("HANDCOMPLE")]       
        HANDCOMPLE,

        /// <summary>
        /// 작업강제완료여부
        /// </summary>
        [Description("WORKFORCECOMPLETE")]
        WORKFORCECOMPLETE,

        /// <summary>
        /// 배합이송시간
        /// </summary>
        [Description("PRODMOVETIME")]     
        PRODMOVETIME,

        /// <summary>
        /// 작업가능여부
        /// </summary>
        [Description("ISWORKABLE")]       
        ISWORKABLE,

        /// <summary>
        /// 배치완료
        /// </summary>
        [Description("BATCHCOMPLETE")]
        BATCHCOMPLETE,

        /// <summary>
        /// 작업완료
        /// </summary>
        [Description("WORKCOMPLETE")]
        WORKCOMPLETE,

        /// <summary>
        /// 빈변경가능여부
        /// </summary>
        [Description("ISBINCHANGE")]
        ISBINCHANGE,

        /// <summary>
        /// 빈보류가능여부
        /// </summary>
        [Description("ISBINLOCKABLE")]
        ISBINLOCKABLE,

        /// <summary>
        /// 믹싱완료
        /// </summary>
        [Description("MIXINGCOMPLETE")]
        MIXINGCOMPLETE,

        /// <summary>
        /// 믹싱현배치번호
        /// </summary>
        [Description("MIXINGCURRBATCH")]
        MIXINGCURRBATCH,

        /// <summary>
        /// 페시피
        /// </summary>
        [Description("RACIPE")]           
        RACIPE,

        /// <summary>
        /// 실적
        /// </summary>
        [Description("RESULT")]           
        RESULT,

        /// <summary>
        /// 실적
        /// </summary>
        [Description("RATIO")]
        RATIO,
    }
}
