﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Userrole1
{
    public int Userroleid { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Rolesandpermission> Rolesandpermissions { get; set; } = new List<Rolesandpermission>();
}
