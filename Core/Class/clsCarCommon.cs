using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsCarCommon
    {
        /// <summary>
        /// 차량마스터
        /// </summary>
        /// <param name="carFullNum"></param>
        /// <returns></returns>
        public static DataSet GetCarMaster(string sVEHICLENO = "", string Name = "")
        {
            string SQL = $@"
            -- 차량마스터
            SELECT 
                VEHICLECODE                         -- 차량코드
                , TMSDIVISIONCODE               -- TMS 디비전코드
                , TMSLOGISTICGROUP              -- TMS 물류운영그룹코드
                , VEHICLENO                             -- 차량번호
                , VEHICLETONCODE                   -- 차량톤급
                , VEHICLETONNAME                    -- 차량 톤급명칭
                , VEHICLEGROUPCODE              -- 차량그룹코드
                , VEHICLEGROUPNAME              -- 차량그룹명칭
                , DRIVERNAME                            -- 기사명
                , DRIVERMOBILE                          -- 기사휴대폰
                , ISREQUIRESIGNATURE            -- 서명 필요 여부
                , ISREQUIRESEAL                         -- 봉인 필요 여부
                , LIVESTOCKVEHICLENO            -- 축산차량등록번호
                , CARRIERCODE                           -- 운송사 코드
                , CARRIERNAME                           -- 운송사 명
                , TRANSACTIONFLAG                   -- 트랜잭션 FLAG
                , NVL(USE_YN, 'Y') AS USE_YN
                , REGISTERAT                                -- 등록일시
                , REGISTERBY                                --  등록자
            FROM TMS_INPUT_CARMASTER_CON
            WHERE ('{sVEHICLENO}' IS NULL OR VEHICLENO LIKE '%{sVEHICLENO}%')
                AND ('{Name}' IS NULL OR DRIVERNAME LIKE '%{Name}%')
            ";

            using (DataSet carDs = Dbconn.conn.ExecutDataset(SQL))
            {
                return carDs;
            }
        }

        public static DataSet GetDecar(string sINCAR_NO = "")
        {
            string SQL = $@"
            SELECT 
                IS_NO           -- 발급번호
                , CUST_CODE     -- 공급자코드
                , INCAR_NO      -- 차량전체번호
                , TEM_TYPE      -- 임시여부
                , CAR_TYPE      -- 차량입고타입
                , CAR_TDETAIL   -- 입고타입상세
                , D_NAME        -- 운전자
                , IN_WEIGHT     -- 입차중량
                , OUT_WEIGHT    -- 출차중량
                , USER_ID       -- 확인관리자코드
                , INCAR_DATE    -- 일문일
                , INCAR_TIME    -- 입문시
                , OUTCAR_DATE   -- 출문일
                , OUTCAR_TIME   -- 출문시
                , OUTCAR_CHK    -- 검근시간
                , CHKIN_TIME    -- 체크인시간
                , PC_STATUS     -- 공정진행상태
                , SEAL_NUM      -- 봉인번호
                , SCALE_CHART   -- 검근표
                , GO_MAYN       -- 하차내역수동입력여부
                , DEP_CHECK     -- 이중입력여부
                , ORDER_NO      -- 배차지시번호
                , U_TIME        -- 업데이트 일시
                , U_USER        -- 업데이트 자
            FROM WAP_DECAR
            WHERE ('{sINCAR_NO}' IS NULL OR INCAR_NO = '{sINCAR_NO}')
            ";

            using (DataSet carDs = Dbconn.conn.ExecutDataset(SQL))
            {
                return carDs;
            }
        }
    }
}
