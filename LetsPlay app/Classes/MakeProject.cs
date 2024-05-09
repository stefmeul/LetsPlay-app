using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsPlay_app.Classes
{
    internal class MakeProject
    {
         
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectLocation {  get; set; }
        
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }

        public double ProjectBudget { get; set; }
        public List<Role> ProjectRoles { get; set; } = new List<Role>();
    }

    internal class Role
    {
        public string RoleName { get; set; }
        public double RolePercentage { get; set; }
    }
}
