using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class SessionLogin
    {
        public SBS_COMS_USER aspnetuser { get; set; }
        public CM_BP_ALL cm_bp_all { get; set; }
    }
}