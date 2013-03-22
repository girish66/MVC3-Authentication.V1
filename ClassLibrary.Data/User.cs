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
    
    public partial class User
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> LastVisit { get; set; }
        public bool Locked { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> LastPasswordFailure { get; set; }
        public Nullable<int> PasswordFailures { get; set; }
        public Nullable<System.DateTime> LastLockout { get; set; }
    
        public virtual ICollection<Role> Roles { get; set; }
    }
}
