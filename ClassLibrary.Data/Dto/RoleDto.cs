using System;
using System.Collections.Generic;

namespace ClassLibrary.Data.Dto
{
    [Serializable]
    public sealed class RoleDto
    {
        public RoleDto()
        {
            Modules = new List<ModuleDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<ModuleDto> Modules { get; set; }
    }
}