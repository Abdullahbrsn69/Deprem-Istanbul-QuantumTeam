using System.ComponentModel.DataAnnotations;

namespace Qu.Dto.Place;

public class Place
{
    public int placeid { get; set; }
    public string name { get; set; }
    public string address { get; set; }
    public int managerid { get; set; }
}