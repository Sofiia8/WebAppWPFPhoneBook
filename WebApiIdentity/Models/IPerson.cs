
namespace WebApiIdentity.Models
{
    internal interface IPerson
    {
        int ID { get; set; }
        string Surname { get; set; }
        string Name { get; set; }
        string Secondname { get; set; }
        string Phonenum { get; set; }
        string Address { get; set; }
        string Description { get; set; }
    }
}
