using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Base
    {
        public const int InvalidId = -1;

        public int Id { get; set; } = InvalidId;
    }
}