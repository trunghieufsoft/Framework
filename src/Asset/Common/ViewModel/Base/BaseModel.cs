using System;

namespace Asset.Common.ViewModel
{
    public abstract class BaseModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
