using System;
using System.Linq;

namespace Core.Class
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AliasAttribute : Attribute
    {
        public string Name { get; }
        public AliasAttribute(string name) { Name = name; }
    }

    public static class clsPlcAddress
    {
        public static string _scale;
        public static string[] _bin;
        public static string _workInfo;
        public static string _handYn;
        public static string _batchcount;
        public static string _batchComplate;
        public static string _currentBatch;
        public static string _mixingInfo;
        public static string _mixingTime;
        public static string _workStart;
        public static string _binChange;
        public static string _binLockYn;
        public static string _handComple;
        public static string _workForceComplete;
        public static string _prodMoveTime;
        public static string _isWorkable;
        public static string _mixingComplate;
        public static string _mixingCurrentBatch;
        public static string _workComplete;
        public static string[] _racipe;
        public static string[] _result;
        public static string _ratio;

        public static string _scale2;
        public static string[] _bin2;
        public static string _workInfo2;
        public static string _handYn2;
        public static string _batchcount2;
        public static string _batchComplate2;
        public static string _currentBatch2;
        public static string _mixingInfo2;
        public static string _mixingTime2;
        public static string _workStart2;
        public static string _binChange2;
        public static string _binLockYn2;
        public static string _handComple2;
        public static string _workForceComplete2;
        public static string _prodMoveTime2;
        public static string _isWorkable2;
        public static string _mixingComplate2;
        public static string _mixingCurrentBatch2;
        public static string _workComplete2;
        public static string[] _racipe2;
        public static string[] _result2;
        public static string _ratio2;

        [Alias("SCALE1")]
        public static string Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        [Alias("BIN1")]
        public static string[] Bin
        {
            get { return _bin; }
            set { _bin = value; }
        }

        [Alias("WORKINFO1")]
        public static string WorkInfo
        {
            get { return _workInfo; }
            set { _workInfo = value; }
        }

        [Alias("HANDYN1")]
        public static string HandYn
        {
            get { return _handYn; }
            set { _handYn = value; }
        }

        [Alias("BATCHCOUNT1")]
        public static string BatchCount
        {
            get { return _batchcount; }
            set { _batchcount = value; }
        }

        [Alias("BATCHCOMPLETE1")]
        public static string BatchComPlate
        {
            get { return _batchComplate; }
            set { _batchComplate = value; }
        }

        [Alias("CURRENTBATCH1")]
        public static string CurrentBatch
        {
            get { return _currentBatch; }
            set { _currentBatch = value; }
        }

        [Alias("MIXINGINFO1")]
        public static string MixingInfo
        {
            get { return _mixingInfo; }
            set { _mixingInfo = value; }
        }

        [Alias("MIXINGTIME1")]
        public static string MixingTime
        {
            get { return _mixingTime; }
            set { _mixingTime = value; }
        }

        [Alias("WORKSTART1")]
        public static string WorkStart
        {
            get { return _workStart; }
            set { _workStart = value; }
        }

        [Alias("BINCHANGE1")]
        public static string BinChange
        {
            get { return _binChange; }
            set { _binChange = value; }
        }

        [Alias("BINLOCKYN1")]
        public static string BinLockYn
        {
            get { return _binLockYn; }
            set { _binLockYn = value; }
        }

        [Alias("HANDCOMPLETE1")]
        public static string HandComple
        {
            get { return _handComple; }
            set { _handComple = value; }
        }

        [Alias("WORKFORCECOMPLETE1")]
        public static string WorkForceComplete
        {
            get { return _workForceComplete; }
            set { _workForceComplete = value; }
        }

        [Alias("PRODMOVETIME1")]
        public static string ProdMoveTime
        {
            get { return _prodMoveTime; }
            set { _prodMoveTime = value; }
        }

        [Alias("ISWORKABLE1")]
        public static string IsWorkable
        {
            get { return _isWorkable; }
            set { _isWorkable = value; }
        }

        [Alias("MIXINGCOMPLETE1")]
        public static string MixingComplate
        {
            get { return _mixingComplate; }
            set { _mixingComplate = value; }
        }

        [Alias("MIXINGCURRBATCH1")]
        public static string MixingCurrentBatch
        {
            get { return _mixingCurrentBatch; }
            set { _mixingCurrentBatch = value; }
        }

        [Alias("WORKCOMPLETE1")]
        public static string WorkComplete
        {
            get { return _workComplete; }
            set { _workComplete = value; }
        }

        [Alias("RACIPE1")]
        public static string[] Racipe
        {
            get { return _racipe; }
            set { _racipe = value; }
        }

        [Alias("RESULT1")]
        public static string[] Result
        {
            get { return _result; }
            set { _result = value; }
        }

        [Alias("RATIO1")]
        public static string Ratio
        {
            get { return _ratio; }
            set { _ratio = value; }
        }

        [Alias("SCALE2")]
        public static string Scale2
        {
            get { return _scale2; }
            set { _scale2 = value; }
        }

        [Alias("BIN2")]
        public static string[] Bin2
        {
            get { return _bin2; }
            set { _bin2 = value; }
        }

        [Alias("WORKINFO2")]
        public static string WorkInfo2
        {
            get { return _workInfo2; }
            set { _workInfo2 = value; }
        }

        [Alias("HANDYN2")]
        public static string HandYn2
        {
            get { return _handYn2; }
            set { _handYn2 = value; }
        }

        [Alias("BATCHCOUNT2")]
        public static string BatchCount2
        {
            get { return _batchcount2; }
            set { _batchcount2 = value; }
        }

        [Alias("BATCHCOMPLETE2")]
        public static string BatchComPlate2
        {
            get { return _batchComplate2; }
            set
            {
                _batchComplate2
                    = value;
            }
        }

        [Alias("CURRENTBATCH2")]
        public static string CurrentBatch2
        {
            get { return _currentBatch2; }
            set { _currentBatch2 = value; }
        }

        [Alias("MIXINGINFO2")]
        public static string MixingInfo2
        {
            get { return _mixingInfo2; }
            set { _mixingInfo2 = value; }
        }

        [Alias("MIXINGTIME2")]
        public static string MixingTime2
        {
            get { return _mixingTime2; }
            set { _mixingTime2 = value; }
        }

        [Alias("WORKSTART2")]
        public static string WorkStart2
        {
            get { return _workStart2; }
            set { _workStart2 = value; }
        }

        [Alias("BINCHANGE2")]
        public static string BinChange2
        {
            get { return _binChange2; }
            set { _binChange2 = value; }
        }

        [Alias("BINLOCKYN2")]
        public static string BinLockYn2
        {
            get { return _binLockYn2; }
            set { _binLockYn2 = value; }
        }

        [Alias("HANDCOMPLETE2")]
        public static string HandComple2
        {
            get { return _handComple2; }
            set { _handComple2 = value; }
        }

        [Alias("WORKFORCECOMPLETE2")]
        public static string WorkForceComplete2
        {
            get { return _workForceComplete2; }
            set { _workForceComplete2 = value; }
        }

        [Alias("PRODMOVETIME2")]
        public static string ProdMoveTime2
        {
            get { return _prodMoveTime2; }
            set { _prodMoveTime2 = value; }
        }

        [Alias("ISWORKABLE2")]
        public static string IsWorkable2
        {
            get { return _isWorkable2; }
            set { _isWorkable2 = value; }
        }

        [Alias("MIXINGCOMPLETE2")]
        public static string MixingComplate2
        {
            get { return _mixingComplate2; }
            set { _mixingComplate2 = value; }
        }

        [Alias("MIXINGCURRBATCH2")]
        public static string MixingCurrentBatch2
        {
            get { return _mixingCurrentBatch2; }
            set { _mixingCurrentBatch2 = value; }
        }

        [Alias("WORKCOMPLETE2")]
        public static string WorkComplete2
        {
            get { return _workComplete2; }
            set { _workComplete2 = value; }
        }

        [Alias("RACIPE2")]
        public static string[] Racipe2
        {
            get { return _racipe2; }
            set { _racipe2 = value; }
        }

        [Alias("RESULT2")]
        public static string[] Result2
        {
            get { return _result2; }
            set { _result2 = value; }
        }

        [Alias("RATIO2")]
        public static string Ratio2
        {
            get { return _ratio2; }
            set { _ratio2 = value; }
        }

        public static void SetByAlias(string alias, string value)
        {
            var props = typeof(clsPlcAddress).GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(AliasAttribute), false)
                               .Cast<AliasAttribute>()
                               .FirstOrDefault();

                if (attr != null && attr.Name == alias)
                {
                    // 속성 타입 확인
                    Type propType = prop.PropertyType;

                    if (propType == typeof(string))
                    {
                        // string 타입일 경우 그대로 대입
                        prop.SetValue(null, value);
                    }
                    else if (propType == typeof(string[]))
                    {
                        // string[] 타입일 경우 쉼표 구분 등으로 split 처리
                        string[] arr = value.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        prop.SetValue(null, arr);
                    }
                    else
                    {
                        // 그 외 타입은 문자열 변환 시도
                        try
                        {
                            object converted = Convert.ChangeType(value, propType);
                            prop.SetValue(null, converted);
                        }
                        catch
                        {
                            // 변환 실패 시 무시 또는 로그
                        }
                    }

                    break;
                }
            }
        }
    }
}
