using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qu.Dto.KitUser;

public class KitUser
{
    public int userid {  get; set; }
    public short kitid { get; set; }
    public int kit_detailid { get; set; }
    public bool is_custom_kit { get; set; }
}
