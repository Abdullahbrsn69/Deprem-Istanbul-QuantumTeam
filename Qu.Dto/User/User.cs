using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Qu.Dto;

public class User
{
    public int userid { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string id_no { get; set; }
    public string phone { get; set; }
    public short gender_type { get; set; }
    public int placeid { get; set; }
}