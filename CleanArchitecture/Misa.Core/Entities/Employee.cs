using Misa.Core.Attributes;
using Misa.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities
{
    public class Employee: Entity
    {
        public Guid EmployeeId { get; set; }
        [MISARequired]
        public string EmployeeCode { get; set; }
        [MISARequired]
        [MISAMaxLength]
        public string Fullname { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentityAddress { get; set; }
        public string PositionName { get; set; }
        [MISARequired]
        public Guid? DepartmentId { get; set; }
        public string CustomerGroupName { get; set; }
        public string AccountsReceivableDebt { get; set; }
        public string AccountsPayable { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string AddressBank { get; set; }
        public string Address { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string TelePhoneNumber { get; set; }

        public string Email { get; set; }
        public DateTime IdentifyDate { get; set; }

        public Boolean? IsCustomer { get; set; }
        public Boolean? IsSupplier { get; set; }
        public int? State { get; set; }

        public string GenderName
        {
            get
            {
                switch (Gender)
                {
                    case Enum.Gender.Male:
                        return Properties.Resources.GenderName_Male;
                    case Enum.Gender.Female:
                        return Properties.Resources.GenderName_Female;
                    case Enum.Gender.Other:
                        return Properties.Resources.GenderName_Other;
                    default:
                        return "Không xác định";
                }
            }
        }



        public string DepartmentName
        {
            get
            {
                var departmentId1 = new Guid("35e773ea-5d44-2dda-26a8-6d513e380bde");
                var departmentId2 = new Guid("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
                var departmentId3 = new Guid("45ac3d26-18f2-18a9-3031-644313fbb055");
                var departmentId4 = new Guid("78aafe4a-67a7-2076-3bf3-eb0223d0a4f7");
                var departmentId5 = new Guid("7c4f14d8-66fb-14ae-198f-6354f958f4c0");
                
                if (DepartmentId == departmentId1)
                {
                    return "Phòng Nhân sự";
                }
                else if(DepartmentId == departmentId2)
                {
                    return "Phòng Kinh doanh";
                }
                else if (DepartmentId == departmentId3)
                {
                    return "Phòng Marketing";
                }
                else if (DepartmentId == departmentId4)
                {
                    return "Phòng Công nghệ";
                }
                else if (DepartmentId == departmentId4)
                {
                    return "Phòng Sale";
                }

                else
                {
                    return "Không xác định";
                }

            }
        }

    }
}
