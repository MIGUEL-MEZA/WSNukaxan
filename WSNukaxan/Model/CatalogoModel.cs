﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace WSNukaxan.Model
{
    

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogoModel
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        
    }

}
