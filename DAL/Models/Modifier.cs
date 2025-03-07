﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Modifier
{
    public int Modifierid { get; set; }

    public int? ModifierGroupId { get; set; }

    public string Modifiername { get; set; } = null!;

    public decimal? Rate { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ModifierGroup? ModifierGroup { get; set; }
}
