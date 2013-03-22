using System;

namespace ClassLibrary.Data.Dto
{
    [Serializable]
    public sealed class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}