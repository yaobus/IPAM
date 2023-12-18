using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPAM2.ViewMode
{
    public class ViewMode
    {


        /// <summary>
        /// 自定义单个IP详情信息1
        /// </summary>
        public class IpInfo
        {


            public int Status { get; set; }

            public string User { get; set; }

            public string Time { get; set; }

            public string Note { get; set; }

            public IpInfo(int status, string user, string time, string note)
            {

                Status = status;
                User = user;
                Time = time;
                Note = note;

            }

        }


        /// <summary>
        /// 自定义IP信息2
        /// </summary>
        public class IpInfos
        {
            public int Ip { get; set; }

            public IpInfo IpInfo { get; set; }

            public IpInfos(int ip,IpInfo ipInfo)
            {
                Ip = ip;
                IpInfo = ipInfo;

            }

        }


        
        /// <summary>
        /// IP段信息
        /// </summary>
        public class IpsInfo
        {
            public string IpSegment { get; set; }

            public string Mask { get; set; }

            public string Description { get; set; }

            public string Notes { get; set; }

            public string Allocation { get; set; }

            public string Id { get; set; }


            public IpsInfo(string ipSegment, string mask, string description, string notes, string allocation, string id)
            {
                IpSegment = ipSegment;

                Mask = mask;

                Description = description;

                Notes = notes;

                Allocation = allocation;

                Id = id;

            }


        }


    }
}
