using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace ClassLibrary.Data
{
    public sealed class AuthorizePrincipal : IPrincipal
    {
        private readonly IList<AuthorizePrincipalRole> _authorizePrincipalRoles;
        private readonly IIdentity _identity;

        public AuthorizePrincipal()
        {
            _authorizePrincipalRoles = new List<AuthorizePrincipalRole>();
        }

        public AuthorizePrincipal(IIdentity identity, IList<AuthorizePrincipalRole> authorizePrincipalRoles) : this()
        {
            _identity = identity;
            _authorizePrincipalRoles = authorizePrincipalRoles;
        }


        public IList<AuthorizePrincipalRole> AuthorizePrincipalRoles
        {
            get { return _authorizePrincipalRoles; }
        }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return (AuthorizePrincipalRoles.FirstOrDefault(x => x.R == role) != null);
        }


        public bool IsInRole(Modules m, Permissions p, Operators operators)
        {
            List<byte> permissions = (from Permissions function in Enum.GetValues(typeof(Permissions))
                               where (p & function) == function
                               select (byte)function).ToList();
            byte module = (byte)m;
            if(operators == Operators.Or)
                return (IsInOneRole(module, permissions));
            return (IsInAllRole(module, permissions));
        }

        private bool IsInOneRole(byte module, IList<byte> permissions)
        {
            bool isAuthorized = false;
            foreach (var permission in permissions)
            {
                var m = Convert.ToInt32(module);
                var p = Convert.ToInt32(permission);
                // p = (int)(p > 2 ? Math.Pow(2, p) : p);
                // p = (int)(p > 2 ? Math.Log(p)/Math.Log(2) : p);
                isAuthorized = IsAuthorized(m, p);
                if (isAuthorized)
                    break;
            }
            return isAuthorized;
        }

        private bool IsInAllRole(byte module, IList<byte> permissions)
        {
            bool isAuthorized = false;
            foreach (var permission in permissions)
            {
                var m = Convert.ToInt32(module);
                var p = Convert.ToInt32(permission);
                // p = (int)(p > 2 ? Math.Pow(2, p) : p);
                // p = (int)(p > 2 ? Math.Log(p) / Math.Log(2) : p);
                isAuthorized = IsAuthorized(m, p);
                if (!isAuthorized)
                    break;
            }
            return isAuthorized;
        }

        public bool IsAuthorized(int module, int permission)
        {
            // return AuthorizePrincipalRoles.FirstOrDefault(x => x.M == module &&  x.P == permission) != null;
            return AuthorizePrincipalRoles.FirstOrDefault(x => x.M == module && ((x.P & permission) == permission)) != null;
        }

        #endregion
    }

    public sealed class AuthorizePrincipalRole
    {
        public string R { get; set; }
        public int M { get; set; }
        public int P { get; set; }

        public AuthorizePrincipalRole()
        {
        }

        public AuthorizePrincipalRole(string roleName, int moduleId, int permissionId)
        {
            R = roleName; /*.Substring(0,1).ToUpper()*/
            M = moduleId;
            P = permissionId;
        }

        public static string ToJson(IList<AuthorizePrincipalRole> authorizePrincipalRoles)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(authorizePrincipalRoles);
        }

        public static IList<AuthorizePrincipalRole> CreateAuthorizePrincipalRoles(string authorizePrincipalRolesJson)
        {
            return !string.IsNullOrEmpty(authorizePrincipalRolesJson) ? new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<IList<AuthorizePrincipalRole>>(authorizePrincipalRolesJson) : null;
        }
    }

    //// Saying byte to prevent it from using 4 bytes for storage
    //public enum Modules : byte
    //{
    //    // This should be ok since we are not doing an | and then use &
    //    Program = 1,
    //    Patient = 2,
    //    Application = 3
    //}

    //[Flags]
    //public enum Permissions
    //{
    //    // Since we are doing an | 1,2,4,8,16,32,64......
    //    // We are type casting this to byte on type so in effect it can only accomidate 256 but if you want more change the type cast to int
    //    Read = 1,
    //    Write = 2,
    //    Delete = 4,
    //    Approve = 8,
    //    Deny = 16,
    //    Activate = 32
    //}

    //[Flags]
    //public enum Operators : byte
    //{
    //    All = 1,
    //    Or = 2
    //}
}