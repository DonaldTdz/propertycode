using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class AlipayCommunityParameter
    {
        /// <summary>
        /// 小区名称
        /// </summary>
        public string community_name { get; set; }
        /// <summary>
        /// 小区地址
        /// </summary>
        public string community_address { get; set; }
        /// <summary>
        /// 区县编码
        /// </summary>
        public string district_code { get; set; }
        /// <summary>
        /// 区县编码-名字
        /// </summary>
        public string district_Name { get; set; }
        /// <summary>
        /// 地级市编码
        /// </summary>
        public string city_code { get; set; }
        /// <summary>
        /// 地级市名称
        /// </summary>
        public string city_Name { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>
        public string province_code { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string province_Name { get; set; }
        /// <summary>
        /// 小区经纬度
        /// </summary>
        public string community_locations { get; set; }
        /// <summary>
        /// 物业电话
        /// </summary>
        public string hotline { get; set; }
        /// <summary>
        /// 已有小区
        /// </summary>
        public string out_community_id { get; set; }

    }
}
