//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Permission
    {
        public Permission()
        {
            this.RoleModulePermissions = new HashSet<RoleModulePermission>();
            this.Modules = new HashSet<Module>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<RoleModulePermission> RoleModulePermissions { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
