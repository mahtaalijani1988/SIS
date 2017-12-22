using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Setting
{
    public class UpdateOptionModel
    {
        public string Department_Name { get; set; }
        public string Start_Election_Date { get; set; }
        public string Start_Election_Time { get; set; }
        public string End_Election_Date { get; set; }
        public string End_Election_Time { get; set; }

        public string Start_Remove_Date { get; set; }
        public string Start_Remove_Time { get; set; }
        public string End_Remove_Date { get; set; }
        public string End_Remove_Time { get; set; }

        public string Student_max_Unit { get; set; }
        public string Term_Name { get; set; }
        public DomainClasses.Entities.Term Term { get; set; }
    }
}