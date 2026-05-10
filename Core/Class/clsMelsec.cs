using ACTETHERLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public static class clsMelsec
    {
        //MELSEC PLC 연결 COM Class 생성자
        public static ActQJ71E71UDP plc_dosing = new ActQJ71E71UDP();
        public static ActQJ71E71UDP plc_cotter = new ActQJ71E71UDP();

        public static ActAJ71E71TCP plc_MicroT = new ActAJ71E71TCP();

        public static ActAJ71E71UDP plc_MicroU = new ActAJ71E71UDP();

        public static ActQJ71E71UDP plc_bin = new ActQJ71E71UDP();

        public static ActQJ71E71UDP plc_scale_dosing = new ActQJ71E71UDP();
        public static ActQJ71E71UDP plc_scale_cotter = new ActQJ71E71UDP();
    }
}
