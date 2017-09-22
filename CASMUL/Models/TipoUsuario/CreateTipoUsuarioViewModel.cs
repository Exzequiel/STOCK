﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.TipoUsuario
{
    public class CreateTipoUsuarioViewModel
    {
        [Display(Name ="Descripción")]
        [Required]
        public string Description { get; set; }
    }
}