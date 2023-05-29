using WebApplicationPhoneBook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPhoneBook
{
    public static class InitPhoneBooks
    {
        public static async Task InitializeAsync(ApplicationContext applicationContext)
        {
            if (!applicationContext.Phonebook.Any())
            {
               await applicationContext.Phonebook.AddRangeAsync(
                    new Person
                    {
                        //ID = 1,
                        Surname = "Иванов",
                        Name = "Иван",
                        Secondname = "Иванович",
                        Phonenum = "9169009897",
                        Address = "г. Москва, ул. Черничная, д. 5",
                        Description = "появился в базе 01.01.2019, работает в Microsoft"
                    },
                    new Person
                    {
                        Surname = "Ивановa",
                        Name = "Людмила",
                        Secondname = "Семеновна",
                        Phonenum = "9158999897",
                        Address = "г. Москва, ул. Черничная, д. 5",
                        Description = "появился в базе 01.02.2019"
                    },
                    new Person
                    {
                        Surname = "Никифоров",
                        Name = "Вадим",
                        Secondname = "Борисович",
                        Phonenum = "9997999999",
                        Address = "г. Москва, ул. Яблочная, д. 59",
                        Description = "появился в базе 01.02.2021"
                    },
                    new Person
                    {
                        Surname = "Александрова",
                        Name = "Екатерина",
                        Secondname = "Андреевна",
                        Phonenum = "9267894561",
                        Address = "г. Москва, ул. Яблочная, д. 118",
                        Description = "появился в базе 01.03.2021"
                    });
                await applicationContext.SaveChangesAsync();
            }
        }
    }
}
