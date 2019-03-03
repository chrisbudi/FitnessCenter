using System.Collections.Generic;

namespace ViewModel.Menu
{
    //    public class SideBar
    //    {
    //        public string Title { get; set; }
    //        public string Icon { get; set; }
    //        public bool IsActive { get; set; }
    //        public int FormId { get; set; }
    //        public IEnumerable<SidebarItem> SideBars { get; set; }
    //    }


    public class BaseMenu
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int FormId { get; set; }
    }

    public class SideNav : BaseMenu
    {
        public IEnumerable<SubNav> Nav { get; set; }

    }

    public class SubNav : BaseMenu
    {
        public string Module { get; set; }
        public string MasterModule { get; set; }
        public IEnumerable<SidebarItem> NavDetail { get; set; }
    }

    public class SubNavChild : SubNav
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? GrandParentId { get; set; }
    }


    public class SidebarItem
    {
        public string IconClass { get; set; }
        public bool IsActive { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string urlParameter { get; set; }
    }
}
