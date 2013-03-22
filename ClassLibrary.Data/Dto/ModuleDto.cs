using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Data.Dto
{
    [Serializable]
    public sealed class ModuleDto
    {
        public ModuleDto()
        {
            Permissions = new List<PermissionDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }

        public ModuleDto GetModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException();
            var moduleDto = new ModuleDto();
            using (var context = new AUTHORIZEV1())
            {
                Module module = context.Modules.FirstOrDefault(x => x.Name == moduleName);
                if (module != null)
                {
                    moduleDto.Id = module.Id;
                    moduleDto.Name = module.Name;
                    {
                        foreach (Permission p in module.Permissions)
                        {
                            moduleDto.Permissions.Add(new PermissionDto
                                {
                                    Id = p.Id,
                                    Name = p.Name
                                });
                        }
                    }
                }
                return moduleDto;
            }
        }
    }
}