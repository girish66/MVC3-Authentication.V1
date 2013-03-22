using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ClassLibrary.Data.Extensions;

namespace ClassLibrary.Data.Dto
{
    [Serializable]
    public sealed class UserDto
    {
        public UserDto()
        {
            Roles = new List<RoleDto>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastVisit { get; set; }
        public bool Locked { get; set; }
        public bool Approved { get; set; }
        public DateTime? LastPasswordFailure { get; set; }
        public int? PasswordFailures { get; set; }
        public DateTime? LastLockout { get; set; }
        public List<RoleDto> Roles { get; set; }

        public UserDto GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException();
            var userDto = new UserDto();
            using (var context = new AUTHORIZEV1())
            {
                User user = context.Users.FirstOrDefault(x => x.Username == userName);
                if (user != null)
                {
                    userDto.Id = user.Id;
                    userDto.Username = user.Username;
                    userDto.EmailAddress = user.EmailAddress;
                    // userDto.LastVisit = userDto.LastPasswordFailure = userDto.LastLockout = userDto.Created = DateTime.UtcNow;

                    foreach (Role role in user.Roles)
                    {
                        var roleDto = new RoleDto {Id = role.Id, Name = role.Name};
                        foreach (RoleModulePermission rmp in role.RoleModulePermissions.DistinctBy(v => v.ModuleId))
                        {
                            roleDto.Modules.Add(new ModuleDto {Id = rmp.ModuleId, Name = rmp.Module.Name});
                        }

                        foreach (ModuleDto module in roleDto.Modules)
                        {
                            var permission = role.RoleModulePermissions.Where(x => x.ModuleId == module.Id).Aggregate(0, (current, rmp) => current + (rmp.PermissionId > 2 ? (int) Math.Pow(2, rmp.PermissionId - 1) : rmp.PermissionId));
                            if (permission > 0)
                            {
                                module.Permissions.Add(new PermissionDto
                                    {
                                        Id = permission,
                                        // Name = rmp.Permission.Name
                                    });
                            }
                        }
                        userDto.Roles.Add(roleDto);
                    }
                }
            }
            return userDto;
        }

        public static string ToXml(UserDto userDto)
        {
            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof (UserDto));
            serializer.Serialize(writer, userDto);
            writer.Close();
            return writer.ToString();
        }

        public static UserDto CreateUserDto(string userDtoXml)
        {
            var serializer = new XmlSerializer(typeof (UserDto));
            var userDto = (UserDto) serializer.Deserialize(new StringReader(userDtoXml));
            return userDto;
        }
    }
}